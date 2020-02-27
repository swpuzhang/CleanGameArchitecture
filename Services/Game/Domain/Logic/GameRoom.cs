using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Commons.Domain.Managers;
using MassTransit;
using AutoMapper;
using System.Timers;
using Commons.Enums;
using CommonModels.GameModels;
using Commons.Threading;
using Commons.Models;
using GameMessages.MqCmds;
using Game.Domain.Manager;
using GameMessages.RoomMessages;
using CommonMessages.MqCmds;
using Game.Domain.Models;
using GameMessages.MqEvents;

namespace Game.Domain.Logic
{
    
    public class GameRoom
    {
        private readonly List<GameSeat> _seats = new List<GameSeat>();
        private readonly Dictionary<long, GamePlayer> _playerInfos = new Dictionary<long, GamePlayer>();
        private readonly MqManager _mqManager;
        private readonly IBusControl _bus;
 
        private readonly IMapper _mapper;
        private List<PokerCard> _bottomCards;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        #region 成员
        public RoomTypes RoomType { get; private set; }
        public string RoomId { get; private set; }
        public long Blind { get; private set; }
        public long MinCoins { get; private set; }
        public long MaxCoins { get; private set; }
        public int TipsPersent { get; private set; }

        public long MinCarry { get; private set; }

        public long MaxCarry { get; private set; }

        public int SeatCount { get; private set; }

        public int ActiveSeatNum { get; private set; } = -1;

        public GameStatusLogic _statusInfo = new GameStatusLogic();

        private int _dealerSeatIndex = 0;

        private int _secondDealer = 0;

        private long _maxAdd = 0;

        private int lastWinSeat = -1;

        private readonly CoinsPool _coinsPool = new CoinsPool();

        private readonly GameLog _gameLog = new GameLog();
        public void Clean()
        {
            ActiveSeatNum = -1;
            _dealerSeatIndex = -1;
            _secondDealer = -1;
            _maxAdd = 0;
            _coinsPool.Clean();
            _bottomCards = null;
            foreach (var seat in _seats)
            {
                seat.Clean();
            }

        }

        #endregion

        #region 初始化
        public GameRoom(
            RoomTypes roomType,
           string roomId,
           long blind,
           int seatCount,
           long minCoins,
           long maxCoins,
           int tipsPersent,
           MqManager mqManager,
           IBusControl bus, IMapper mapper, long minCarry, long maxCarry)
        {
            RoomType = roomType;
            RoomId = roomId;
            Blind = blind;
            MinCoins = minCoins;
            MaxCoins = maxCoins;
            TipsPersent = tipsPersent;
            SeatCount = seatCount;
            _mqManager = mqManager;
            _bus = bus;
            _mapper = mapper;
            MinCarry = minCarry;
            MaxCarry = maxCarry;
            _timer.Interval = 60;
            _timer.AutoReset = true;
            _timer.Elapsed += CheckPlayerAlive;
            _timer.Start();
        }
        public void Init()
        {
            for (int i = 0; i < SeatCount; ++i)
            {
                _seats.Add(new GameSeat(i));
            }
        }

        public GamePlayer GetPlayer(long id)
        {
            _playerInfos.TryGetValue(id, out var player);
            return player;
        }

        public void CheckPlayerAlive(object sender, ElapsedEventArgs e)
        {
            OneThreadSynchronizationContext.Instance.Post(x =>
            {
                List<GamePlayer> willLeavePlayer = new List<GamePlayer>();
                foreach (var player in _playerInfos)
                {
                    if (!player.Value.IsAlive())
                    {
                        willLeavePlayer.Add(player.Value);
                    }
                }
                foreach (var player in willLeavePlayer)
                {
                    PlayerLeave(player);
                }
            }, null);
        }
        public void EnsurDealer()
        {
            if (lastWinSeat == -1 || !_seats[lastWinSeat].IsSeated())
            {
                _dealerSeatIndex = _seats.Where(x => x.IsSeated()).First().SeatNum;
                return;
            }
            _dealerSeatIndex = lastWinSeat;
        }

