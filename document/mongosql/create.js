var url = "mongodb://localhost:27017/CleanGame";
var db = connect(url);
db.UserIdGenInfo.save({"_id":NumberLong(10000000000),"UserId":NumberLong(10000000000)})
db.createCollection("AccountInfo");
db.AccountInfo.ensureIndex({PlatformAccount:1}, {unique: true});
db.createCollection("LevelConfig");
db.LevelConfig.ensureIndex({Level:1}, {unique: true});
db.LevelConfig.insert( {Level:1, NeedExp:10});
db.LevelConfig.insert( {Level:2, NeedExp:10});
db.CoinsRangeConfig.insert({CoinsBegin:NumberLong(5000),CoinsEnd:NumberLong(100000),Blind:NumberLong(500)})
db.CoinsRangeConfig.insert({CoinsBegin:NumberLong(100000),CoinsEnd:NumberLong(-1),Blind:NumberLong(2000)})
db.RoomListConfig.ensureIndex({Blind:1}, {unique: true});
db.RoomListConfig.insert({RoomType:1, Blind:NumberLong(700), MinCoins:NumberLong(3500), MaxCoins:NumberLong(10000), TipsPersent:NumberLong(10),MinCarry:NumberLong(3500), MaxCarry:NumberLong(35000)})
db.RoomListConfig.insert({RoomType:1, Blind:NumberLong(1500), MinCoins:NumberLong(7500), MaxCoins:NumberLong(-1), TipsPersent:NumberLong(20),MinCarry:NumberLong(7500), MaxCarry:NumberLong(75000)})
db.RegisterRewardConfig.insert({DayRewards:[NumberLong(1200),NumberLong(2000),NumberLong(3500),NumberLong(6000),NumberLong(10000)]})
db.LoginRewardConfig.insert({DayRewards:[NumberLong(1000),NumberLong(1500),NumberLong(2500),NumberLong(3000),NumberLong(4000),NumberLong(5000),NumberLong(8000)]})
db.BankruptcyConfig.insert({BankruptcyRewards:[NumberLong(4000),NumberLong(6000)], BankruptcyLimit:NumberLong(3500)})
db.InviteRewardConfig.insert({InviteRewards:NumberLong(4000)})
db.GameActivityConfig.insert({ActivityId:"game2019082101",Title:"打牌活动",ActivityType:0,RoomConfigs:[{SubId:"1",NeedCount:NumberLong(2), RewardCoins:NumberLong(200), Title:"2局", RoomType:1},{SubId:"2",NeedCount:NumberLong(5), RewardCoins:NumberLong(500), Title:"5局", RoomType:1}]})
