using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //해금 가능한 Receipe와 그 Tree구조를 보여주는 UI입니다.
    public class ReceipeAcheivementUI : MonoBehaviour, IDragHandler
    {
        [SerializeField] float xSpacer;
        [SerializeField] float ySpacer;
        [SerializeField] float dragSpeed;
        [SerializeField] GameObject receipeAcheivementItem;
        [SerializeField] GameObject receipeAcheivementContainer;
        [SerializeField] LineDrawer lineDrawer;
        [SerializeField] InvastigationUI invastigationUI;

        Dictionary<Receipe, ReceipeAcheivementItem> receipeAcheiveItemDic = new Dictionary<Receipe, ReceipeAcheivementItem>();


        public void Setup()
        {
            ReceipeTree receipeTree = GameInstanceSystem.instance.ReceipeTree;

            List<int> layerIndexer = new List<int>();
            foreach (ReceipeTreeNode node in receipeTree.basicReceipes)
            {
                VisualizeReceipeTree(node, 0, ref layerIndexer);
            }
            UpdateReceipeAcheivement();
        }

        public GameObject VisualizeReceipeTree(ReceipeTreeNode node, int layer, ref List<int> layerIndexer)
        {
            if (layer == layerIndexer.Count)
            {
                if (layerIndexer.Count == 0)
                    layerIndexer.Add(0);
                else
                    layerIndexer.Add(layerIndexer.Count - 1);
            }
            else
            {
                layerIndexer[layer] += 1;
            }
            GameObject obj = Instantiate(receipeAcheivementItem, receipeAcheivementContainer.transform);
            obj.GetComponent<ReceipeAcheivementItem>().SetReceipe(node.data);
            obj.GetComponent<ReceipeAcheivementItem>().onClick = (receipe) => { invastigationUI.Open(receipe); };
            obj.GetComponent<RectTransform>().anchoredPosition += new Vector2(xSpacer * layer, -ySpacer * layerIndexer[layer]);
            receipeAcheiveItemDic.Add(node.data, obj.GetComponent<ReceipeAcheivementItem>());
            for (int i = 0; i < layer; i++)
            {
                layerIndexer[i] = layerIndexer[layer];
            }
            foreach (ReceipeTreeNode child in node.children)
            {
                GameObject childObj = VisualizeReceipeTree(child, layer + 1, ref layerIndexer);
                lineDrawer.AddLine(obj.GetComponent<RectTransform>().anchoredPosition, childObj.GetComponent<RectTransform>().anchoredPosition);
            }
            return obj;
        }

        public void OnDrag(PointerEventData eventData)
        {
            receipeAcheivementContainer.GetComponent<RectTransform>().anchoredPosition += eventData.delta * dragSpeed;
        }

        public void UpdateReceipeAcheivement()
        {
            ReceipeTree receipeTree = GameInstanceSystem.instance.ReceipeTree;
            receipeTree.BFS((current, parent) =>
            {
                if (receipeAcheiveItemDic.ContainsKey(current.data))
                {
                    receipeAcheiveItemDic[current.data].ToggleScreen(!current.data.bIsEnabled);
                }
            });
        }
    }
}
