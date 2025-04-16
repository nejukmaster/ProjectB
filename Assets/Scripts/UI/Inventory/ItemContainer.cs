using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //InvenotryContainer 내에서 Inventory에 들어있는 각각의 아이템을 표시하는 UI입니다.
    public class ItemContainer : MonoBehaviour,IPointerClickHandler
    {
        public int index;
        public Action<int> onLeftClicked;
        public Action<int> onRightClicked;

        [SerializeField] ItemUI ItemUI;
        [SerializeField] TextMeshProUGUI amountDisplay;

        public void SetItem(ItemStack itemStack)
        {
            amountDisplay.gameObject.SetActive(true);
            ItemUI.SetItem(itemStack.GetType());
            amountDisplay.text = itemStack.GetAmount().ToString();
        }

        public void SetEmpty()
        {
            ItemUI.SetItem(ItemType.NONE);
            amountDisplay.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (onLeftClicked != null)
                    onLeftClicked(index);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (onRightClicked != null)
                    onRightClicked(index);
            }
        }

        public void ToggleAmountDisplay(bool pBool)
        {
            amountDisplay.gameObject.SetActive(pBool);
        }
    }

}
