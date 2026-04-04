using UnityEngine;

namespace MornLib
{
    public abstract class MornGlobalPureBase<T> : IMornGlobal where T : new()
    {
        private static T _instance;
        public static T I => _instance ??= new T();
        private static MornGlobalLogger _logger;
        public static MornGlobalLogger Logger => _logger ??= new MornGlobalLogger((IMornGlobal)I);
        string IMornGlobal.ModuleName => ModuleName;
        Color IMornGlobal.ModuleColor => ModuleColor;
        protected abstract string ModuleName { get; }
        protected virtual Color ModuleColor => Color.green;
    }
}