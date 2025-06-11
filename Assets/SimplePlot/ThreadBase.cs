using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZhTool.SimplePlot
{
    #region thread base
    public abstract class ThreadAssetBase<T> : ScriptableObject where T : ThreadBase
    {
        public List<ActionAsset> preActions;
        public List<ActionAsset> actions;
        public bool isLinear;

        public T Transfer()
        {
            T thread = Get();

            thread.actions = new(actions.Count);
            for (int i = 0; i < actions.Count; i++)
            {
                thread.actions.Add(actions[i].Transfer());
            }

            thread.preActions = new(preActions.Count);
            for (int i = 0; i < preActions.Count; i++)
            {
                thread.preActions.Add(preActions[i].Transfer());
            }
            thread.isLinear = isLinear;

            return thread;
        }
        protected abstract T Get();

        protected void TransferList<TSource, TTarget>(List<TSource> source, ref List<TTarget> target) where TSource : ITransferable<TTarget>
        {
            target = new(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                target.Add(source[i].Transfer());
            }
        }

    }

    public class ThreadBase : ICondition, IInitializable
    {
        public List<Action> preActions;
        public List<Action> actions;

        public bool isLinear;

        // state
        protected bool isSatisfied;
        protected bool isPreActionDone;
        protected bool isActionDone;

        protected Task actionExecution;

        protected UnityAction onEventSatisfied;

        public void Initialize()
        {
            isSatisfied = false;
            isActionDone = false;
            isPreActionDone = false;
            actionExecution?.Stop();
            actionExecution = null;

            for (int i = 0; i < actions.Count; i++)
                actions[i].Initialize();
            for (int i = 0; i < preActions.Count; i++)
            {
                preActions[i].Initialize();
                preActions[i].InitializePreAction(ref onEventSatisfied);
            }

            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public bool IsSatisfied(PlotBase plot)
        {
            if (!isPreActionDone)
            {
                if (actionExecution == null)
                {
                    Action(preActions, plot);
                    actionExecution.Finished += (x) => isPreActionDone = true;
                }
                return false;
            }

            if (!isSatisfied)
            {
                if (!CheckCondition(plot)) return false;

                onEventSatisfied?.Invoke();
                Action(actions, plot);
                actionExecution.Finished += (x) => isActionDone = true;
                isSatisfied = true;
                return false;
            }

            return isActionDone;
        }

        protected virtual bool CheckCondition(PlotBase plot)
        {
            return true;
        }

        protected bool CheckCondition<T>(List<T> conditions, PlotBase plot) where T : ICondition
        {
            bool allSatisfied = true;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i].IsSatisfied(plot))
                {
                    if (isLinear) return false;
                    allSatisfied = false;
                }
            }
            return allSatisfied;
        }

        protected void InitializeSub<T>(List<T> sub) where T : IInitializable
        {
            for (int i = 0; i < sub.Count; i++)
            {
                sub[i].Initialize();
            }
        }

        protected void Action(List<Action> obj, PlotBase plot)
        {
            actionExecution = new Task(ActionExecution(obj, plot));
        }

        IEnumerator ActionExecution(List<Action> obj, PlotBase plot)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].Act(plot);
                if (obj[i].waitForAction)
                {
                    while (!obj[i].IsActionDone()) yield return null;
                }
            }
        }
    }
    #endregion

    public interface ICondition
    {
        public bool IsSatisfied(PlotBase plot);
    }
    public interface IInitializable
    {
        public void Initialize();
    }
    public interface ITransferable<T>
    {
        public T Transfer();
    }
}
