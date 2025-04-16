using ProjectB;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectB
{
    //Inventory를 표시하는 UI입니다.
    public class InventoryContainer : MonoBehaviour
    {
        [SerializeField] Vector2Int Grid = new Vector2Int(8, 5);
        [SerializeField] GameObject ItemContainer;
        [SerializeField] GameObject InventoryFocus;

        List<ItemContainer> containers = new List<ItemContainer>();

        Inventory opendInventory;
        int focusingContianerIndex = -1;

        // Start is called before the first frame update
        public void Setup()
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 container_size = new Vector2(rect.sizeDelta.x / (float)Grid.x, rect.sizeDelta.y / (float)Grid.y);
            for (int i = 0; i < Grid.y; i++)
            {
                for (int j = 0; j < Grid.x; j++)
                {
                    Vector2 pos = new Vector2((rect.sizeDelta.x / (float)Grid.x) * j, -(rect.sizeDelta.y / (float)Grid.y) * i);
                    GameObject obj = Instantiate(ItemContainer, transform);
                    RectTransform obj_rect = obj.GetComponent<RectTransform>();
                    obj_rect.anchoredPosition = pos;
                    obj_rect.sizeDelta = container_size;

                    obj.GetComponent<ItemContainer>().index = i * Grid.x + j;
                    obj.GetComponent<ItemContainer>().onLeftClicked = OnLeftClickInventory;
                    obj.GetComponent<ItemContainer>().onRightClicked = OnRightClickInventory;

                    containers.Add(obj.GetComponent<ItemContainer>());
                }
            }
        }

        public void UpdateInventory()
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (i < opendInventory.Count)
                {
                    containers[i].SetItem(opendInventory[i]);
                }
                else
                    containers[i].SetEmpty();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenInventory(Inventory inventory)
        {
            opendInventory = inventory;
            UpdateInventory();
            inventory.onInventoryUpdate = UpdateInventory;
        }

        public void OnLeftClickInventory(int index)
        {
            if (focusingContianerIndex == index)
            {
                if (InventoryFocus.activeSelf)
                {
                    InventoryFocus.SetActive(false);
                    focusingContianerIndex = -1;
                }
                else
                {
                    InventoryFocus.SetActive(true);
                    focusingContianerIndex = index;
                }
            }
            else
            {
                if (!InventoryFocus.activeSelf)
                    InventoryFocus.SetActive(true);
                InventoryFocus.GetComponent<RectTransform>().anchoredPosition = containers[index].GetComponent<RectTransform>().anchoredPosition;
                focusingContianerIndex = index;
            }
        }
        public void OnRightClickInventory(int index)
        {
            if (index < opendInventory.Count)
            {
                InventorySystem.instance.GetHotbar().AddItem(opendInventory[index].GetType(), opendInventory[index].GetAmount());
                opendInventory.RemoveAt(index);
                UpdateInventory();
            }
        }
    }
}
