using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public enum ParticleType
    {
        TWINKLE
    }

    //각종 파티클의 생성을 담당하는 싱글톤 시스템 클래스입니다.
    public class WorldParticleSystem : MonoBehaviour
    {
        public static WorldParticleSystem instance;

        [SerializeField] ParticleSystem twinkle;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        public void PlayParticle(ParticleType particleType, Vector3 pos)
        {
            switch (particleType)
            {
                case ParticleType.TWINKLE:
                    twinkle.transform.position = pos; 
                    twinkle.Play();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
