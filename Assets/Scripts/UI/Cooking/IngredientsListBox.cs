using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���� ������ Receipe�� ���� ������ ǥ���ϴ� UI�Դϴ�.
    public class IngredientsListBox : MonoBehaviour
    {
        [SerializeField] int IngredientsListItemCount = 50;
        [SerializeField] GameObject ContentContainer;
        [SerializeField] GameObject IngredientsListItem;

        //�� ��Ḧ ǥ���ϴ� IngredientsListItem�� ObjectPooling�� ����Ʈ
        List<IngredientsListItem> poolingItems = new List<IngredientsListItem>();
        
        public void Setup()
        {
            for (int i = 0; i < IngredientsListItemCount; i++)
            {
                GameObject obj = Instantiate(IngredientsListItem, ContentContainer.transform);
                poolingItems.Add(obj.GetComponent<IngredientsListItem>());
                obj.SetActive(false);
            }
        }

        public void Initialize()
        {
            foreach (IngredientsListItem item in poolingItems)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void SetReceipe(Receipe receipe)
        {
            foreach (ItemStack ingredient in receipe.Ingredients)
                AddIngredientsItem(ingredient);
        }

        public bool GetIngredientsAllReady()
        {
            bool result = true;
            foreach (IngredientsListItem item in poolingItems)
            {
                if (!item.IsEnough() && item.gameObject.activeSelf)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public void AddIngredientsItem(ItemStack ingredient)
        {
            foreach (IngredientsListItem item in poolingItems)
            {
                if (item.gameObject.activeInHierarchy) continue;

                item.SetIngredient(ingredient);
                item.gameObject.SetActive(true);
                break;
            }
        }
    }
}
