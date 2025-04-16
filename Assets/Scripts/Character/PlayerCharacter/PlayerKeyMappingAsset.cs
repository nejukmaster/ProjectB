using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    [System.Serializable]
    public class PlayerKeyMappingAsset
    {
        [SerializeField] public KeyCode up = KeyCode.W;
        [SerializeField] public KeyCode down = KeyCode.S;
        [SerializeField] public KeyCode right = KeyCode.D;
        [SerializeField] public KeyCode left = KeyCode.A;
        [SerializeField] public KeyCode toggleInventory = KeyCode.I;
        [SerializeField] public KeyCode interaction = KeyCode.Space;
        [SerializeField] public KeyCode rush = KeyCode.LeftShift;
        [SerializeField] public KeyCode closePopup = KeyCode.Escape;

        [SerializeField] public KeyCode hotbar1 = KeyCode.Alpha1;
        [SerializeField] public KeyCode hotbar2 = KeyCode.Alpha2;
        [SerializeField] public KeyCode hotbar3 = KeyCode.Alpha3;
        [SerializeField] public KeyCode hotbar4 = KeyCode.Alpha4;
        [SerializeField] public KeyCode hotbar5 = KeyCode.Alpha5;
        [SerializeField] public KeyCode hotbar6 = KeyCode.Alpha6;
        [SerializeField] public KeyCode hotbar7 = KeyCode.Alpha7;
        [SerializeField] public KeyCode hotbar8 = KeyCode.Alpha8;
    }
}
