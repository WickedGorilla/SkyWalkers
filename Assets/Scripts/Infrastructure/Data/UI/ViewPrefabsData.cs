using System.Collections.Generic;
using UI.ViewService;
using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "ViewPrefabsData", menuName = "ScriptableObjects/ViewPrefabsData", order = 0)]
    public class ViewPrefabsData : ScriptableObject
    {
        [SerializeField] private Canvas _root;
        [SerializeField] private List<View> _prefabs;

        public Canvas Root => _root;
        public IEnumerable<View> Prefabs => _prefabs;
    }
}