        #endregion
        public int NextSeatedNum(int curNum)
        {
            int index = (curNum + 1) % _seats.Count;
            while (!_seats[index].IsSeated() && index != curNum)
            {
                index = (index + 1) % _seats.Count;
            }
            return index;
        }
        public int NextInGameNum(int curNum)
        {
            int index = (curNum + 1) % _seats.Count;
            while (!_seats[index].IsCanContinue() && index != curNum)
            {
                index = (index + 1) % _seats.Count;
            }
            return index;
        }

        public bool NextActiveNum()
        {
            if (ActiveSeatNum == -1)
            {
                ActiveSeatNum = _dealerSeatIndex;
                if (_seats[ActiveSeatNum].IsCanActive(_maxAdd))
                {
                    return true;
                }
            }
            int index;
            index = (ActiveSeatNum + 1) % _seats.Count;
            //对于已经allin 或者加注过的跳过
            while (!_seats[index].IsCanActive(_maxAdd))
            {
                index = (index + 1) % _seats.Count;
                if (index == ActiveSeatNum)
                {
                    break;
                }
            }
            if (index == ActiveSeatNum)
            {
                return false;
            }
            ActiveSeatNum = index;
            return true;
        }
        public bool IsGameCanStart()
        {
            if (_statusInfo.IsGameCanStart() && GetPlayerCount() > 1)
            {
                return true;
            }

            return false;
        }
        public int GetPlayerCount()
        {
            int count = 0;
            foreach (var seat in _seats)
            {
                if (seat.IsSeated())
                {
                    ++count;
                }
            }
            return count;
        }

        public int GetInGameCount()
        {
            int count = 0;
            foreach (var seat in _seats)
            {
                if (seat.IsCanContinue())
                {
                    ++count;
                }
            }
            return count;
        }

        public GameSeat GetEmptySeat()
        {
            return _seats.Where(x => !x.IsSeated()).FirstOrDefault();
        }
        public async Task<WrappedResponse<JoinGameRoomMqResponse>> JoinRoom(long id)
        {
            var accountInfo = _mqManager.GetAccountInfo(id);
            var moneyInfo = _mqManager.BuyIn(id, MinCarry, MaxCarry);
            await Task.WhenAll(accountInfo, moneyInfo);
            if (moneyInfo.Result == null)
            {
                return new WrappedResponse<JoinGameRoomMqResponse>(ResponseStatus.Error, new List<string>() { "make player error " }, null);
            }
            _playerInfos.TryGetValue(id, out var player);
            //已经在房间直接返回成功
            if (player != null)
            {
                if (accountInfo.Result == null)
                {
                    player.UpdateInfo(id, "", "", 0, "", moneyInfo.Result.CurCoins, 
                        moneyInfo.Result.CurDiamonds, moneyInfo.Result.Carry);
                }
                else
                {
                    player.UpdateInfo(id, accountInfo.Result.PlatformAccount, accountInfo.Result.UserName,
                        accountInfo.Result.Sex, accountInfo.Result.HeadUrl, moneyInfo.Result.CurCoins,
                        moneyInfo.Result.CurDiamonds, moneyInfo.Result.Carry);
                }
            }
            else
            {
                  player =  MakePlayer(id, accountInfo.Result, moneyInfo.Result);
            }
       
            if (player == null)
            {
                return new WrappedResponse<JoinGameRoomMqResponse>(ResponseStatus.Error, new List<string>() { "make player error " }, null);
            }

            if (!player.IsSeated())
            {
                
                GameSeat seat = GetEmptySeat();
               
                if (seat == null)
                {
                    _playerInfos.Remove(id);
                    return new WrappedResponse<JoinGameRoomMqResponse>(ResponseStatus.Error, new List<string>() { "room is full " }, 
                        new JoinGameRoomMqResponse(id, RoomId, GameRoomManager.gameKey, SeatCount, Blind));
                }
                player.Seat(seat);
                BroadCastMessage(new PlayerSeatedEvent(player.Id, player.UserName,
                    player.Coins, player.Diamonds, player.SeatInfo.SeatNum, player.Carry), 
                    "PlayerSeatedEvent");
                //判断房间人数>2 人， 而且是正在准备状态， 开始牌局， 否者旁观
                if (IsGameCanStart())
                {
                    _statusInfo.WaitForNexStatus(OnGameReady, GameStatus.ready, GameTimerConfig.ReadyWait);
                }
            }
            
            return new WrappedResponse<JoinGameRoomMqResponse>(ResponseStatus.Success, null,
                new JoinGameRoomMqResponse(id, RoomId, GameRoomManager.gameKey, GetPlayerCount(), Blind));
        }
        public void BroadCastMessage(object request, string reqName)
        {
            foreach (var player in _playerInfos)
            {
                GameServerRequest GameRequest = new GameServerRequest(player.Key, request,
                    reqName, GameRoomManager.gameKey, RoomId);
                _bus.Publish<GameServerRequest>(GameRequest);
            }
        }
        public void BroadCastMessage(object request, string reqName,  GamePlayer player)
        {
            GameServerRequest GameRequest = new GameServerRequest(player.Id, request, reqName,
                    GameRoomManager.gameKey, RoomId);
                _bus.Publish<GameServerRequest>(GameRequest);
        }

