using UnityEngine;

namespace MornLib
{
    public interface IMornGlobal
    {
        string ModuleName { get; }
        Color ModuleColor { get; }
    }
}