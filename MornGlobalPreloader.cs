#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    [InitializeOnLoad]
    public static class MornGlobalPreloader
    {
        static MornGlobalPreloader()
        {
            var assets = PlayerSettings.GetPreloadedAssets();
            foreach (var asset in assets)
            {
                if (asset is ScriptableObject scriptableObject)
                {
                    scriptableObject.hideFlags = HideFlags.None;
                    EditorUtility.SetDirty(scriptableObject);
                }
            }
        }
    }
}
#endif