        #region 超时处理
        public void OnGameIdle()
        {
            if (IsGameCanStart())
            {
                _statusInfo.WaitForNexStatus(OnGameReady, GameStatus.ready, GameTimerConfig.ReadyWait);
            }
        }

        public void OnGameReady()
        {
            if (IsGameCanStart())
            {
                _statusInfo.WaitForNexStatus(OnGamePlaying, GameStatus.playing, 0);
            }
            else
            {
                _statusInfo.WaitForNexStatus(OnGameIdle, GameStatus.Idle, 0);
            }
        }
        public void OnGamePlaying()
        {
            _gameLog.GameStart(Guid.NewGuid().ToString(), Blind, RoomId, RoomType);
            Clean();
            //定庄
            EnsurDealer();

            //发牌
            CardDealer.DealCard(GetPlayerCount(), out var allUserCards, out _bottomCards);

            //从庄家开始发牌,下注
            int index = _dealerSeatIndex;
            List<int> dealerOrder = new List<int>();
            List<long> carrys = new List<long>();
            GameStartAct act = new GameStartAct();
            do
            {
                dealerOrder.Add(index);
                _seats[index].DealCard(allUserCards.Last(), Blind);
                _coinsPool.PlayerBetCoins(_seats[index].SeatNum, Blind);
                var player = _seats[index].InGamePlayerInfo;
                carrys.Add(player.Carry);
                act.AddPlayer(new GameStartAct.PlayerInfo(player.Id, player.Carry,
                   index, allUserCards.Last()));
                allUserCards.RemoveAt(allUserCards.Count - 1);
               
            } while ((index = NextSeatedNum(index)) != _dealerSeatIndex);
            _gameLog.AddGameAction(act);
            _coinsPool.BlindPool(dealerOrder.Count, Blind);

            foreach (var player in _playerInfos)
            {
                DealCardsEvent dealCard = new DealCardsEvent(_dealerSeatIndex, dealerOrder,
                    CardDealer.UserCardsCount,
                    player.Value.SeatInfo?.HandCards, Blind, carrys);
                BroadCastMessage(dealCard, "DealCardsEvent", player.Value);
            }
          
           
            //等待发牌结束
            _statusInfo.WaitForNexStatus(OnDealingCards, GameStatus.playing, GameTimerConfig.DealCard);
        }
        public void OnDealingCards()
        {

            ActivePlayer(true);
        }

        public void OnDealingThirdCard()
        {
            ActivePlayer(false);
        }

        public async void OnGameOver()
        {
            List<Task<MoneyMqResponse>> tasks = new List<Task<MoneyMqResponse>>();
            List<KeyValuePair<GameSeat, Task<MoneyMqResponse>>> seatTasks = new List<KeyValuePair<GameSeat, Task<MoneyMqResponse>>>();
            //检查玩家携带是否足够
          
            foreach (var seat in _seats)
            {
                if (!seat.IsSeated())
                {
                    continue;
                }
                var player = seat.PlayerInfo;
                if (player.Carry >=  MinCarry)
                {
                    continue;
                }
                var moneyInfo = _mqManager.BuyIn(player.Id, MinCarry, MaxCarry);
                tasks.Add(moneyInfo);
                seatTasks.Add(new KeyValuePair<GameSeat, Task<MoneyMqResponse>>(seat, moneyInfo));
            }
            await Task.WhenAll(tasks);
            foreach (var oneResult in seatTasks)
            {
                if (!oneResult.Key.IsSeated() || oneResult.Key.PlayerInfo.Carry >= MinCarry 
                    || oneResult.Key.PlayerInfo.Id != oneResult.Value.Result.Id)
                {
                    continue;
                }
                if (oneResult.Value.Result == null)
                {
                    //买入失败
                    PlayerStandup(oneResult.Key.PlayerInfo);
                }
                else
                {
                    oneResult.Key.PlayerInfo.BuyIn(oneResult.Value.Result.CurCoins, oneResult.Value.Result.CurDiamonds,
                        oneResult.Value.Result.Carry);
                    BroadCastMessage(new PlayerBuyInEvent(oneResult.Key.PlayerInfo.SeatInfo.SeatNum, oneResult.Key.PlayerInfo.Carry),
                        "PlayerBuyInEvent");
                }
            }
            _statusInfo.WaitForNexStatus(OnGameIdle, GameStatus.Idle, 1000);
        }

