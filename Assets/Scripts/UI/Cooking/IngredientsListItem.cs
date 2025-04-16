using ProjectB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectB
{
    //IngredientsListBox������ ������ ��Ḧ ���� ������ �ʿ�������� ���� ǥ���ϴ� UI�Դϴ�.
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
