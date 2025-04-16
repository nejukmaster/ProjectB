#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectB
{
    public class ReceipeTreeGraphWindow : EditorWindow
    {
        private ReceipeTreeGraphView graphView;
        private ReceipeTree currentReceipeTree;

        [MenuItem("Window/ProjectB/Receipe Tree")]
        public static void Open()
        {
            var window = GetWindow<ReceipeTreeGraphWindow>();
            window.titleContent = new GUIContent("Receipe Tree");
        }

        private void OnEnable()
        {
            // GraphView 생성 및 설정
            graphView = new ReceipeTreeGraphView((action) =>
            {
                currentReceipeTree.ClearSerializeSet();
                currentReceipeTree.BFS((current, from) =>
                {
                    currentReceipeTree.AddSerializedNode(current);
                });
            })
            {
                name = "Receipe Tree"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void OnGUI()
        {
            Object selectedAsset = Selection.activeObject;

            if (selectedAsset != null && selectedAsset is ReceipeTree)
            {
                if(currentReceipeTree != selectedAsset)
                {
                    currentReceipeTree = (ReceipeTree)selectedAsset;
                    graphView.UpdateTreeView(currentReceipeTree);
                }

                else
                {
                    currentReceipeTree.basicReceipes = graphView.GetGraph();
                }
            }
            else
            {
                currentReceipeTree = null;
                graphView.ClearGraphView();
            }
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

    }
}

#endif