using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PatrolPoints
{
    public class PatrolPoint : MonoBehaviour
    {
        [SerializeField] private List<PatrolPoint> _neighbors = new();
        [SerializeField] private float _gizmoRadius = 1f;
        [SerializeField] private Color _gizmoColor = new Color32(145, 49, 202, 255);
        
        public Vector3 Position => transform.position;
        public IReadOnlyList<PatrolPoint> Neighbors => _neighbors;
 
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(transform.position, _gizmoRadius);
 
            foreach (PatrolPoint n in _neighbors)
            {
                if (n == null)
                    continue;
                Gizmos.DrawLine(transform.position, n.transform.position);
            }
        }
    }
}