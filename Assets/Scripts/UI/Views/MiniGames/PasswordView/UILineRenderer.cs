using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Views
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic
    {
        [FormerlySerializedAs("points")] public List<Vector2> _points = new();
        public float thickness = 5f;

        private List<UIVertex> _cacheVertices = new();
        private List<int> _cacheTriangles = new();
        private bool _meshDirty = true;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (!_meshDirty)
            {
                vh.Clear();
                vh.AddUIVertexStream(_cacheVertices, _cacheTriangles);
                return;
            }

            vh.Clear();

            if (_points.Count < 2)
                return;

            float halfThickness = thickness / 2f;
            _cacheVertices = new List<UIVertex>(_points.Count * 4);
            _cacheTriangles = new List<int>(_points.Count * 6);

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Vector2 start = _points[i];
                Vector2 end = _points[i + 1];

                Vector2 direction = (end - start).normalized;
                Vector2 perpendicular = new Vector2(-direction.y, direction.x) * halfThickness;

                Vector2 v1 = start - perpendicular;
                Vector2 v2 = start + perpendicular;
                Vector2 v3 = end - perpendicular;
                Vector2 v4 = end + perpendicular;

                int index = _cacheVertices.Count;

                _cacheVertices.Add(CreateVertex(v1, color));
                _cacheVertices.Add(CreateVertex(v2, color));
                _cacheVertices.Add(CreateVertex(v3, color));
                _cacheVertices.Add(CreateVertex(v4, color));

                _cacheTriangles.Add(index);
                _cacheTriangles.Add(index + 1);
                _cacheTriangles.Add(index + 2);

                _cacheTriangles.Add(index + 1);
                _cacheTriangles.Add(index + 3);
                _cacheTriangles.Add(index + 2);
            }

            vh.AddUIVertexStream(_cacheVertices, _cacheTriangles);
            _meshDirty = false;
        }

        public void SetPoints(IEnumerable<Vector2> newPoints)
        {
            _points = newPoints.ToList();
            _meshDirty = true;
            SetVerticesDirty();
        }

        public void ClearPoints()
        {
            _points.Clear();
            _meshDirty = true;
            SetVerticesDirty();
        }

        private UIVertex CreateVertex(Vector2 position, Color32 color)
        {
            return new UIVertex
            {
                position = position,
                color = color,
                uv0 = Vector2.zero
            };
        }

        public void UpdateColor(Color newColor)
        {
            
            
            color = newColor;
            
            if (_meshDirty)
                return;
            
            for (int i = 0; i < _cacheVertices.Count; i++)
            {
                var uiVertex = _cacheVertices[i];
                uiVertex.color = newColor;
                _cacheVertices[i] = uiVertex;
            }
        }
    }
}
