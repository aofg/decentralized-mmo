using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DebugView : AbstractView
    {
        public Text LinePrefab;

        private void Start()
        {
            Application.logMessageReceived += OnLog;
        }

        private void OnLog(string condition, string stackTrace, LogType type)
        {
            var line = Instantiate(LinePrefab);
            line.text = condition;
            line.color = GetColor(type);
            line.rectTransform.SetParent(transform);
            line.rectTransform.localScale = Vector3.one;
            
            Destroy(line.gameObject, 1f);
        }

        private Color GetColor(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    return Color.red;
                case LogType.Log:
                    return Color.black;
                case LogType.Warning:
                    return new Color(0.9f, 0.65f, 0.2f);
            }

            return Color.black;
        }
    }
}