using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //�ֹ���� ��ȣ�ۿ��Ͽ� ǥ�õǴ�, �丮�� �� �� �ְ� �ϴ� UI�Դϴ�.
    public class CookingUI : MonoBehaviour
    {
        [SerializeField] Image process;
        [SerializeField] ItemContainer resultContainer;
        [SerializeField] IngredientsListBox ingredientsListBox;
        [SerializeField] ReceipeListBox receipeListBox;

        Cookware currentCookware;

        private void Update()
        {
            process.fillAmount = currentCookware.CookingRate;
            if(currentCookware.isCompleted )
            {
                resultContainer.SetItem(currentCookware.GetCurrentReceipe().Result);
            }
        }

        public void Initialize(Cookware cookware)
        {
            receipeListBox.Initialize();
            ingredientsListBox.Initialize();

            GameInstanceSystem gameInstance = GameInstanceSystem.instance;
            currentCookware = cookware;
            if(currentCookware.isCompleted)
            {
                ItemStack result = currentCookware.GetCurrentReceipe().Result;
                resultContainer.SetItem(result);
            }
            else
            {
                resultContainer.SetEmpty();
            }
            resultContainer.ToggleAmountDisplay(false);
            gameInstance.ReceipeTree.BFS((current, from) =>
            {
                if (cookware.GetCookwareType() == current.data.type)
                {
                    receipeListBox.AddReceipeItem(current.data);
                }
            });
        }

        public void Setup()
        {
            ingredientsListBox.Setup();
            receipeListBox.Setup();
            resultContainer.onLeftClicked = (index) =>
            {
                if (currentCookware.isCompleted)
                {
                    currentCookware.Complete();
                    resultContainer.SetEmpty();
                    currentCookware.isCompleted = false;
                }
            };
        }

        public ReceipeListBox GetReceipeListBox()
        {
            return receipeListBox;
        }

        public IngredientsListBox GetIngredientsListBox()
        {
            return ingredientsListBox;
        }

        public void OnCookButton()
        {
            Receipe receipe = receipeListBox.GetCurrentReceipe();
            if (receipe != null && ingredientsListBox.GetIngredientsAllReady())
            {
                foreach (ItemStack ingredient in receipe.Ingredients) 
                {
                    InventorySystem.instance.GetInventory().RemoveItem(ingredient.GetType(), ingredient.GetAmount());
                }
                currentCookware.StartCooking(receipe);
            }
            ingredientsListBox.Initialize();
            ingredientsListBox.SetReceipe(receipe);
        }
    }
}
