using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectB
{
    //����� ��ȭ���� ������ ����Ǿ��ִ� Script Ŭ�����Դϴ�.
    [System.Serializable]
    public class Script
    {
        [SerializeField] public string speaker;
        [SerializeField] public string messages;
    }

    //�� ��ȭ�� ��µǴ� Script���� �����ϴ� Ŭ�����Դϴ�.
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
