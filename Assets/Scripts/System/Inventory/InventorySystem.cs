using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB {
    //플레이어의 인벤토리를 관리하는 싱글톤 시스템 클래스입니다.
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
