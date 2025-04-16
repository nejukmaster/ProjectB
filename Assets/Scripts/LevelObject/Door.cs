using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //�ٸ� ��ҷ� �÷��̾ �Ű��ִ� InteractableObject Ŭ�����Դϴ�.
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