        public void OnPlayerOpt()
        {
            GameSeat seat = _seats[ActiveSeatNum];
            //超时, 默认过牌,否则弃牌,然后激活下一个玩家
            if (seat.BetedCoins < _maxAdd)
            {
                PlayerDrop(seat.PlayerInfo);
            }
            else
            {
                PlayerPass(seat.PlayerInfo);
            }
        }
        #endregion


        public void PlayerDrop(GamePlayer player)
        {
            BroadCastMessage(new DropEvent(player.SeatInfo.SeatNum), "DropEvent");
            _gameLog.AddGameAction(new DropAct(player.Id, player.SeatInfo.SeatNum));
            player.SeatInfo.Drop();
            if (GetInGameCount() < 2)
            {
                GameAccount();
            }
            else
            {
                
                ActivePlayer(_statusInfo.IsFirstRound());
            }
            
        }

        public void PlayerPass(GamePlayer player)
        {
            BroadCastMessage(new PassEvent(player.SeatInfo.SeatNum), "PassEvent");
            _gameLog.AddGameAction(new PassAct(player.Id, player.SeatInfo.SeatNum));
            player.SeatInfo.Follow(0);
            ActivePlayer(_statusInfo.IsFirstRound());
        }

        public bool PlayerFollow(GamePlayer player, out long followChips)
        {
            followChips = _maxAdd - player.SeatInfo.BetedCoins;
            followChips = followChips <= player.Carry ? followChips : player.Carry;
            if (player.Carry == 0 || player.SeatInfo.BetedCoins >= _maxAdd 
                    || !player.SeatInfo.Follow(followChips))
            {
                return false;
            }
            _coinsPool.PlayerBetCoins(player.SeatInfo.SeatNum, followChips);
            BroadCastMessage(new FollowEvent(player.SeatInfo.SeatNum, followChips, player.Carry), "FollowEvent");
            _gameLog.AddGameAction(new FollowAct(player.Id, player.SeatInfo.SeatNum, followChips, player.Carry));
            ActivePlayer(_statusInfo.IsFirstRound());
            return true;
        }

        public bool PlayerAdd(GamePlayer player, long addChips)
        {
            if (player.Carry < addChips || player.SeatInfo.BetedCoins > _maxAdd
                || !player.SeatInfo.Add(addChips))
            {
                return false;
            }
            _coinsPool.PlayerBetCoins(player.SeatInfo.SeatNum, addChips);
            BroadCastMessage(new AddEvent(player.SeatInfo.SeatNum, addChips, player.Carry), "AddEvent");
            _gameLog.AddGameAction(new AddAct(player.Id, player.SeatInfo.SeatNum, addChips, player.Carry));
            ActivePlayer(_statusInfo.IsFirstRound());
            return true;
        }

