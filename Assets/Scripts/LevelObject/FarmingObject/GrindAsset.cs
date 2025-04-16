using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ProjectB
{
    //작물의 모델, 머티리얼, 획득시 나오는 아이템 타입, 자라는 시간을 설정하는 에셋 클래스입니다.
    [CreateAssetMenu(fileName = "NewGrindAsset", menuName = "ProjectB/GrindAsset")]
#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class GrindAsset : ScriptableObject
    {
        public int GrindLevelsNum
        {
            get
            {
                return meshes.Length;
            }
        }
        [SerializeField] Mesh[] meshes;
        [SerializeField] Material material;
        [SerializeField] ItemType crop;
        [SerializeField] float GrowSec;

        public GrindAsset(Mesh[] meshes, Material materials)
        {
            this.meshes = meshes;
            this.material = materials;
        }

        public Material GetMaterial()
        {
            return material;
        }

        public Mesh GetMesh(int level)
        {
            return meshes[level];
        }

        public ItemType GetCrop()
        {
            return crop;
        }

        public float GetSec()
        {
            return GrowSec;
        }
    }
}
