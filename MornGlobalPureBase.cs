namespace MornLib
{
    public abstract class MornGlobalPureBase<T> : IMornGlobal where T : new()
    {
        private static T _instance;
        public static T I => _instance ??= new T();
        private static MornGlobalLogger _logger;
        public static MornGlobalLogger Logger => _logger ??= new MornGlobalLogger((IMornGlobal)I);
        string IMornGlobal.ModuleName => ModuleName;
        protected abstract string ModuleName { get; }
    }
}