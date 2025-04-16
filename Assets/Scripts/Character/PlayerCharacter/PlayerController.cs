using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace ProjectB
{
    public enum KeyboardControlmode
    {
        PLAIN,
        COOKING
    }
    public class PlayerController : MonoBehaviour
    {
        public bool bIsSit = false;
        public bool bInventoryVisible = false;
        public KeyboardControlmode keyboardControlMode = KeyboardControlmode.PLAIN;

        public GameObject focusedInteractableObj;
        
        public PlayerKeyMappingAsset keyMappingAsset = new PlayerKeyMappingAsset();
        public PlayerInfo playerInfo = new PlayerInfo();
        [SerializeField] public float playerSpeed = 0.5f;
        [SerializeField] public float unfocusingDistance = 2.5f;

        [SerializeField] Animator animator;
        [SerializeField] EquipmentSocket equipmentSocket;

        Vector3 collisionPosition;

        CharacterController characterController
        {
            get
            {
                return GetComponent<CharacterController>();
            }
        }
        bool bCanMove
        {
            get
            {
                return !bIsSit  && !bInventoryVisible && !animator.GetBool("OnSit") && !animator.GetBool("Hoeing");
            }
        }
        bool bCanInteract
        {
            get
            {
                return !bInventoryVisible;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //플레이어가 들고있는 아이템을 표시
            _playerEquipItem();
            //플레이어 상호작용 검사
            bool interacted = _playerInteract();
            //인벤토리 열기 버튼 검사
            _openInventory();
            //팝업을 닫는지 검사
            _closePopup();
            //플레이어 핫키 변경 입력 검사
            _hotbarControl();

            //플레이어 움직임
            Vector3 movement = Vector3.zero;
            //상호작용키와 동시입력 방지
            if(!interacted)
                movement += (bCanMove? 1.0f : 0.0f) * playerSpeed * _playerMovement().normalized * Time.deltaTime;

            //중력 적용
            if(!characterController.isGrounded)
                movement += Physics.gravity * Time.deltaTime;

            float _currentSpeed = (new Vector3(movement.x, 0f, movement.z)).sqrMagnitude;
            characterController.Move(movement);

            if(_currentSpeed > 0.0003)
                transform.rotation = Quaternion.LookRotation((new Vector3(movement.x, 0f, movement.z)).normalized);

            //포커싱된 오브젝트와 일정이상 떨어졌을 때
            if (focusedInteractableObj&&(collisionPosition-transform.position).sqrMagnitude > unfocusingDistance)
            {
                focusedInteractableObj = null;
            }

            animator.SetFloat("Speed", _currentSpeed);

        }

        public void Sit(Transform pTransform)
        {
            characterController.enabled = false;
            transform.position = pTransform.position;
            transform.rotation = pTransform.rotation;
            characterController.enabled = true;

            if (!animator.GetBool("OnSit"))
            {
                bIsSit = true;
                collisionPosition = transform.position;
            }
            animator.SetBool("OnSit", !animator.GetBool("OnSit"));
        }

        public void Teleport(Transform pTransform)
        {
            characterController.enabled = false;
            transform.position = pTransform.position;
            transform.rotation = pTransform.rotation;
            characterController.enabled = true; 
        }

        public void OnIdle()
        {
            bIsSit = false;
        }

        public void SetHoeing(bool pBool)
        {
            if (!pBool)
                animator.Play("Idle");
            animator.SetBool("Hoeing",pBool);
        }

        public ItemStack GetCurrentItem()
        {
            ItemStack focusingItem = MainUI.instance.GetHotbar().GetFocusingItem();
            if (focusingItem != null)
                return focusingItem;
            else
                return new ItemStack(ItemType.NONE, 0);
        }

        private Vector3 _playerMovement()
        {
            Vector3 dir = Vector3.zero;

            if (Input.GetKey(keyMappingAsset.up))
                dir += new Vector3(1, 0, 0);
            if (Input.GetKey(keyMappingAsset.down))
                dir += new Vector3(-1, 0, 0);
            if (Input.GetKey(keyMappingAsset.left))
                dir += new Vector3(0, 0, 1);
            if (Input.GetKey(keyMappingAsset.right))
                dir += new Vector3(0, 0, -1);

            return dir;
        }

        private bool _playerInteract()
        {
            if (!bCanInteract)
                return false;
            if (Input.GetKeyDown(keyMappingAsset.interaction) && focusedInteractableObj)
            {
                focusedInteractableObj.GetComponent<InteractableObject>()?.OnInteract(this);
                return true;
            }
            return false;
        }

        private void _openInventory()
        {
            if(Input.GetKeyDown(keyMappingAsset.toggleInventory))
            {
                bInventoryVisible = true;
                MainUI.instance.Toggle(PopupUI.INVENTORY);
            }
        }

        private void _closePopup()
        {
            if (Input.GetKeyDown(keyMappingAsset.closePopup))
            {
                bInventoryVisible = false;
                MainUI.instance.ClosePopup();
            }
        }
        private void _hotbarControl()
        {
            if (Input.GetKeyDown(keyMappingAsset.hotbar1))
                MainUI.instance.GetHotbar().SetHotbarFocus(0);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar2))
                MainUI.instance.GetHotbar().SetHotbarFocus(1);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar3))
                MainUI.instance.GetHotbar().SetHotbarFocus(2);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar4))
                MainUI.instance.GetHotbar().SetHotbarFocus(3);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar5))
                MainUI.instance.GetHotbar().SetHotbarFocus(4);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar6))
                MainUI.instance.GetHotbar().SetHotbarFocus(5);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar7))
                MainUI.instance.GetHotbar().SetHotbarFocus(6);
            else if (Input.GetKeyDown(keyMappingAsset.hotbar8))
                MainUI.instance.GetHotbar().SetHotbarFocus(7);
        }

        private void _playerEquipItem()
        {
            ItemStack currentItem = GetCurrentItem();
            equipmentSocket.EquipItem(currentItem.GetType());
        }

        public void SetKeyMappingAsset(PlayerKeyMappingAsset keyMappingAsset)
        {
            this.keyMappingAsset = keyMappingAsset;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(hit.gameObject.GetComponent<InteractableObject>())
            {
                collisionPosition = transform.position;
                focusedInteractableObj = hit.gameObject;
            }
        }
    }
}
