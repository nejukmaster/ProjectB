using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //UI상에서 동적으로 선을 그리는 작업을 하는 클래스입니다.
    public class LineDrawer : MaskableGraphic
    {
        [System.Serializable]
        public struct Line
        {
            public Vector2 startPoint; // 선의 시작 위치
            public Vector2 endPoint;   // 선의 끝 위치

            public Line(Vector2 startPoint, Vector2 endPoint)
            {
                this.startPoint = startPoint;
                this.endPoint = endPoint;
            }
        }

        public List<Line> lines = new List<Line>(); // 여러 선을 저장하는 리스트
        public float thickness = 5f;                // 선의 두께

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

            // 네 개의 버텍스를 추가하여 선을 사각형으로 만듭니다
            vh.AddVert(v0, color, Vector2.zero);
            vh.AddVert(v1, color, Vector2.zero);
            vh.AddVert(v2, color, Vector2.zero);
            vh.AddVert(v3, color, Vector2.zero);

            // 삼각형 두 개로 사각형(선을 그릴)을 구성
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
    }
}