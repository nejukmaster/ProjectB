using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    //ReceipeTree ���¿� ���� ��� Ŭ����
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

    //�� Receipe���� Tree������ �����ϴ� ���� Ŭ����
    [CreateAssetMenu(fileName = "New ReceipeTree", menuName = "ProjectB/Receipe Tree Asset")]
    public class ReceipeTree : ScriptableObject
    {
        [SerializeField] public List<ReceipeTreeNode> basicReceipes;

        //�Ϲ� Ŭ������ ����� ReceipeTreeNode�� �����ϱ����� ����Ʈ
        [SerializeField] List<ReceipeTreeNode> _node_serialize_set = new List<ReceipeTreeNode>();

        //���� Tree�� BFS Ž�� �޼���. �� ��� Ž���� ȣ��� CallBack�� ������ �� �ֽ��ϴ�.
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
