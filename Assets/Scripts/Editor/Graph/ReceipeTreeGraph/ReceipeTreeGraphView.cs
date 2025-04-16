#if UNITY_EDITOR

using log4net.Util;
using ProjectB;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectB
{
    public class ReceipeTreeGraphView : GraphView
    {
        TreeStartNode startNode;
        Action<DropdownMenuAction> onSave;

        public ReceipeTreeGraphView(Action<DropdownMenuAction> onSave)
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.onSave = onSave;

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(OnContextualMenuPopulate));

            // �׸��� ��� �߰�
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // �巡�� & ��� �̺�Ʈ ���
            RegisterDragAndDrop();
        }

        public void UpdateTreeView(ReceipeTree receipeTree)
        {
            ClearGraphView();

            //Start��� �߰�
            startNode = new TreeStartNode();
            startNode.SetPosition(new Rect(new Vector2(100, 200), Vector2.zero));
            AddElement(startNode);

            int i = 0;
            Dictionary<ReceipeTreeNode, ReceipeNode> nodeMap = new Dictionary<ReceipeTreeNode, ReceipeNode> ();
            receipeTree.BFS((current, from) =>
            {
                var node = new ReceipeNode(current.data);
                if (from != null)
                {
                    ReceipeNode parent = nodeMap[from];
                    Edge edge = new Edge
                    {
                        output = parent.outputPort,
                        input = node.inputPort
                    };
                    edge.output.Connect(edge);
                    edge.input.Connect(edge);

                    AddElement(edge);
                }
                else
                {
                    TreeStartNode parent = startNode;
                    Edge edge = new Edge
                    {
                        output = parent.outputPort,
                        input = node.inputPort
                    };
                    edge.output.Connect(edge);
                    edge.input.Connect(edge);

                    AddElement(edge);
                }
                node.SetPosition(new Rect(new Vector2((i+1)*100, 200), Vector2.zero));
                AddElement(node);
                nodeMap.Add(current,node);
                i++;
            });
        }

        public void ClearGraphView()
        {
            DeleteElements(graphElements.ToList());
        }

        public List<ReceipeTreeNode> GetGraph()
        {
            Dictionary<ReceipeNode, ReceipeTreeNode> nodeMap = new Dictionary<ReceipeNode, ReceipeTreeNode> ();

            Queue<ReceipeNode> queue = new Queue<ReceipeNode>();

            List<ReceipeTreeNode> result = new List<ReceipeTreeNode> ();

            foreach (Edge edge in startNode.outputPort.connections)
            {
                if(edge.input.node is ReceipeNode connectedNode)
                {
                    queue.Enqueue(connectedNode);
                    ReceipeTreeNode node = new ReceipeTreeNode(connectedNode.receipeData);
                    nodeMap.Add(connectedNode, node);
                    result.Add(node);
                }
            }

            while (queue.Count > 0)
            {
                ReceipeNode node = queue.Dequeue();

                foreach(Edge edge in node.outputPort.connections)
                {
                    if(edge.input.node is ReceipeNode connectedNode)
                    {
                        queue.Enqueue(connectedNode);
                        ReceipeTreeNode receipeTreeNode = new ReceipeTreeNode(connectedNode.receipeData);
                        nodeMap.Add(connectedNode, receipeTreeNode);
                        nodeMap[node].children.Add(receipeTreeNode);
                    }
                }
            }

            return result;
        }

        // Drag & Drop ��� �߰�
        private void RegisterDragAndDrop()
        {
            this.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            this.RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            // Receipe Asset�̸� Drag ���� ǥ��
            if (DragAndDrop.objectReferences.Length == 1 && DragAndDrop.objectReferences[0] is ProjectB.Receipe)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            // ��ӵ� ������Ʈ�� Receipe���� Ȯ��
            if (DragAndDrop.objectReferences.Length == 1 && DragAndDrop.objectReferences[0] is ProjectB.Receipe receipe)
            {
                // �巡�� ��ġ�� ���� ReceipeNode ����
                Vector2 mousePosition = evt.mousePosition;
                Vector2 worldPosition = contentViewContainer.WorldToLocal(mousePosition);
                AddReceipeNode(receipe, worldPosition);
                DragAndDrop.AcceptDrag();
            }
        }

        // ReceipeNode�� �߰��ϴ� �޼���
        public void AddReceipeNode(ProjectB.Receipe receipe, Vector2 position)
        {
            var node = new ProjectB.ReceipeNode(receipe);
            node.SetPosition(new Rect(position, Vector2.zero));
            AddElement(node);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private void OnContextualMenuPopulate(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("save", onSave, DropdownMenuAction.Status.Normal);
        }
    }
}

#endif