        public GamePlayer MakePlayer(long id, GetAccountInfoMqResponse accountInfo, MoneyMqResponse moneyInfo)
        {
            GamePlayer player;
            if (accountInfo == null)
            {
                player = new GamePlayer(id, moneyInfo.CurCoins, moneyInfo.CurDiamonds, moneyInfo.Carry);
            }
            else
            {
                player = new GamePlayer(id, accountInfo.PlatformAccount, accountInfo.UserName, accountInfo.Sex,
                    accountInfo.HeadUrl, moneyInfo.CurCoins, moneyInfo.CurDiamonds, moneyInfo.Carry);
            }
            _playerInfos.Add(id, player);
            return player;
        }
        public long FollowCoins(GameSeat seat)
        {
            long followChips = 0;
            if (seat.BetedCoins < _maxAdd)
            {
                if (seat.PlayerInfo.Carry >= _maxAdd)
                {
                    followChips = _maxAdd;
                }
                else
                {
                    followChips = seat.PlayerInfo.Carry;
                }

            }
            return followChips;

        }
        public void ActivePlayer(bool isFirstRound)
        {
            
            if (!NextActiveNum())
            {
                if (isFirstRound)
                {
                    StartSecondRound();
                }
                else
                {
                    //比牌结算
                    GameAccount();
                }
            }
            else
            {
                var seat = _seats[ActiveSeatNum];
                ActiveEvent actEvent = new ActiveEvent(ActiveSeatNum, FollowCoins(seat));
                GameActiveAct act = new GameActiveAct(seat.InGamePlayerInfo.Id, ActiveSeatNum);
                _gameLog.AddGameAction(act);
                BroadCastMessage(actEvent, "ActiveEvent");
                _statusInfo.WaitForNexStatus(OnPlayerOpt, isFirstRound?GameStatus.FirstRound : GameStatus.SecondRound, GameTimerConfig.BetChips);
            }
        }

        public void FindSecondRondDealer()
        {
            int index = _dealerSeatIndex;
            while (!_seats[index].IsCanContinue())
            {
                index = (index + 1) % _seats.Count;
                if (index == _dealerSeatIndex)
                {
                    break;
                }
            }
            _secondDealer = index;
        }

        public void StartSecondRound()
        {
            //重置下下注, 激活玩家
            _maxAdd = 0;
            ActiveSeatNum = -1;
            List<int> order = new List<int>();
            FindSecondRondDealer();
            _coinsPool.FirstEndPool();
            int index = _secondDealer;
            do
            {
                _seats[index].SecondRoundStarted(_bottomCards.Last());
                _bottomCards.RemoveAt(_bottomCards.Count - 1);
                order.Add(index);
            } while ((index = NextInGameNum(index)) != _secondDealer);
            
            foreach (var one in _playerInfos)
            {
                DealThirdCardEvent thirdEvent = null;
                if (!one.Value.IsSeated())
                {
                    thirdEvent = new DealThirdCardEvent(_coinsPool._pool,
                        null, order, 0, 0);
                    
                }
                else
                {
                    if (one.Value.SeatInfo.IsCanContinue())
                    {
                        thirdEvent = new DealThirdCardEvent(_coinsPool._pool,
                        one.Value.SeatInfo.HandCards.Last(), order, (int)one.Value.SeatInfo.Combination.ComType,
                        one.Value.SeatInfo.Combination.Point);
                    }
                    else
                    {
                        thirdEvent = new DealThirdCardEvent(_coinsPool._pool,
                        null, order, 0, 0);
                    }
                }
                BroadCastMessage(thirdEvent, "DealThirdCardEvent", one.Value);
            }

            _statusInfo.WaitForNexStatus(OnDealingThirdCard, GameStatus.playing, GameTimerConfig.DealThirdCard);
        }

        public void PlayerStandup(GamePlayer player)
        {
            PlayerStanupEvent standupEvent = new PlayerStanupEvent(player.Id, player.SeatInfo.SeatNum);
            BroadCastMessage(standupEvent, "PlayerStanupEvent");
            _gameLog.AddGameAction(new StandupAct(player.Id, player.SeatInfo.SeatNum));
            player.Standup();

            //返还携带
            _bus.Publish(new AddMoneyMqCmd(player.Id, player.Carry, -player.Carry, AddReason.None));

            //告诉matchingserver该玩家已经站起
            _bus.Publish(new LeaveGameRoomMqEvent(player.Id, RoomId, GameRoomManager.gameKey,
                GetPlayerCount(), Blind, GameRoomManager.matchingGroup));

            if (GetInGameCount() < 2)
            {
                GameAccount();
            }
            else
            {
                if (ActiveSeatNum == player.SeatInfo.SeatNum)
                {
                    ActivePlayer(_statusInfo.IsFirstRound());
                }
            }
            //返还携带
           
            
        }

