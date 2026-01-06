#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace MornGlobal
{
    public sealed class MornGlobalDefineSymbolRegisterer
    {
        private readonly string _symbolName;
        private readonly MornGlobalLogger _logger;

        public MornGlobalDefineSymbolRegisterer(string symbolName, MornGlobalLogger logger)
        {
            _symbolName = symbolName;
            _logger = logger;
            EditorApplication.delayCall += ApplyToAllTargets;
        }

        private void ApplyToAllTargets()
        {
            var anyChanged = false;
            foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (group == BuildTargetGroup.Unknown || IsObsolete(group))
                {
                    continue;
                }

                anyChanged |= TryAddDefine(group, _symbolName);
            }

            if (anyChanged)
            {
                _logger.LogInternal($"Defineシンボル[{_symbolName}]をすべてのBuildTargetGroupに追加しました。");
                EditorUtility.RequestScriptReload();
            }
        }

        private static bool IsObsolete(BuildTargetGroup g)
        {
            var mem = typeof(BuildTargetGroup).GetMember(g.ToString()).FirstOrDefault();
            return mem != null && Attribute.IsDefined(mem, typeof(ObsoleteAttribute));
        }

        private bool TryAddDefine(BuildTargetGroup group, string symbol)
        {
            try
            {
                var named = NamedBuildTarget.FromBuildTargetGroup(group);
                var defines = PlayerSettings.GetScriptingDefineSymbols(named).Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                if (defines.Contains(symbol)) return false;
                defines.Add(symbol);
                PlayerSettings.SetScriptingDefineSymbols(named, string.Join(";", defines.Distinct()));
                return true;
            }
            catch (Exception e)
            {
                _logger.LogErrorInternal($"Defineシンボル[{_symbolName}]の追加に失敗: " + e.Message);
                return false;
            }
        }
    }
}
#endif