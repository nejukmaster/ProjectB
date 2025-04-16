using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public enum CookwareType
    {
        STOVE,
        OVEN,
        FIRE_FIT
    }

    //CookUI를 열 수 있는 InteractableObject입니다.
    public class Cookware : InteractableObject
    {
        public float CookingRate = 0.0f;
        public bool isCompleted = false;

        [SerializeField] CookwareType type;

        Receipe currentReceipe;
        float cookingSec = 0f;

        public override void InteractCallback(PlayerController playerController)
        {
            MainUI.instance.Toggle(PopupUI.COOKING, cookware: this);
        }

        public override void InteractPreprocess(PlayerController playerController)
        {

        }
        
        public CookwareType GetCookwareType()
        {
            return type;
        }

        public void StartCooking(Receipe receipe)
        {
            currentReceipe = receipe;
        }

        public Receipe GetCurrentReceipe()
        {
            return currentReceipe;
        }

        public void Complete()
        {
            InventorySystem.instance.GetInventory().AddItem(currentReceipe.Result.GetType(), currentReceipe.Result.GetAmount());
            isCompleted = false;
            currentReceipe = null;
        }

        private void Update()
        {
            if (currentReceipe != null && !isCompleted)
            {
                cookingSec += Time.deltaTime;
                CookingRate = cookingSec / currentReceipe.CookingSec;
                if(cookingSec >= currentReceipe.CookingSec)
                {
                    isCompleted = true;
                    cookingSec = 0f;
                    CookingRate = 0f;
                }
            }
        }
    }
}
