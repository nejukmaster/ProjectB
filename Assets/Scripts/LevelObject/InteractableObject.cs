using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    [System.Serializable]
    public class InteractionParams
    {
        [SerializeField] public bool bUseProgression;
        [SerializeField] public float progressionTime;
        [SerializeField] public float waitTime;
    }

    //상호작용 가능한 Object들의 최상위 클래스입니다.
    //상호작용후 Callback과 상호작용 전처리 콜백을 설정해야 하므로 추상 클래스로 구현합니다.
    public abstract class InteractableObject : MonoBehaviour
    {
        public InteractionParams interactionParams;

        public bool needItem = false;
        public HashSet<ItemType> InteractItems = new HashSet<ItemType>();

        [SerializeField] List<ItemType> InteractItems_inspector;

        private void OnEnable()
        {
            InitInteractItem();
        }

        public void OnInteract(PlayerController playerController)
        {
            if (InteractItems.Contains(playerController.GetCurrentItem().GetType()) || !needItem)
            {
                MainUI.instance.ShowInteractionProgression(interactionParams.bUseProgression,
                                                            interactionParams.progressionTime,
                                                            interactionParams.waitTime,
                                                            () => { InteractCallback(playerController); },
                                                            () => { InteractPreprocess(playerController); });
            }
        }

        public void InitInteractItem()
        {
            InteractItems = new HashSet<ItemType>(InteractItems_inspector);
        }

        public abstract void InteractCallback(PlayerController playerController);
        public abstract void InteractPreprocess(PlayerController playerController);
    }
}
