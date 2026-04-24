using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public class Legacy_NavMeshPathFollower
    {
        private readonly Transform _transform;
        private readonly NavMeshPath _path;
        private int _cornerIndex;
        private bool _hasPath;
 
        public Legacy_NavMeshPathFollower(Transform transform)
        {
            _transform = transform;
            _path = new NavMeshPath();
        }
 
        public bool HasPath => 
            _hasPath && _cornerIndex < _path.corners.Length;
 
        public bool SetDestination(Vector3 destination)
        {
            bool ok = NavMesh.CalculatePath(_transform.position, destination, NavMesh.AllAreas, _path);
            _hasPath = ok
                       && _path.status == NavMeshPathStatus.PathComplete
                       && _path.corners.Length > 1;
            _cornerIndex = _hasPath ? 1 : 0;
            return _hasPath;
        }
 
        public void Reset()
        {
            _hasPath = false;
            _cornerIndex = 0;
            _path.ClearCorners();
        }
        
        public bool AdvanceAndCheckCompleted(float cornerReachDistance)
        {
            if (!HasPath)
                return true;
 
            Vector3 toCorner = _path.corners[_cornerIndex] - _transform.position;
            toCorner.y = 0f;
 
            if (toCorner.sqrMagnitude <= cornerReachDistance * cornerReachDistance)
                _cornerIndex++;
 
            return _cornerIndex >= _path.corners.Length;
        }
 
        public Vector3 GetMoveDirection()
        {
            if (!HasPath)
                return Vector3.zero;
 
            Vector3 dir = _path.corners[_cornerIndex] - _transform.position;
            dir.y = 0f;
            return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;
        }
    }
}