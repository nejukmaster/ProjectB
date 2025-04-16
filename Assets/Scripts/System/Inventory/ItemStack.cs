using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //�������� Ÿ���� ��Ÿ���� EnumŬ�����Դϴ�.
    public enum ItemType
    {
        NONE,
        STEAK,
        WATER_BOTTLE,
        HOE,
        WHEAT_SEED,
        WHEAT,
        CORN_SEED,
        CORN
    }

    //ItemType�� ExtensionsŬ���� �Դϴ�.
    public static class ItemTypeExtensions
    {
        public static string GetName(this ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.STEAK:
                    return "steak";
                case ItemType.WATER_BOTTLE:
                    return "water_bottle";
                case ItemType.HOE:
                    return "hoe";
                case ItemType.WHEAT_SEED:
                    return "seed_base";
                case ItemType.WHEAT:
                    return "wheat";
                case ItemType.CORN_SEED:
                    return "seed_base";
                case ItemType.CORN:
                    return "corn";
                default:
                    return "empty";
            }
        }

        public static Color GetColor(this ItemType itemType)
        {
            switch(itemType)
            {
                case ItemType.WHEAT_SEED:
                    return new Color(0.57f, 0.42f, 0.24f, 1.0f);
                case ItemType.CORN_SEED:
                    return new Color(1.0f, 0.92f, 0.33f);
                default:
                    return Color.white;
            }
        }

        public static GameObject GetEquipment(this ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.HOE:
                    return Resources.Load<GameObject>("Prefabs/Item/equipment_hoe");
                default:
                    return null;
            }
        }

        public static Sprite GetSprite(this ItemType itemType)
        {
            return Resources.Load<Sprite>("Images/Sprite/Item/" + itemType.GetName());
        }
    }

    //���ӳ� �������� �����Ǵ� ��ü�� Ŭ�����Դϴ�.
    [System.Serializable]
    public class ItemStack
    {
        [SerializeField] ItemType type;
        [SerializeField] int Amount;

        public ItemStack(ItemType type, int amount)
        {
            this.type = type;
            this.Amount = amount;
        }

        public ItemStack()
        {
            this.type = ItemType.NONE;
            this.Amount = 0;
        }

        public ItemType GetType()
        {
            return type;
        }

        public void SetType(ItemType type)
        {
            this.type = type;
        }

        public int GetAmount()
        {
            return Amount;
        }

        public void SetAmount(int amount)
        {
            this.Amount = amount;
        }
    }
}
