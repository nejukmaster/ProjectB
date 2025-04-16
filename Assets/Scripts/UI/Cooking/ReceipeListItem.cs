using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //ReceipeListBox������ �� Receipe���� ǥ���ϴ� UI�Դϴ�.
    public class ReceipeListItem : MonoBehaviour,IPointerClickHandler
    {
        public Action<Receipe> onClick;

        [SerializeField] ItemContainer ItemContainer;
        [SerializeField] TextMeshProUGUI description;

        Receipe currentReceipe;

        public void SetReceipe(Receipe receipe)
        {
            currentReceipe = receipe;
            ItemContainer.SetItem(receipe.Result);
            description.text = receipe.Result.GetType().ToString();
            ItemContainer.ToggleAmountDisplay(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick(currentReceipe);
        }
    }
}
