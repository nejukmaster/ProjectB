using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //게임에서 "하루"를 총 4개의 부분으로 나눕니다.
    public enum DayTime
    {
        MORNING,
        AFTERNOON,
        EVENING,
        NIGHT
    }

    //"하루"중 발생할 수 있는 이벤트의 클래스입니다.
    public class DayTimeEvent
    {
        public HashSet<DayTime> encounterDayTime;
        public float encounterProbability;
        public int maxEncounterOnDay;
        public Action action;

        public int encounterNum = 0;
        public bool enabled = true;

        public DayTimeEvent(HashSet<DayTime> encounterDayTime, float encounterProbability, int maxEncounterOnDay, Action action)
        {
            this.encounterDayTime = encounterDayTime;
            this.encounterProbability = encounterProbability;
            this.maxEncounterOnDay = maxEncounterOnDay;
            this.action = action;
        }
    }

    //게임에서 "하루" 사이클을 관리하는 클래스로, 시간의 흐름, 이벤트 발생등을 관리합니다.
    public class DayCycleSystem : MonoBehaviour
    {
        public static DayCycleSystem instance;

        public DayTime dayTime = DayTime.MORNING;

        [SerializeField] public float oneDaySec = 120f;
        [SerializeField] public float morningCycleSec = 20f;
        [SerializeField] public float afternoonCycleSec = 60f;
        [SerializeField] public float eveningCycleSec = 40f;

        float dayCycleSec = 0;

        //"하루" 동안 발생가능한 이벤트들을 저장합니다.
        List<DayTimeEvent> dayTimeEventManager = new List<DayTimeEvent>();

        void Start()
        {
            instance = this;
            InitializeDayTime();
        }

        // Update is called once per frame
        void Update()
        {
            if(dayCycleSec > 0)
            {
                dayCycleSec -= Time.deltaTime;
                if(dayCycleSec <= 0)
                {
                    dayTime = DayTime.NIGHT;
                }
                else if(dayCycleSec <= oneDaySec - morningCycleSec - afternoonCycleSec)
                {
                    dayTime = DayTime.EVENING;
                }
                else if(dayCycleSec <= oneDaySec - morningCycleSec)
                {
                    dayTime = DayTime.AFTERNOON;
                }
                foreach(DayTimeEvent e in dayTimeEventManager)
                {
                    if (!e.encounterDayTime.Contains(dayTime) || !e.enabled) continue;

                    float rand = UnityEngine.Random.value;
                    if(rand < e.encounterProbability && e.encounterNum < e.maxEncounterOnDay)
                    {
                        e.action();
                        e.encounterNum++;
                    }
                }
            }
        }

        public void InitializeDayTime()
        {
            dayTime = DayTime.MORNING;
            dayCycleSec = oneDaySec;
            foreach(DayTimeEvent e in dayTimeEventManager)
            {
                e.encounterNum = 0;
            }
        }

        public void RegisterDayTimeEvent(DayTimeEvent dayTimeEvent)
        {
            dayTimeEventManager.Add(dayTimeEvent);
        }
    }
}
