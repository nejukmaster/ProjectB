using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB {
    //�÷��̾��� �κ��丮�� �����ϴ� �̱��� �ý��� Ŭ�����Դϴ�.
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem instance;

        Inventory inventory;
        Inventory hotbar;

        void Start()
        {
            instance = this;
            inventory = new Inventory(40);
            hotbar = new Inventory(8);
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public Inventory GetHotbar()
        {
            return hotbar;
        }
    }
}
