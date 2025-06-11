using UnityEngine;

namespace ZhTool.SimplePlot.Example
{
    [CreateAssetMenu(fileName = "SetActive", menuName = "Simple Plot/Action/SetActive")]
    public class ActionAsset_SetActive : ActionAsset
    {
        [Tooltip("Enter the name of the GameObject and whether to activate it or inactivate it.")]
        public Target[] targets;

        protected override Action GetAction()
        {
            return new Action_SetActive();
        }
    }

    public class Action_SetActive : Action
    {
        public override void Act(PlotBase plot)
        {
            var a = asset as ActionAsset_SetActive;
            if (a == null || a.targets == null || a.targets.Length == 0)
            {
                Debug.LogWarning("ActionAsset_SetActive has no targets set.");
                return;
            }


            foreach (var target in a.targets)
            {
                var go = plot.GetPlotObject(target.name);
                if (go != null)
                    go.SetActive(target.activeOrInactive);
            }
        }
    }

    [System.Serializable]
    public struct Target
    {
        public string name;
        public bool activeOrInactive;
    }
}
