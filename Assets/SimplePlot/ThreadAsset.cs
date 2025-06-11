using System.Collections.Generic;
using UnityEngine;

namespace ZhTool.Level
{
    [CreateAssetMenu(fileName = "Thread", menuName = "Simple Plot/Thread")]
    public class ThreadAsset : ThreadAssetBase<Thread>, ITransferable<Thread>
    {
        public List<SubThreadAsset> subThreads = new ();
        public List<ConditionAsset> loseConditions = new();

        protected override Thread Get()
        {
            Thread thread = new();
            TransferList(subThreads, ref thread.subThreads);

            thread.loseConditions = new(loseConditions.Count);
            for (int i = 0; i < loseConditions.Count; i++)
            {
                thread.loseConditions.Add(loseConditions[i].Transfer());
            }
            return thread;
        }
    }

    public class Thread : ThreadBase
    {
        public List<SubThread> subThreads;
        public List<Condition> loseConditions ;

        protected override void OnInitialize()
        {
            InitializeSub(subThreads);
            InitializeSub(loseConditions);
        }

        protected override bool CheckCondition(PlotBase plot)
        {
            return CheckCondition(subThreads, plot);
        }

        public bool CheckLoseCondition(PlotBase plot)
        {
            for (int i = 0; i < loseConditions.Count; i++)
            {
                if (loseConditions[i].IsSatisfied(plot))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
