using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //물을 떠서 "WATER_BOTTLE" 아이템을 얻을 수 있는 InteractableObject
    public class Well : InteractableObject
    {
        
        public override void InteractCallback(PlayerController playerController)
        {
            InventorySystem.instance.GetInventory().AddItem(ItemType.WATER_BOTTLE, 1);
            WorldParticleSystem.instance.PlayParticle(ParticleType.TWINKLE, playerController.transform.position + new Vector3(0f, 0.6f, 0f));
            MainUI.instance.GetProfile().FaceChange(ProfileFace.HAPPY, 1f);
        }

        public override void InteractPreprocess(PlayerController playerController)
        {
            
        }
    }
}
