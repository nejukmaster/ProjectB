using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���ӿ��� "�Ϸ�"�� �� 4���� �κ����� �����ϴ�.
    public enum DayTime
    {
        MORNING,
        AFTERNOON,
        EVENING,
        NIGHT
    }

    //"�Ϸ�"�� �߻��� �� �ִ� �̺�Ʈ�� Ŭ�����Դϴ�.
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

    //���ӿ��� "�Ϸ�" ����Ŭ�� �����ϴ� Ŭ������, �ð��� �帧, �̺�Ʈ �߻����� �����մϴ�.
    public class DayCycleSystem : MonoBehaviour
    {
        public static DayCycleSystem instance;

        public DayTime dayTime = DayTime.MORNING;

        [SerializeField] public float oneDaySec = 120f;
        [SerializeField] public float morningCycleSec = 20f;
        [SerializeField] public float afternoonCycleSec = 60f;
        [SerializeField] public float eveningCycleSec = 40f;

        float dayCycleSec = 0;

        //"�Ϸ�" ���� �߻������� �̺�Ʈ���� �����մϴ�.
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
