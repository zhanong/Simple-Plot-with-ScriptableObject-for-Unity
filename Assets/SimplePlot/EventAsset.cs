using System.Collections.Generic;
using UnityEngine;

namespace ZhTool.Level
{

    [CreateAssetMenu(fileName = "Event", menuName = "Simple Plot/Event")]
    public class EventAsset : ThreadAssetBase<Event>, ITransferable<Event>
    {
        public List<ConditionAsset> conditions = new();

        protected override Event Get()
        {
            Event threadEvent = new();
            threadEvent.conditions = new(conditions.Count);
            for (int i = 0; i < conditions.Count; i++)
            {
                threadEvent.conditions.Add(conditions[i].Transfer());
            }
            return threadEvent;
        }
    }

    public class Event : ThreadBase
    {
        public List<Condition> conditions;

        protected override void OnInitialize()
        {
            InitializeSub(conditions);
        }

        protected override bool CheckCondition(PlotBase plot)
        {
            return CheckCondition(conditions, plot);
        }
    }
}
