using UnityEngine;

namespace ZhTool.Level
{
    public abstract class ConditionAsset : ScriptableObject
    {

        public Condition Transfer()
        {
            Condition condition = GetCondition();
            condition.asset = this;
            return condition;
        }

        protected abstract Condition GetCondition();

    }

    public class Condition : ICondition, IInitializable
    {
        bool isSatisfied;
        public ConditionAsset asset;

        public virtual bool IsSatisfied(PlotBase plot)
        {
            if (isSatisfied) return true;
            if (CheckCondition(plot))
            {
                isSatisfied = true;
                return true;
            }
            return false;
        }

        public virtual bool CheckCondition(PlotBase plot)
        {
            return true;
        }

        public void Initialize()
        {
            Debug.Log(" ThreadCondition Initialize");
            isSatisfied = false;
            OnInitialize();
        }
        public virtual void OnInitialize() { }
    }
}
