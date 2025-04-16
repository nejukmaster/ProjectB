using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //하단부에 위치하여 플레이어가 아이템을 바꿀수 있도록 하는 UI입니다.
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] ItemContainer[] hotbarSlots;
        [SerializeField] GameObject hotbarFocus;

        int focusing_index;

        private void Start()
        {
            for(int i = 0; i < hotbarSlots.Length; i++)
            {
                hotbarSlots[i].index = i;
                hotbarSlots[i].onRightClicked = OnRightClickHotbar;
            }
        }

        public void SetHotbarFocus(int index)
        {
            hotbarFocus.GetComponent<RectTransform>().anchoredPosition = hotbarSlots[index].GetComponent<RectTransform>().anchoredPosition;
            focusing_index = index;
        }

        public void UpdateHotbar()
        {
            Inventory hotbar = InventorySystem.instance.GetHotbar();
            for(int i = 0; i < hotbarSlots.Length; i++)
            {
                if (i < hotbar.Count)
                {
                    hotbarSlots[i].SetItem(hotbar[i]);
                }
                else
                    hotbarSlots[i].SetEmpty();
            }
        }

        public ItemStack GetFocusingItem()
        {
            if (focusing_index < InventorySystem.instance.GetHotbar().Count)
                return InventorySystem.instance.GetHotbar()[focusing_index];
            else
                return null;
        }

        public void OnRightClickHotbar(int index)
        {
            Inventory hotbar_inv = InventorySystem.instance.GetHotbar();

            InventorySystem.instance.GetInventory().AddItem(hotbar_inv[index].GetType(), hotbar_inv[index].GetAmount());
            hotbar_inv.RemoveAt(index);

            UpdateHotbar();
        }
    }
}

