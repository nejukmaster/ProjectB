using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //농작물을 심을 수 있는 InteractableObject입니다.
    public class Farmland : InteractableObject
    {
        public bool bIsReady;
        [SerializeField] public Grinds grinds;
        [SerializeField] public List<ItemType> AvailableSeeds;

        [SerializeField] GameObject Dirt;
        [SerializeField] GameObject soil;

        private GrindType _targetGrind;

        public override void InteractCallback(PlayerController playerController)
        {
            if (!bIsReady)
            {
                WorldParticleSystem.instance.PlayParticle(ParticleType.TWINKLE, playerController.transform.position + new Vector3(0f, 0.6f, 0f));
                Dirt.SetActive(false);
                bIsReady = true;
                playerController.SetHoeing(false);
                MainUI.instance.GetProfile().FaceChange(ProfileFace.HAPPY, 1f);
                this.InteractItems = new HashSet<ItemType>(AvailableSeeds);
            }
            else if (grinds.currentGrind == GrindType.NONE)
            {
                grinds.SetGrind(_targetGrind);
                this.InteractItems = new HashSet<ItemType> { ItemType.WATER_BOTTLE };
                grinds.GrowingCallback += GrindGrowthCallback;
            }
            else if (grinds.bCanHarvest)
            {
                InventorySystem.instance.GetInventory().AddItem(grinds.GetCrop(), 1);
                grinds.Init();
                needItem = true;
                this.InteractItems = new HashSet<ItemType>(AvailableSeeds);
            }
        }

        public override void InteractPreprocess(PlayerController playerController)
        {
            if (!bIsReady)
            {
                playerController.SetHoeing(true);
            }
            else if (grinds.currentGrind == GrindType.NONE)
            {
                switch (playerController.GetCurrentItem().GetType())
                {
                    case ItemType.WHEAT_SEED:
                        _targetGrind = GrindType.WHEAT;
                        break;
                    case ItemType.CORN_SEED:
                        _targetGrind = GrindType.CORN;
                        break;
                    default:
                        break;
                }
            }
            else if (grinds.bCanHarvest)
            {

            }
        }

        public void GrindGrowthCallback(int level, bool bGrownUp)
        {
            if (bGrownUp)
            {
                this.needItem = false;
            }
        }

        public void Init()
        {
            grinds.Init();
            Dirt.SetActive(true);
        }
    }
}
