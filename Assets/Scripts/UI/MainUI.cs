using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //UI를 관리하는 최상단 클래스입니다.
    public enum PopupUI
    {
        NONE,
        INVENTORY,
        COOKING,
        RECEIPE_ACHEIVEMENT,
        SCRIPT
    }
    public class MainUI : MonoBehaviour
    {
        public static MainUI instance;

        [SerializeField] GameObject Inventory;
        [SerializeField] GameObject Hotbar;
        [SerializeField] GameObject Profile;
        [SerializeField] GameObject InteractionProgress;
        [SerializeField] GameObject CookingPanel;
        [SerializeField] GameObject ReceipeAcheivement;
        [SerializeField] GameObject ScriptPanel;

        PopupUI currentPopup = PopupUI.NONE;

        private void Start()
        {
            instance = this;
            SetupUI();
            InventorySystem.instance.GetHotbar().AddItem(ItemType.CORN_SEED, 1);
            InventorySystem.instance.GetHotbar().AddItem(ItemType.HOE,1);
        }

        private void Update()
        {
            GetHotbar().UpdateHotbar();
        }

        public void SetupUI()
        {
            Inventory.GetComponentInChildren<InventoryContainer>().Setup();
            CookingPanel.GetComponent<CookingUI>().Setup();
            ReceipeAcheivement.GetComponent<ReceipeAcheivementUI>().Setup();
        }

        public void OpenScriptPanel(ScriptGroup scriptGroup, Consumer currentConsumer)
        {
            ScriptPanel.gameObject.SetActive(true);
            GetScriptPanel().SetCurrentConsumer(currentConsumer);
            GetScriptPanel().SetScriptGroup(scriptGroup);
            GetScriptPanel().OutputNextScript();
        }

        public void Toggle(PopupUI popup, Cookware cookware = null)
        {
            if (currentPopup == PopupUI.NONE)
            {
                switch (popup)
                {
                    case PopupUI.INVENTORY:
                        ToggleInventory(true);
                        break;
                    case PopupUI.COOKING:
                        ToggleCookingUI(true, cookware);
                        break;
                    case PopupUI.RECEIPE_ACHEIVEMENT:
                        ToggleReceipeAcheivement(true);
                        break;
                }
                currentPopup = popup;
            }
        }

        public void ClosePopup()
        {
            switch (currentPopup)
            {
                case PopupUI.INVENTORY:
                    ToggleInventory(false);
                    break;
                case PopupUI.COOKING:
                    ToggleCookingUI(false, null);
                    break;
                case PopupUI.RECEIPE_ACHEIVEMENT:
                    ToggleReceipeAcheivement(false);
                    break;
            }
            currentPopup = PopupUI.NONE;
        }

        public void ToggleInventory(bool bToggle)
        {
            Inventory.SetActive(bToggle);
            Profile.SetActive(!bToggle);
            if (bToggle)
                Inventory.GetComponentInChildren<InventoryContainer>().OpenInventory(InventorySystem.instance.GetInventory());
        }

        public void ToggleCookingUI(bool bToggle, Cookware cookware)
        {
            CookingPanel.SetActive(bToggle);
            if(bToggle == true)
                CookingPanel.GetComponent<CookingUI>().Initialize(cookware);
            Profile.SetActive(!bToggle);
        }

        public void ToggleReceipeAcheivement(bool bToggle)
        {
            if(bToggle)
                GetReceipeAcheivementUI().UpdateReceipeAcheivement();
            ReceipeAcheivement.SetActive(bToggle);
        }

        public InventoryContainer GetInventory()
        {
            return Inventory.GetComponentInChildren<InventoryContainer>();
        }

        public Hotbar GetHotbar()
        {
            return Hotbar.GetComponent<Hotbar>();
        }

        public Profile GetProfile()
        {
            return Profile.GetComponent<Profile>();
        }

        public ReceipeAcheivementUI GetReceipeAcheivementUI()
        {
            return ReceipeAcheivement.GetComponent<ReceipeAcheivementUI>();
        }

        public ScriptUI GetScriptPanel()
        {
            return ScriptPanel.GetComponent<ScriptUI>();
        }

        public void ShowInteractionProgression(bool useProgression, float progressionTime, float waitTime, Action Callback, Action Preprocess)
        {
            InteractionProgress.GetComponent<InteractionProgress>().StartProgression(useProgression, progressionTime, waitTime, Callback, Preprocess);
        }
    }
}
