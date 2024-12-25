using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace UI.Views
{
    public class PasswordView : View
    {
        [SerializeField] private UILineRenderer _lineRenderer;
        [SerializeField] private RectTransform[] _circles;
        [SerializeField] private RectTransform _viewTransofrm;

        private LinkedList<int> _selectedNodes = new();
        
        private int _selectedNodesMask;

        private void Awake()
            => ResetPattern();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                ResetPattern();

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewTransofrm, mousePosition, null,
                    out _);

                for (int i = 0; i < _circles.Length; i++)
                {
                    RectTransform node = _circles[i];
                    
                    if (CheckContainsInBitmask(_selectedNodesMask, i))
                        continue;
                    
                    if (RectTransformUtility.RectangleContainsScreenPoint(node, mousePosition)) 
                        AddNode(i);
                }
                
                _selectedNodesMask = GenerateBitmask(_selectedNodes);
            }
        }
        
        private int GenerateBitmask(IEnumerable<int> indexes)
        {
            int bitmask = 0;

            foreach (var index in indexes) 
                bitmask |= 1 << index;

            return bitmask;
        }

        private bool CheckContainsInBitmask(int mask, int index) 
            => (mask & (1 << index)) != 0;
        
        private void AddNode(int index)
        {
            _selectedNodes.AddLast(index);
            _lineRenderer.SetPoints(GetSelectedNodes());
        }

        private void ResetPattern()
        {
            _selectedNodes.Clear();
            _lineRenderer.ClearPoints();
        }
        
        private IEnumerable<Vector2> GetSelectedNodes()
        {
            foreach (int index in _selectedNodes)
                yield return _circles[index].transform.localPosition;
        }
    }
}