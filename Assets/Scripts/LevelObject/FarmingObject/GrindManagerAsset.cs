using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //게임에 사용되는 GrindAsset들을 저장하는 에셋 클래스입니다.
    [CreateAssetMenu(fileName = "NewGrindManager", menuName = "ProjectB/GrindManagerAsset")]
    public class GrindManagerAsset : ScriptableObject
    {
        [SerializeField] public List<GrindAsset> GrindAssetList;
    }
}