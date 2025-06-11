
namespace ZhTool.Level
{
    public struct Timer
    {
        readonly float fixedTime;
        float _timeLapse;
        public readonly float TimeLapse => _timeLapse;
        public readonly bool TimeUp => setUpped && _timeLapse >= fixedTime;
        bool setUpped;

        public Timer(float fixedTime)
        {
            this.fixedTime = fixedTime;
            _timeLapse = 0;
            setUpped = true;
        }

        public void Tick(float deltaTime)
        {
            _timeLapse += deltaTime;
        }

        public void Reset()
        {
            _timeLapse = 0;
        }

        public readonly bool IsTimeUp(out float timeLapse)
        {
            timeLapse = _timeLapse;
            return setUpped && _timeLapse >= fixedTime;
        }

        public bool TickOrReset(float deltaTime, out float timeLapse)
        {
            _timeLapse += deltaTime;
            timeLapse = _timeLapse;
            if (_timeLapse >= fixedTime)
            {
                Reset();
                return true;
            }
            return false;
        }
        
    }
}
