using UnityEngine;

namespace ZhTool.SimplePlot.Extension
{
    [CreateAssetMenu(fileName = "AlwaysFalse", menuName = "Simple Plot/Condition/AlwaysFalse")]
    public class Condition_AlwaysFalseAsset : ConditionAsset
    {
        protected override Condition GetCondition()
        {
            return new Condition_AlwaysFalse();
        }
    }

    public class Condition_AlwaysFalse : Condition
    {
        public override bool CheckCondition(PlotBase plot)
        {
            return false;
        }
    }
}