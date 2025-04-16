using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectB
{
    //���� �� �ִ� InteractableObjectŬ���� �Դϴ�.
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
