using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //ReceipeAcheivementUI�� �� �� �ִ� InteractableObjectŬ���� �Դϴ�.
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
