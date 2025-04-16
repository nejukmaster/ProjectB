using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectB
{
    public enum GrindType
    {
        WHEAT,
        BARLEY,
        OATS,
        RICE,
        CORN,
        SUGAR_CANE,
        NONE,
    }

    //Farmland에서 활성화되는 농작물의 클래스입니다.
    public class Grinds : MonoBehaviour
    {

        public GrindType currentGrind = GrindType.NONE;
        public bool bCanHarvest;
        public Action<int,bool> GrowingCallback;
        

        [SerializeField] MeshFilter meshFilters;
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] GameObject notify;

        [SerializeField] float GrowSec;

        float _oversec = 0;

        GrindAsset currentGrindAsset;
        int grindLevel = 0;

        private void Start()
        {
            FarmingSystem.instance.RegisterGrinds(this);
        }

        public void UpdateGrowing(float sec)
        {
            if(currentGrindAsset != null && grindLevel < currentGrindAsset.GrindLevelsNum-1)
            {
                _oversec += sec;
                if(_oversec >= GrowSec)
                {
                    Levelup();
                    _oversec -= GrowSec;
                }
            }
            else if(currentGrindAsset != null && grindLevel == currentGrindAsset.GrindLevelsNum - 1)
            {
                notify.SetActive(true);
                bCanHarvest = true;
            }
        }

        public void SetGrind(GrindType grindType)
        {
            currentGrind = grindType;
            currentGrindAsset = FarmingSystem.instance.GetGrind(grindType);

            meshRenderer.material = currentGrindAsset.GetMaterial();
            GrowSec = currentGrindAsset.GetSec();
            bCanHarvest = false;
            grindLevel = 0;
            SetLevel(0);
        }

        public bool SetLevel(int level)
        {
            if(FarmingSystem.instance.GetGrind(currentGrind).GrindLevelsNum > level)
            {
                meshFilters.mesh = FarmingSystem.instance.GetGrind(currentGrind).GetMesh(level);
                grindLevel = level;
                return true;
            }
            return false;
        }

        public bool Levelup()
        {
            if(FarmingSystem.instance.GetGrind(currentGrind).GrindLevelsNum > grindLevel + 1)
            {
                grindLevel++;
                SetLevel(grindLevel);
                GrowingCallback(grindLevel, grindLevel == currentGrindAsset.GrindLevelsNum-1);
                return true;
            }
            return false;
        }

        public ItemType GetCrop()
        {
            return currentGrindAsset.GetCrop();
        }

        public void Init()
        {
            meshFilters.mesh = null;
            meshRenderer.material = null;
            notify.SetActive(false);
            currentGrindAsset = null;
            currentGrind = GrindType.NONE;
            bCanHarvest = false;
            GrowSec = 0f;
            _oversec = 0f;
            GrowingCallback = null;
        }
    }
}
