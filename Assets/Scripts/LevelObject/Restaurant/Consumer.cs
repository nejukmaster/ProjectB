using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB {

    //�ΰ��ӿ��� �Ĵ翡 �湮�ϴ� �մ� ��ü�� ���� Ŭ�����Դϴ�.
    //��ȣ�ۿ�� Script�� �ͷ��� �� �ֵ���  InteractableObject�� �����մϴ�.
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
