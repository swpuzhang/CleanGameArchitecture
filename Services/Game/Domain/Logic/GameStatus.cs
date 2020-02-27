using Commons.Extenssions;
using Commons.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Game.Domain.Logic
{
    public enum GameStatus
    {
        Idle = 0,
        ready,
        playing,
        FirstRound,
        SecondRound,
        GameOver,
    }

    public class GameStatusLogic
    {
        public GameStatusLogic()
        {
            _timer.Elapsed += TimeOut;
            _timer.AutoReset = false;
        }

        public GameStatus _status { get; private set; } = GameStatus.Idle;
        public DateTime _beginTime { get; private set; }
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private Action _a;
        public void  WaitForNexStatus(Action a, GameStatus nexStatus, double ms)
        {
            _beginTime = DateTime.Now;
            _timer.Stop();
            if (ms == 0)
            {
                a();
                return;
            }
            _timer.Interval = ms;
            _timer.Start();
            //_timer = new Timer(TimeOut, null, ts, TimeSpan.MinValue .FromSeconds(-1));
            _status = nexStatus;
            _a = a;
           
        }

        public bool IsGameCanStart()
        {
            return _status == GameStatus.Idle || _status == GameStatus.ready;
        }

        public bool IsFirstRound()
        {
            return _status == GameStatus.FirstRound;
        }

        public bool IsSecondRound()
        {
            return _status == GameStatus.SecondRound;
        }

        public bool IsActive()
        {
            return IsFirstRound() || IsSecondRound();
        }

        public bool IsGameOver()
        {
            return _status == GameStatus.GameOver;
        }

        public bool IsGamePlaying()
        {
            return _status == GameStatus.playing || _status == GameStatus.SecondRound
                || _status == GameStatus.FirstRound;
        }

        public bool IsPlaying()
        {
            return _status == GameStatus.playing;
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        private void TimeOut(object o, ElapsedEventArgs e)
        {
            OneThreadSynchronizationContext.Instance.Post(x => _a() , null);
        }

    }
}
