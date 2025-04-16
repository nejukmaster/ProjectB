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

    //��ȣ�ۿ� ������ Object���� �ֻ��� Ŭ�����Դϴ�.
    //��ȣ�ۿ��� Callback�� ��ȣ�ۿ� ��ó�� �ݹ��� �����ؾ� �ϹǷ� �߻� Ŭ������ �����մϴ�.
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
