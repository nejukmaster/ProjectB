using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //다른 장소로 플레이어를 옮겨주는 InteractableObject 클래스입니다.
    public class Door : InteractableObject
    {
        [SerializeField] public Door target;
        
        [SerializeField] public string currentLayer;
        [SerializeField] public Transform TeleportationTransform;

        public override void InteractCallback(PlayerController playerController)
        {
            playerController.Teleport(target.TeleportationTransform);
            Camera camera = playerController.GetComponent<CameraController>().GetCamera();
            int to = LayerMask.NameToLayer(target.currentLayer);
            int from = LayerMask.NameToLayer(currentLayer);
            camera.cullingMask &= ~(1 << from);
            camera.cullingMask |= (1<<to);
        }

        public override void InteractPreprocess(PlayerController playerController)
        {
           
        }
    }
}
