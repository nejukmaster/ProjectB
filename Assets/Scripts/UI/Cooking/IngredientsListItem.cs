using ProjectB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectB
{
    //IngredientsListBox내에서 각각의 재료를 현재 수량과 필요수량으로 각각 표시하는 UI입니다.
    public class IngredientsListItem : MonoBehaviour
    {
        [SerializeField] ItemContainer itemContainer;
        [SerializeField] TextMeshProUGUI ingredientsAmountDisplay;

        int targetAmount = 0;
        int owningAmount = 0;

        public void SetIngredient(ItemStack itemStack)
        {
            Inventory playerInventory = InventorySystem.instance.GetInventory();
            int amount = 0;
            foreach (var item in playerInventory)
            {
                if (item.GetType() == itemStack.GetType())
                {
                    amount += item.GetAmount();
                }
            }
            owningAmount = amount;
            targetAmount = itemStack.GetAmount();
            itemContainer.SetItem(itemStack);
            itemContainer.ToggleAmountDisplay(false);
            ingredientsAmountDisplay.text = amount + "/" + itemStack.GetAmount();
        }

        public bool IsEnough()
        {
            return owningAmount >= targetAmount;
        }
    }
}