        public void GameAccount()
        {
            _coinsPool.SecondEndPool();
            GameOverEvent gameOverEvent = GameAccounter.Caculate(_seats, _coinsPool.GetUserPools(), out lastWinSeat);
            BroadCastMessage(gameOverEvent, "GameOverEvent");

            foreach (var seat in _seats)
            {
                if (seat.InGamePlayerInfo == null)
                {
                    continue;
                }

                if (seat.WinCoins > 0)
                {
                    //说明这个赢的玩家已经走了, 那么将钱还是加给他
                    if (seat.InGamePlayerInfo != seat.PlayerInfo)
                    {
                        _bus.Publish(new AddMoneyMqCmd(seat.InGamePlayerInfo.Id, seat.WinCoins, 0, AddReason.GameAccount));
                    }
                    else
                    {
                        seat.PlayerInfo.AddCarry(seat.WinCoins);
                        _bus.Publish(new AddMoneyMqCmd(seat.InGamePlayerInfo.Id, 0, seat.WinCoins, AddReason.GameAccount));
                    }
                }
                else
                {
                    _bus.Publish(new AddMoneyMqCmd(seat.InGamePlayerInfo.Id, 0, -seat.TotalBetedCoins, AddReason.GameAccount));
                }
            }
            List<GameOverAct.PlayerInfo> logPlayers = gameOverEvent.PlayerInfos
                .Select(one => new GameOverAct.PlayerInfo(one.Id, one.SeatNum,
                 one.CoinsInc, one.Carry, one.Cards, one.CardType, one.Point))
                .ToList();

            GameOverAct act = new GameOverAct(gameOverEvent.WinnerSeats, gameOverEvent.WinnerPool, logPlayers);
            _gameLog.AddGameAction(act);
            _bus.Publish(new GameLogMqCommand(_gameLog));
            
            _statusInfo.WaitForNexStatus(OnGameOver, GameStatus.Idle, GameTimerConfig.GameAccount);
            Clean();
        }

        public bool IsPlayerActive(GamePlayer player)
        {
            if (player.IsSeated() && player.SeatInfo.SeatNum == ActiveSeatNum)
            {
                return true;
            }
            return false;
        }

        public void PlayerLeave(GamePlayer player)
        {
            if (player.IsSeated())
            {
                player.Standup();
            }
            _playerInfos.Remove(player.Id);
        }

