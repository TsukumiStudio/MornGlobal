using UnityEngine;

namespace MornLib
{
    public static class MornGlobalUtil
    {
        public static void SetDirty(Object target)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(target);
#endif
        }
    }
}
