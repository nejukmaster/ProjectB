using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //ItemContainer내부에 아이템의 Sprite를 표시하는 UI입니다.
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
