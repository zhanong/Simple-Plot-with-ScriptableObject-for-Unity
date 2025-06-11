using UnityEngine;

namespace ZhTool.Level.Extension
{
    [CreateAssetMenu(fileName = "BondedCondition", menuName = "Simple Plot/Condition/BondedCondition")]
    public class BondedConditionAsset : ConditionAsset
    {
        public ConditionAsset[] conditions;
        protected override Condition GetCondition()
        {
            return new BondedCondition();
        }
    }

    public class BondedCondition : Condition
    {
        public override bool CheckCondition(PlotBase plot)
        {
            var asset = this.asset as BondedConditionAsset;
            for (int i = 0; i < asset.conditions.Length; i++)
            {
                var condition = asset.conditions[i].Transfer();
                condition.Initialize();

                if (!condition.CheckCondition(plot))
                {
                    return false;
                }
            }
            return true;
        }
    }
}