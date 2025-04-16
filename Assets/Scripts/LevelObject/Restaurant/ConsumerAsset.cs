using ProjectB;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ProjectB {

    //Consumer�� ���� ������ �����ϴ� ���� Ŭ�����Դϴ�.
    [CreateAssetMenu(fileName = "NewConsumerAsset", menuName = "ProjectB/ConsumerAsset")]
#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class ConsumerAsset : ScriptableObject
    {
        [SerializeField] GameObject ConsumerPrefab;
        [SerializeField] GameObject CarPrefab;

        public GameObject GetConsumerPrefab()
        {
            return ConsumerPrefab;
        }
        public GameObject GetCarPrefab()
        {
            return CarPrefab;
        }
    }
}
