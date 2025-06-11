using UnityEngine;

namespace ZhTool.SimplePlot.Example
{
    [CreateAssetMenu(fileName = "Click", menuName = "Simple Plot/Condition/Click")]
    public class ConditionAsset_Click : ConditionAsset
    {
        protected override Condition GetCondition()
        {
            return new Condition_Click();
        }
    }

    public class Condition_Click : Condition
    {
        public override bool CheckCondition(PlotBase plot)
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}
