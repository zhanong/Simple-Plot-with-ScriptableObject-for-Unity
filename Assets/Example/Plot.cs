using UnityEngine;
using ZhTool.Level;

namespace ZhTool.Level.Example
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
