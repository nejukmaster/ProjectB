#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace ProjectB
{
    [CustomEditor(typeof(GrindManagerAsset))]
    public class GrindManagerAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GrindManagerAsset asset = (GrindManagerAsset)target;
            while(asset.GrindAssetList.Count() < Enum.GetValues(typeof(GrindType)).Length)
            {
                asset.GrindAssetList.Add(null);
            }
            for(int i = 0; i < asset.GrindAssetList.Count(); i++)
            {
                GUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.EnumPopup((GrindType)i);
                GUI.enabled = true;
                asset.GrindAssetList[i] = (GrindAsset)EditorGUILayout.ObjectField(
                    asset.GrindAssetList[i],
                    typeof(GrindAsset),
                    false
                );
                GUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);
            }
        }
    }
}
#endif
