using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    [Serializable]
    public class EnumGameObjectPair
    {
        public ItemType key;
        public GameObject value;

        public EnumGameObjectPair(ItemType key, GameObject value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public class EquipmentSocket : MonoBehaviour
    {
        public Dictionary<ItemType, GameObject> EquipmentMap = new Dictionary<ItemType, GameObject>();

        [SerializeField] List<EnumGameObjectPair> equipmentMap_inspector;

        private void Start()
        {
            EquipmentMap = new Dictionary<ItemType, GameObject>();
            foreach (var entry in equipmentMap_inspector)
            {
                if (!EquipmentMap.ContainsKey(entry.key))
                {
                    EquipmentMap[entry.key] = entry.value;
                }
            }
        }

        public bool EquipItem(ItemType itemType)
        {
            bool result = false;
            foreach(var pair in EquipmentMap)
            {
                if(pair.Key == itemType)
                {
                    pair.Value.SetActive(true);
                    result = true;
                }
                else
                {
                    pair.Value.SetActive(false);
                }
            }
            return result;
        }
    }
}
