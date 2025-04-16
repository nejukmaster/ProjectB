using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //ReceipeAcheivementUI를 열 수 있는 InteractableObject클래스 입니다.
    public class InvastigationTable : InteractableObject
    {
        public override void InteractCallback(PlayerController playerController)
        {
            MainUI.instance.Toggle(PopupUI.RECEIPE_ACHEIVEMENT);
        }

        public override void InteractPreprocess(PlayerController playerController)
        {

        }
    }
}
