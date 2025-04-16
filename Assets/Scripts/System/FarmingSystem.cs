using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���۹��� ���� ������ �����ϴ� �̱��� �ý��� Ŭ�����Դϴ�.
    public class FarmingSystem : MonoBehaviour
    {
        public static FarmingSystem instance;

        [SerializeField] public GrindManagerAsset grindManagerAsset;
        
        List<Grinds> GrindsList = new List<Grinds>();

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            //DayCycleSystem�� ���۹��� �ڶ�� �̺�Ʈ�� �߰����ݴϴ�.
            DayCycleSystem.instance.RegisterDayTimeEvent(new DayTimeEvent(new HashSet<DayTime> { DayTime.MORNING, DayTime.AFTERNOON },
                                                                            1.0f,
                                                                            int.MaxValue,
                                                                            GrindsGrowingAction));
        }

        public GrindAsset GetGrind(GrindType grindType)
        {
            return grindManagerAsset.GrindAssetList[(int)grindType];
        }

        public void RegisterGrinds(Grinds grinds)
        {
            GrindsList.Add(grinds);
        }

        public void GrindsGrowingAction()
        {
            foreach(Grinds grinds in GrindsList)
            {
                grinds.UpdateGrowing(Time.deltaTime);
            }
        }
    }
}
