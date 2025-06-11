using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZhTool.Level.Example
{
    [CreateAssetMenu(fileName = "ResetSlider", menuName = "Simple Plot/Action/ResetSlider")]
    public class ActionAsset_ResetSlider : ActionAsset
    {
        public string targetName;
        public float speed = 1f;

        protected override Action GetAction()
        {
            return new Action_ResetSlider();
        }
    }

    public class Action_ResetSlider : Action
    {
        bool actionDone = false;
        Task task;

        public override void Act(PlotBase plot)
        {
            var a = asset as ActionAsset_ResetSlider;
            if (a == null || a.targetName == null || a.targetName.Length == 0)
            {
                Debug.LogWarning("ActionAsset_ResetSlider has no targets set.");
                return;
            }


            var go = plot.GetPlotObject(a.targetName);
            if (go != null && go.TryGetComponent(out Slider target))
            {
                task = new Task(ResetSlider(target, a.speed));
                task.Start();
            }

        }

        IEnumerator ResetSlider(Slider target, float speed)
        {
            while (target.value > 0f)
            {
                target.value -= speed * Time.deltaTime;
                yield return null;
            }
            target.value = 0f;
            actionDone = true;
        }

        public override bool IsActionDone()
        {
            return actionDone;
        }
    }
}
