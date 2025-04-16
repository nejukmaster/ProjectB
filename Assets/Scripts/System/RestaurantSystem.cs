using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //�Ĵ翡 �մ��� �湮�ϴ� ����� ����ϴ� �̱��� �ý��� Ŭ�����Դϴ�.
    public class RestaurantSystem : MonoBehaviour
    {
        [SerializeField] ConsumerAsset[] consumerList;
        [SerializeField] RestaurantSeat[] seats;

        List<Consumer> consumers = new List<Consumer>();
        List<GameObject> cars = new List<GameObject>();
        
        // Start is called before the first frame update
        void Start()
        {
            //Ȯ���� ���� �մ��� �湮�ϴ� �̺�Ʈ�� DayCycleSystem�� ����մϴ�.
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
