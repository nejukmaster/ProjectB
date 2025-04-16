using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectB
{
    //내용과 발화자의 정보가 저장되어있는 Script 클래스입니다.
    [System.Serializable]
    public class Script
    {
        [SerializeField] public string speaker;
        [SerializeField] public string messages;
    }

    //한 대화에 출력되는 Script들을 저장하는 클래스입니다.
    [CreateAssetMenu(fileName = "NewScriptGroup", menuName = "ProjectB/ScriptGroup")]
#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class ScriptGroup : ScriptableObject
    {
        [SerializeField] Script[] scriptList;

        public IEnumerator<Script> GetEnumerator()
        {
            return ((IEnumerable<Script>)scriptList).GetEnumerator();
        }
    }
}
