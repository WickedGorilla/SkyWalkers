using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "ViewPrefabsData", menuName = "ScriptableObjects/ViewPrefabsData", order = 0)]
    public class ViewPrefabsData : ScriptableObject
    {
        [SerializeField] private UIRoot _root;
        [SerializeField] private List<View> _prefabs;

        public UIRoot Root => _root;
        public IEnumerable<View> Prefabs => _prefabs;
    }
}