using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //농작물의 정보 전반을 관리하는 싱글톤 시스템 클래스입니다.
    public class FarmingSystem : MonoBehaviour
    {
        public static FarmingSystem instance;

        [SerializeField] public GrindManagerAsset grindManagerAsset;
        
        List<Grinds> GrindsList = new List<Grinds>();

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            //DayCycleSystem에 농작물이 자라는 이벤트를 추가해줍니다.
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
