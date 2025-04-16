using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    [Serializable]
    public class ShopEntry
    {
        [SerializeField] ItemStack item;
        [SerializeField] int price;
    }

    [CreateAssetMenu(fileName = "New Shop Asset", menuName = "ProjectB/Shop Asset")]
    public class ShopAsset : ScriptableObject
    {
        [SerializeField] public List<ShopEntry> entries;
    }
}
