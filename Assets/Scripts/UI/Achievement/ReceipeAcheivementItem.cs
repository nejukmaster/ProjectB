using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //InvastigationUI������ ������ �����Ǹ� ǥ���ϴ� UI�Դϴ�.
    public class ReceipeAcheivementItem : MonoBehaviour,IPointerClickHandler
    {
        public Action<Receipe> onClick;
        [SerializeField] ItemContainer itemContainer;
        [SerializeField] GameObject screen;

        Receipe currentReceipe;

        public void ToggleScreen(bool flag)
        {
            screen.SetActive(flag);
        }

        public void SetReceipe(Receipe receipe)
        {
            itemContainer.ToggleAmountDisplay(false);
            itemContainer.SetItem(receipe.Result);
            currentReceipe = receipe;
        }

        public Receipe GetReceipe()
        {
            return currentReceipe;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick(currentReceipe);
        }
    }
}
