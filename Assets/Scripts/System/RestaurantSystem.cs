using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //식당에 손님이 방문하는 기능을 담당하는 싱글톤 시스템 클래스입니다.
    public class RestaurantSystem : MonoBehaviour
    {
        [SerializeField] ConsumerAsset[] consumerList;
        [SerializeField] RestaurantSeat[] seats;

        List<Consumer> consumers = new List<Consumer>();
        List<GameObject> cars = new List<GameObject>();
        
        // Start is called before the first frame update
        void Start()
        {
            //확률에 따라 손님이 방문하는 이벤트를 DayCycleSystem에 등록합니다.
            DayCycleSystem.instance.RegisterDayTimeEvent(new DayTimeEvent(new HashSet<DayTime> { DayTime.AFTERNOON},
                                                                            0.3f,
                                                                            2,
                                                                            VisitConsumer));
            foreach(ConsumerAsset asset in consumerList)
            {
                GameObject consumer = Instantiate(asset.GetConsumerPrefab());
                //GameObject car = Instantiate(asset.GetCarPrefab());
                consumer.SetActive(false);
                //car.SetActive(false);
                consumers.Add(consumer.GetComponent<Consumer>());
                //cars.Add(car);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void VisitConsumer()
        {
            int idx = Random.Range(0, consumers.Count);
            foreach(RestaurantSeat seat in seats)
            {
                if (seat.IsAvailable())
                {
                    consumers[idx].Visit(seat);
                }
            }
        }
    }
}
