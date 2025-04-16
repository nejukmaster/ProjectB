using ProjectB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //재료를 소모하여 새로운 Receipe를 해금할 수 있는 UI입니다.
    public class InvastigationUI : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] ItemContainer Result;
        [SerializeField] GameObject IngredientsPrefab;
        [SerializeField] GameObject IngredientsContainer;
        [SerializeField] TextMeshProUGUI nameTmp;

        Receipe currentReceipe;

        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }

        public void Open(Receipe receipe)
        {
            currentReceipe = receipe;
            Result.SetItem(receipe.Result);
            Result.ToggleAmountDisplay(false);
            nameTmp.text = receipe.Result.GetType().ToString();
            foreach (ItemStack ingredient in receipe.Ingredients)
            {

            }
            gameObject.SetActive(true);
        }

        public void UnlockReceipe()
        {
            Inventory inventory = InventorySystem.instance.GetInventory();
            if (inventory.RemoveItem(currentReceipe.Ingredients))
            {
                currentReceipe.bIsEnabled = true;
                MainUI.instance.GetReceipeAcheivementUI().UpdateReceipeAcheivement();
                gameObject.SetActive(false);
            }
        }
    }
}
