using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace MornLib
{
    public sealed class MornGlobalLogger
    {
        private readonly IMornGlobal _target;

        public MornGlobalLogger(IMornGlobal target)
        {
            _target = target;
        }

        private string Prefix => $"[<color=#{ColorUtility.ToHtmlStringRGB(_target.ModuleColor)}>{_target.ModuleName}</color>] ";

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void Log(object message)
        {
            Debug.Log($"{Prefix}{message}");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void Log(object message, Object context)
        {
            Debug.Log($"{Prefix}{message}", context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void LogWarning(object message)
        {
            Debug.LogWarning($"{Prefix}{message}");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void LogWarning(object message, Object context)
        {
            Debug.LogWarning($"{Prefix}{message}", context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void LogError(object message)
        {
            Debug.LogError($"{Prefix}{message}");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public void LogError(object message, Object context)
        {
            Debug.LogError($"{Prefix}{message}", context);
        }
    }
}