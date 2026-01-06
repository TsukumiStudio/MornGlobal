using UnityEngine;

namespace MornGlobal
{
    public abstract class MornGlobalPureBase<T> : IMornGlobal where T : new()
    {
        private static T _instance;
        public static T I => _instance ??= new T();
        private MornGlobalLogger _logger;
        private MornGlobalLogger Logger => _logger ??= new MornGlobalLogger(this);
        string IMornGlobal.ModuleName => ModuleName;
        protected abstract string ModuleName { get; }

        protected void LogInternal(string message)
        {
            Logger.LogInternal(message);
        }

        protected void LogErrorInternal(string message)
        {
            Logger.LogErrorInternal(message);
        }

        protected void LogWarningInternal(string message)
        {
            Logger.LogWarningInternal(message);
        }

        public void AddDefineSymbol(string symbolName)
        {
#if UNITY_EDITOR
            _ = new MornGlobalDefineSymbolRegisterer(symbolName, _logger);
#endif
        }

        protected void SetDirtyInternal(Object target)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(target);
#endif
        }
    }
}