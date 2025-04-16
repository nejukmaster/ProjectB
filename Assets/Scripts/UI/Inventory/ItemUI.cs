using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //ItemContainer���ο� �������� Sprite�� ǥ���ϴ� UI�Դϴ�.
    public class ItemUI : MonoBehaviour
    {
        public void SetItem(ItemType itemType)
        {
            Sprite itemSprite = itemType.GetSprite();
            GetComponent<Image>().color = itemType.GetColor();
            GetComponent<Image>().sprite = itemSprite;
        }
    }

}
