using ProjectB;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���� ������ ���� Ŭ����
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewReceipe", menuName = "ProjectB/Receipe Asset")]
    public class Receipe : ScriptableObject
    {
        [SerializeField] public List<ItemStack> Ingredients;
        [SerializeField] public ItemStack Result;
        [SerializeField] public float CookingSec;
        [SerializeField] public CookwareType type;

        [NonSerialized] public bool bIsEnabled = false;
    }
}
