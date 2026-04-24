using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public class PatrolGraph : MonoBehaviour
    {
        [SerializeField] private List<PatrolPoint> _points = new();
 
        public IReadOnlyList<PatrolPoint> Points => _points;
 
        [ContextMenu("Collect Children")]
        private void CollectChildren()
        {
            _points.Clear();
            _points.AddRange(GetComponentsInChildren<PatrolPoint>());
        }
    }
}