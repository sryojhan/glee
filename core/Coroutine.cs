using System;
using System.Collections;
using System.Threading.Tasks;
using Glee.Engine;

namespace Glee
{
    class Coroutine(IEnumerator obj)
    {
        public IEnumerator enumerator = obj;
        public Wait wait;
        public GleeObject Owner;
    }

    public abstract class Wait
    {
        public bool IsComplete { get; private set; }

        /// <returns>Returns is complete</returns>
        public bool NewFrame(Time time)
        {
            if (IsComplete) return true;

            IsComplete = CheckCondition(time);
            return IsComplete;
        }

        /// <returns>Returns true if the wait is complete</returns>
        protected abstract bool CheckCondition(Time time);
    }


    class WaitSeconds(float seconds, bool useRealSeconds = false) : Wait
    {
        readonly bool realSeconds = useRealSeconds;
        float remainingTime = seconds;

        protected override bool CheckCondition(Time time)
        {
            remainingTime -= realSeconds ? time.realDeltaTime : time.deltaTime;

            return remainingTime <= 0f;
        }
    }


    class WaitFrames(int count) : Wait
    {
        int remainingFrames = count;

        protected override bool CheckCondition(Time time)
        {
            remainingFrames--;
            return remainingFrames <= 0;
        }
    }


    class WaitCondition(Condition condition) : Wait
    {
        readonly Condition condition = condition;

        protected override bool CheckCondition(Time time)
        {
            return condition.Invoke();
        }
    }



    namespace Engine
    {
        public partial class GleeObject
        {
            protected void Launch(IEnumerator coroutine)
            {
                Get<CoroutineManager>().Launch(this, coroutine);
            }

        }
    }

}