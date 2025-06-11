using UnityEngine;
using ZhTool.SimplePlot;

namespace ZhTool.SimplePlot.Example
{
    public class Plot : PlotBase
    {
        [SerializeField] GameObject winUI, loseUI;

        void Start()
        {
            sceneStarted = true;
        }

        protected override void OnWin()
        {
            winUI.SetActive(true);
        }

        protected override void OnLose()
        {
            loseUI.SetActive(true);
        }
    }
}
