using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB {
    //CookingUI 좌측에서 현재 제작 가능한 Receipe들을 표시하는데 사용합니다.
    public class ReceipeListBox : MonoBehaviour
    {
        [SerializeField] int ReceipeListItemCount = 50;
        [SerializeField] GameObject ContentContainer;
        [SerializeField] GameObject ReceipeListItem;
        [SerializeField] GameObject ResultUI;
        [SerializeField] IngredientsListBox ingredientsListBox;

        List<ReceipeListItem> poolingItems = new List<ReceipeListItem>();
        Receipe currentReceipe = null;

        public void Setup()
        {
            for (int i = 0; i < ReceipeListItemCount; i++)
            {
                GameObject obj = Instantiate(ReceipeListItem, ContentContainer.transform);
                obj.GetComponent<ReceipeListItem>().onClick = (receipe) =>
                {
                    ingredientsListBox.Initialize();
                    ingredientsListBox.SetReceipe(receipe);
                    currentReceipe = receipe;
                };

                poolingItems.Add(obj.GetComponent<ReceipeListItem>());
                obj.SetActive(false);
            }
        }

        public void Initialize()
        {
            foreach (ReceipeListItem item in poolingItems)
            {
                item.gameObject.SetActive(false);
                currentReceipe = null;
            }
        }

        public void AddReceipeItem(Receipe receipe)
        {
            foreach (ReceipeListItem item in poolingItems)
            {
                if (item.gameObject.activeInHierarchy) continue;

                item.SetReceipe(receipe);
                item.gameObject.SetActive(true);
                break;
            }
        }

        public Receipe GetCurrentReceipe()
        {
            return currentReceipe;
        }
    }
}