        #region 消息处理
        public ToAppResponse OnApplyStandupCommand(long id, ApplyStandupCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || !player.IsSeated())
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }
            PlayerStandup(player);
            return new ToAppResponse();
        }

        public ToAppResponse OnApplyLeaveCommand(long id, ApplyLeaveCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null)
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }
            PlayerStandup(player);
            return new ToAppResponse();
        }

        public ToAppResponse OnApplyDropCommand(long id, ApplyDropCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || !IsPlayerActive(player))
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }
            PlayerDrop(player);
            return new ToAppResponse();
        }

        public ToAppResponse OnApplyPassCommand(long id, ApplyPassCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || !IsPlayerActive(player))
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }
            
            PlayerPass(player);
            return new ToAppResponse();
        }


        public ToAppResponse OnApplyFollowCommand(long id, ApplyFollowCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || !IsPlayerActive(player))
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }

            if (!PlayerFollow(player, out var _))
            {
                return new ToAppResponse(null, ResponseStatus.NoEnoughMoney, null);
            }
            
            return new ToAppResponse();
        }

        public ToAppResponse OnApplyAddCommand(long id, ApplyAddCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || !IsPlayerActive(player))
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }

            if (!PlayerAdd(player, command.AddCoins))
            {
                return new ToAppResponse(null, ResponseStatus.NoEnoughMoney, null);
            }

            return new ToAppResponse();
        }
        public ToAppResponse OnApplyStayInRoom(long id, ApplyStayInRoom command)
        {
            return new ToAppResponse();
        }

        public ToAppResponse OnApplySitDownCommand(long id, Guid gid, ApplySitdownCommand command)
        {
            _playerInfos.TryGetValue(id, out var player);
            if (player == null || player.IsSeated() || command.SeatNum >= SeatCount || _seats[command.SeatNum].IsSeated())
            {
                return new ToAppResponse(null, ResponseStatus.PlayerNotInRoom, null);
            }

            //向matching请求加入该房间
            var mqresponse = _mqManager.UserApplySit(id, RoomId, GameRoomManager.gameKey, Blind);
            mqresponse.Wait();
            var response = mqresponse.Result;
            if (response == null || response.ResponseStatus != ResponseStatus.Success)
            {
                return new ToAppResponse(null, ResponseStatus.Error, null);
            }
            var moneyMqResponse = _mqManager.BuyIn(id, MinCarry, MaxCarry);
            moneyMqResponse.Wait();
            var moneyInfo = moneyMqResponse.Result;
            if (moneyInfo == null)
            {
                //买入失败
                _bus.Publish(new UserSitFailedMqEvent(id, RoomId, GameRoomManager.gameKey, GameRoomManager.matchingGroup));
                return new ToAppResponse(null, ResponseStatus.Error, null);
            }
            player.UpdateInfo(id, player.PlatformAccount, player.UserName,
                player.Sex, player.HeadUrl, moneyInfo.CurCoins,
                moneyInfo.CurDiamonds, moneyInfo.Carry);
            player.Seat(_seats[command.SeatNum]);
            BroadCastMessage(new PlayerSeatedEvent(player.Id, player.UserName,
                player.Coins, player.Diamonds, player.SeatInfo.SeatNum, player.Carry),
                "PlayerSeatedEvent");
            //判断房间人数>2 人， 而且是正在准备状态， 开始牌局， 否者旁观
            if (IsGameCanStart())
            {
                _statusInfo.WaitForNexStatus(OnGameReady, GameStatus.ready, GameTimerConfig.ReadyWait);
            }
            
            return new ToAppResponse();
        }


        public ToAppResponse OnApplySyncGameRoomCommand(long id, ApplySyncGameRoomCommand command)
        {
            ApplySyncGameRoomResponse.GameStatus status = ApplySyncGameRoomResponse.GameStatus.Idle;
            int optLeftMs = 0;
            var interval = DateTime.Now - _statusInfo._beginTime;
            if (_statusInfo.IsActive())
            {
                status = ApplySyncGameRoomResponse.GameStatus.PlayerOpt;
               
                optLeftMs = (int)interval.TotalMilliseconds;
            }
            else if (_statusInfo.IsGameOver())
            {
                status = ApplySyncGameRoomResponse.GameStatus.GameOver;
                optLeftMs = (int)interval.TotalMilliseconds;

            }
            List<PlayerInfo> players = new List<PlayerInfo>();
            foreach (var seat in _seats)
            {
                if (!seat.IsSeated())
                {
                    continue;
                }
                int handcardCount = 0;
                PlayerInfo.PlayerStatus seatStatus = PlayerInfo.PlayerStatus.Idle;
                if (_statusInfo.IsGamePlaying())
                {
                    if (!seat.IsInGame())
                    {
                        seatStatus = PlayerInfo.PlayerStatus.Watching;
                    }
                    else if (!seat.IsCanContinue())
                    {
                        seatStatus = PlayerInfo.PlayerStatus.Drop;
                    }
                    else
                    {
                        if (seat.IsAllin())
                        {
                            seatStatus = PlayerInfo.PlayerStatus.Allin;
                        }
                        else
                        {
                            seatStatus = PlayerInfo.PlayerStatus.Playing;
                        }
                    }
                    handcardCount = seat.HandCards.Count;
                    seatStatus = PlayerInfo.PlayerStatus.Playing;
                }
                var com = seat.GetAppHandCards();


                 var player = new PlayerInfo(seat.PlayerInfo.Id, seat.SeatNum, seat.PlayerInfo.UserName, seat.PlayerInfo.Carry,
                seat.PlayerInfo.HeadUrl, handcardCount, seatStatus, com.Cards, (int)com.ComType, com.Point, seat.BetedCoins);
                players.Add(player);
            }

            ApplySyncGameRoomResponse response = new ApplySyncGameRoomResponse(status, players, _coinsPool._pool, optLeftMs,
                GameTimerConfig.BetChips, GameTimerConfig.GameAccount);
            return new ToAppResponse(response, ResponseStatus.Success, null);
        }
        #endregion
    }
}
