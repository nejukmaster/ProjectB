using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public class AnimationEvent : MonoBehaviour
    {
        public void OnIdle()
        {
            GetComponentInParent<PlayerController>().OnIdle();
        }
    }
}
