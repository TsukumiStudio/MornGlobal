using UnityEditor;
using UnityEngine;

namespace MornLib
{
    public sealed class MornGlobalLogger
    {
        private readonly IMornGlobal _target;

        public MornGlobalLogger(IMornGlobal target)
        {
            _target = target;
        }

#if UNITY_EDITOR
        private bool ShowLog => EditorPrefs.GetBool($"{_target.ModuleName}_ShowLog", true);
        private bool ShowLogWarning => EditorPrefs.GetBool($"{_target.ModuleName}_ShowLogWarning", true);
        private bool ShowLogError => EditorPrefs.GetBool($"{_target.ModuleName}_ShowLogError", true);
#else
        private bool ShowLog => Debug.isDebugBuild;
        private bool ShowLogWarning => Debug.isDebugBuild;
        private bool ShowLogError => Debug.isDebugBuild;
#endif
        private string Prefix => $"[<color=green>{_target.ModuleName}</color>] ";

        public void LogInternal(string message)
        {
            if (ShowLog)
            {
                Debug.Log($"{Prefix} {message}");
            }
        }

        public void LogErrorInternal(string message)
        {
            if (ShowLogWarning)
            {
                Debug.LogError($"{Prefix} {message}");
            }
        }

        public void LogWarningInternal(string message)
        {
            if (ShowLogError)
            {
                Debug.LogWarning($"{Prefix} {message}");
            }
        }
    }
}