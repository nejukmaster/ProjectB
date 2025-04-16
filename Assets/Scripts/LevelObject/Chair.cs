using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectB
{
    //앉을 수 있는 InteractableObject클래스 입니다.
    public class Chair : InteractableObject
    {
        [SerializeField] Transform sitTransform;

        public override void InteractCallback(PlayerController playerController)
        {
            playerController.Sit(sitTransform);
        }

        public override void InteractPreprocess(PlayerController playerController)
        {
            
        }
    }
}
