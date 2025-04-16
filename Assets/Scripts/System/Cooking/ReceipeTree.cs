using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //ReceipeTree 에셋에 들어가는 노드 클래스
    [Serializable]
    public class ReceipeTreeNode
    {
        [SerializeField] public Receipe data;
        [SerializeField] public List<ReceipeTreeNode> children;


        public ReceipeTreeNode(Receipe data, List<ReceipeTreeNode> children)
        {
            this.data = data;
            this.children = children;
        }

        public ReceipeTreeNode(Receipe data)
        {
            this.data = data;
            this.children = new List<ReceipeTreeNode>();
        }
    }

    //각 Receipe들의 Tree구조를 저장하는 에셋 클래스
    [CreateAssetMenu(fileName = "New ReceipeTree", menuName = "ProjectB/Receipe Tree Asset")]
    public class ReceipeTree : ScriptableObject
    {
        [SerializeField] public List<ReceipeTreeNode> basicReceipes;

        //일반 클래스로 선언된 ReceipeTreeNode를 저장하기위한 리스트
        [SerializeField] List<ReceipeTreeNode> _node_serialize_set = new List<ReceipeTreeNode>();

        //현재 Tree의 BFS 탐색 메서드. 각 요소 탐색시 호출될 CallBack을 지정할 수 있습니다.
        public void BFS(Action<ReceipeTreeNode, ReceipeTreeNode> elementFindCallback)
        {
            if (basicReceipes == null) return;
            Queue<ReceipeTreeNode> queue = new Queue<ReceipeTreeNode>(basicReceipes);
            Dictionary<ReceipeTreeNode, ReceipeTreeNode> parentMap = new Dictionary<ReceipeTreeNode, ReceipeTreeNode>();

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (elementFindCallback != null)
                {
                    if (parentMap.ContainsKey(node))
                        elementFindCallback(node, parentMap[node]);
                    else
                        elementFindCallback(node, null);
                }

                for (int i = 0; i < node.children.Count; i++)
                {
                    queue.Enqueue(node.children[i]);
                    parentMap[node.children[i]] = node;
                }
            }
        }

        public void ClearSerializeSet()
        {
            _node_serialize_set.Clear();
        }

        public void AddSerializedNode(ReceipeTreeNode node)
        {
            _node_serialize_set.Add(node);
        }
    }
}
