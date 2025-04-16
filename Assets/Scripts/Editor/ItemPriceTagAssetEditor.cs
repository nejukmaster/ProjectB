#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectB
{
    [CustomEditor(typeof(ItemPriceTagAsset))]
    public class ItemPriceTagAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ItemPriceTagAsset asset = (ItemPriceTagAsset)target;
            while(asset.priceTags.Count < Enum.GetValues(typeof(ItemType)).Length)
            {
                asset.priceTags.Add(0);
            }
            for(int i = 0; i < asset.priceTags.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.EnumPopup((ItemType)i);
                GUI.enabled = true;
                asset.priceTags[i] = EditorGUILayout.IntField(asset.priceTags[i]);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);
            }
        }
    }
}
#endif
