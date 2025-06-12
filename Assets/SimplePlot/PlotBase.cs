using System.Collections.Generic;
using UnityEngine;

namespace ZhTool.SimplePlot
{
    public class PlotBase : MonoBehaviour
    {
        [SerializeField] ThreadAsset threadAsset;
        [SerializeField] float updatePeriod = 0.1f;

        Thread thread;
        protected bool sceneStarted = false;
        bool EndGame = false;
        Timer timer;

        Dictionary<string, GameObject> plotObjects;

        void Awake()
        {
            thread = threadAsset.Transfer();
            thread.Initialize();
            timer = new Timer(updatePeriod);
            RegisterPlotObject();
        }

        void RegisterPlotObject()
        {
            var plotObjectsInScene = FindObjectsByType<PlotObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            plotObjects = new Dictionary<string, GameObject>(plotObjectsInScene.Length);
            foreach (var plotObject in plotObjectsInScene)
            {
                if (plotObjects.ContainsKey(plotObject.name))
                {
                    Debug.LogWarning($"Duplicate PlotObject name found: {plotObject.name}. Only the first one will be registered.");
                    continue;
                }
                plotObjects.Add(plotObject.name, plotObject.gameObject);
            }
        }

        void Update()
        {
            if (!sceneStarted) return;
            if (EndGame) return;
            if (!timer.TickOrReset(Time.deltaTime, out _)) return;

            // REGULAR UPDATE
            OnUpdate();

            if (thread.CheckLoseCondition(this))
            {
                EndGame = true;
                OnLose();
                return;
            }

            if (thread.IsSatisfied(this))
            {
                EndGame = true;
                OnWin();
            }
        }

        public void StartThread()
        {
            if (sceneStarted) return;

            OnSceneStart();
            thread.IsSatisfied(this);
            sceneStarted = true;
        }

        public GameObject GetPlotObject(string name)
        {
            if (plotObjects.TryGetValue(name, out var go))
            {
                return go;
            }
            Debug.LogWarning($"'{name}' is not registered as PlotObject. Add it to the scene with a PlotObject component.");
            return null;
        }

        protected virtual void OnUpdate() { }
        protected virtual void OnSceneStart() { }
        protected virtual void OnWin() { }
        protected virtual void OnLose() { }
    }



}
