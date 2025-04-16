using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���� ������ ���� ������ �����ϴ� �̱��� �ý��� Ŭ�����Դϴ�.
    public class GameInstanceSystem : MonoBehaviour
    {
        public static GameInstanceSystem instance;

        [SerializeField] public ReceipeTree ReceipeTree;

        PlayerController localPlayerController;
        // Start is called before the first frame update
        void Start()
        {
            instance = this;

            localPlayerController = FindObjectsByType<PlayerController>(FindObjectsSortMode.InstanceID)[0];
            foreach(ReceipeTreeNode node in ReceipeTree.basicReceipes) 
            {
                node.data.bIsEnabled = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public PlayerController GetLocalPlayer()
        {
            return localPlayerController;
        }
    }
}
