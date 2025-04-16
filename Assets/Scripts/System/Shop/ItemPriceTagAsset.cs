using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    [CreateAssetMenu(fileName = "NewPriceTagAsset", menuName = "ProjectB/ItemPriceTagAsset")]
    public class ItemPriceTagAsset : ScriptableObject
    {
        [SerializeField] public List<int> priceTags;
    }
}
