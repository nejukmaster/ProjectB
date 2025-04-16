using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB {

    //인게임에서 식당에 방문하는 손님 객체에 대한 클래스입니다.
    //상호작용시 Script를 촐력할 수 있도록  InteractableObject로 선언합니다.
    public class Consumer : InteractableObject
    {
        [SerializeField] ItemType[] Menues;
        [SerializeField] ScriptGroup Scripts;

        ItemType currentMenu;
        bool bIsReady = false;

        public override void InteractCallback(PlayerController playerController)
        {
            if (!bIsReady)
            {
                MainUI.instance.OpenScriptPanel(Scripts, this);
                bIsReady = true;
                this.InteractItems = new HashSet<ItemType> { currentMenu};
            }
            else
            {

            }
        }

        public override void InteractPreprocess(PlayerController playerController)
        {

        }

        public void Visit(RestaurantSeat seat)
        {
            this.gameObject.SetActive(true);
            this.transform.position = seat.GetSitPos().position;
            this.transform.rotation = seat.GetSitPos().rotation;

            currentMenu = Menues[Random.Range(0, Menues.Length)];
        }

        public ItemType GetCurrentMenu()
        {
            return currentMenu;
        }
    }
}
