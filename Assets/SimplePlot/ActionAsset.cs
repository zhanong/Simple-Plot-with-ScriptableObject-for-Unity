using UnityEngine;
using UnityEngine.Events;

namespace ZhTool.SimplePlot
{
    public abstract class ActionAsset : ScriptableObject
    {
        public bool waitForAction;
        public Action Transfer()
        {
            Action action = GetAction();
            action.waitForAction = waitForAction;
            action.asset = this;
            return action;
        }

        protected abstract Action GetAction();
    }


    public class Action : IInitializable
    {
        public bool waitForAction;
        public ActionAsset asset;

        public virtual void Act(PlotBase plot) { }

        public void Initialize()
        {
            OnInitialize();
        }

        /// <summary>
        /// this is called only when the action is a pre-action in threadEvent
        /// </summary>
        public void InitializePreAction(ref UnityAction onEventSatisfied)
        {
            OnInitializePreAction(ref onEventSatisfied);
        }

        public virtual void OnInitialize() { }

        /// <summary>
        /// this is called only when the action is a pre-action in threadEvent and after the OnInitialize()
        /// </summary>
        public virtual void OnInitializePreAction(ref UnityAction onEventSatisfied) { }

        public virtual bool IsActionDone()
        {
            return true;
        }
    }
}
