using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ClawbearGames
{
    [CustomEditor(typeof(AdmobController))]
    public class AdmobControllerCustomEditor : Editor
    {
        private bool isAdmobNamespaceExists = false;
        private bool isAddedAdmobSymbols = false;
        private void OnEnable()
        {
            isAdmobNamespaceExists = ScriptingSymbolsHandler.NamespaceExists(NamespaceData.GoogleMobileAdsNameSpace);

            string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> currentSymbols = new List<string>(symbolStr.Split(';'));
            isAddedAdmobSymbols = currentSymbols.Contains(ScriptingSymbolsData.ADMOB);
        }

        public override void OnInspectorGUI()
        {
            if (!isAdmobNamespaceExists)
            {
                EditorGUILayout.HelpBox("Google Mobile Ads plugin is not imported. Please click the button bellow to download the plugin.", MessageType.Warning);
                if (GUILayout.Button("Download Plugin", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases");
                }
            }
            else
            {
                if (!isAddedAdmobSymbols)
                {
                    isAddedAdmobSymbols = true;
                    string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                    List<string> currentSymbols = new List<string>(symbolStr.Split(';'));
                    if (!currentSymbols.Contains(ScriptingSymbolsData.ADMOB))
                    {
                        List<string> sbs = new List<string>();
                        sbs.Add(ScriptingSymbolsData.ADMOB);
                        ScriptingSymbolsHandler.AddDefined_ScriptingSymbol(sbs.ToArray(), EditorUserBuildSettings.selectedBuildTargetGroup);
                    }
                }
            }
            base.OnInspectorGUI();
        }
    }
}

