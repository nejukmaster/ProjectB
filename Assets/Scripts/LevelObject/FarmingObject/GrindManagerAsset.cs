using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //���ӿ� ���Ǵ� GrindAsset���� �����ϴ� ���� Ŭ�����Դϴ�.
    [CreateAssetMenu(fileName = "NewGrindManager", menuName = "ProjectB/GrindManagerAsset")]
    public class GrindManagerAsset : ScriptableObject
    {
        [SerializeField] public List<GrindAsset> GrindAssetList;
    }
}