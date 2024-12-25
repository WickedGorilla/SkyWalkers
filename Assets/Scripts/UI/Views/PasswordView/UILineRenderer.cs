using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic
    {
        public List<Vector2> points = new(); 
        public float thickness = 5f;
        public float pointSize = 10f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (points.Count < 2)
                return;

            float halfThickness = thickness / 2f;
            float halfPointSize = pointSize / 2f;

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 start = points[i];
                Vector2 end = points[i + 1];

                Vector2 direction = (end - start).normalized;
                Vector2 perpendicular = new Vector2(-direction.y, direction.x) * halfThickness;

                Vector2 v1 = start - perpendicular;
                Vector2 v2 = start + perpendicular;
                Vector2 v3 = end - perpendicular;
                Vector2 v4 = end + perpendicular;

                int index = vh.currentVertCount;

                vh.AddVert(v1, color, Vector2.zero);
                vh.AddVert(v2, color, Vector2.zero);
                vh.AddVert(v3, color, Vector2.zero);
                vh.AddVert(v4, color, Vector2.zero);

                vh.AddTriangle(index, index + 1, index + 2);
                vh.AddTriangle(index + 1, index + 3, index + 2);
            }

            foreach (Vector2 point in points)
            {
                AddPoint(vh, point, halfPointSize);
            }
        }

        private void AddPoint(VertexHelper vh, Vector2 center, float radius)
        {
            Vector2 v1 = center + new Vector2(-radius, -radius);
            Vector2 v2 = center + new Vector2(-radius, radius);
            Vector2 v3 = center + new Vector2(radius, radius);
            Vector2 v4 = center + new Vector2(radius, -radius);

            int index = vh.currentVertCount;

            vh.AddVert(v1, color, Vector2.zero);
            vh.AddVert(v2, color, Vector2.zero);
            vh.AddVert(v3, color, Vector2.zero);
            vh.AddVert(v4, color, Vector2.zero);

            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index, index + 2, index + 3);
        }

        public void SetPoints(IEnumerable<Vector2> newPoints)
        {
            points = newPoints.ToList();
            SetVerticesDirty();
        }

        public void ClearPoints() 
            => points = new List<Vector2>();
    }
}