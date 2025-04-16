using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //UI�󿡼� �������� ���� �׸��� �۾��� �ϴ� Ŭ�����Դϴ�.
    public class LineDrawer : MaskableGraphic
    {
        [System.Serializable]
        public struct Line
        {
            public Vector2 startPoint; // ���� ���� ��ġ
            public Vector2 endPoint;   // ���� �� ��ġ

            public Line(Vector2 startPoint, Vector2 endPoint)
            {
                this.startPoint = startPoint;
                this.endPoint = endPoint;
            }
        }

        public List<Line> lines = new List<Line>(); // ���� ���� �����ϴ� ����Ʈ
        public float thickness = 5f;                // ���� �β�

        public void AddLine(Vector2 startP, Vector2 endP)
        {
            lines.Add(new Line(startP, endP));
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            foreach (var line in lines)
            {
                DrawLine(vh, line.startPoint, line.endPoint);
            }
        }

        private void DrawLine(VertexHelper vh, Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = (endPoint - startPoint).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * thickness / 2;

            Vector2 v0 = startPoint + perpendicular;
            Vector2 v1 = startPoint - perpendicular;
            Vector2 v2 = endPoint - perpendicular;
            Vector2 v3 = endPoint + perpendicular;

            int startIndex = vh.currentVertCount;

            // �� ���� ���ؽ��� �߰��Ͽ� ���� �簢������ ����ϴ�
            vh.AddVert(v0, color, Vector2.zero);
            vh.AddVert(v1, color, Vector2.zero);
            vh.AddVert(v2, color, Vector2.zero);
            vh.AddVert(v3, color, Vector2.zero);

            // �ﰢ�� �� ���� �簢��(���� �׸�)�� ����
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
    }
}