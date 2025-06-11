using System.Collections.Generic;
using UnityEngine;

namespace ZhTool.SimplePlot
{
    [CreateAssetMenu(fileName = "SubThread", menuName = "Simple Plot/SubThread")]
    public class SubThreadAsset : ThreadAssetBase<SubThread>, ITransferable<SubThread>
    {
        public List<EventAsset> events = new();

        protected override SubThread Get()
        {
            SubThread subThread = new();
            TransferList(events, ref subThread.events);
            return subThread;
        }
    }

    public class SubThread : ThreadBase
    {
        public List<Event> events = new();

        protected override void OnInitialize()
        {
            InitializeSub(events);
        }

        protected override bool CheckCondition(PlotBase plot)
        {
            return CheckCondition(events, plot);
        }
    }
}
