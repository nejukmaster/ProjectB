using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public enum ParticleType
    {
        TWINKLE
    }

    //���� ��ƼŬ�� ������ ����ϴ� �̱��� �ý��� Ŭ�����Դϴ�.
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
