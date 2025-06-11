using UnityEngine;
using UnityEngine.UI;

namespace ZhTool.Level.Example
{
    [CreateAssetMenu(fileName = "SliderValue", menuName = "Simple Plot/Condition/SliderValue")]
    public class ConditionAsset_SliderValue : ConditionAsset
    {
        public string sliderName;
        public float targetValue;
        public Comparision comparision;

        protected override Condition GetCondition()
        {
            return new Condition_SliderValue();
        }
    }

    public class Condition_SliderValue : Condition
    {
        public override bool CheckCondition(PlotBase plot)
        {
            var a = asset as ConditionAsset_SliderValue;
            var go = plot.GetPlotObject(a.sliderName);
            if (go != null && go.TryGetComponent(out Slider slider))
            {
                return (a.comparision == Comparision.BiggerThan && slider.value > a.targetValue) ||
                       (a.comparision == Comparision.SmallerThan && slider.value < a.targetValue);
            }
            else
            {
                Debug.LogWarning($"Slider with name '{a.sliderName}' not found in plot objects.");
                return false;
            }
        }
    }

    public enum Comparision { BiggerThan, SmallerThan }
}
