using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DebugWindow : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI debugText = default;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private bool log;
        [SerializeField]
        private bool warning;
        [SerializeField]
        private bool error;
        [SerializeField]
        private bool assert;
        [SerializeField]
        private bool exception;
        
        private void Awake()
        {
            debugText.text = "";
            Application.logMessageReceived += HandleLog;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string message, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log when log:
                    debugText.text += $"[<b>{DateTime.Now:HH:mm:ss}</b>] <color=white>{message}</color> \n";
                    break;
                case LogType.Warning when warning:
                    debugText.text += $"[<b>{DateTime.Now:HH:mm:ss}</b>] <color=yellow>{message}</color> \n";
                    break;
                case LogType.Error when error:
                    debugText.text += $"[<b>{DateTime.Now:HH:mm:ss}</b>] <color=red>{message}</color> \n";
                    break;
                case LogType.Assert when assert:
                    debugText.text += $"[<b>{DateTime.Now:HH:mm:ss}</b>] <color=orange>{message}</color> \n";
                    break;
                case LogType.Exception when exception:
                    debugText.text += $"[<b>{DateTime.Now:HH:mm:ss}</b>] <color=blue>{message}</color> \n";
                    break;
            }
            
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }

        public void ClearOnClick()
        {
            debugText.text = "";
        }
    }
