using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PatrolPoints;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PointProvider
{
    public class GraphPatrolPointProvider : IPatrolPointProvider
    {
        private readonly Transform _agentTransform;
        private readonly IReadOnlyList<PatrolPoint> _allPoints;
 
        private PatrolPoint _currentPoint;
        private PatrolPoint _previousPoint;
        private readonly float _pointSearchRadius;

        public GraphPatrolPointProvider(Transform agentTransform, IReadOnlyList<PatrolPoint> allPoints, float pointSearchRadius)
        {
            _agentTransform = agentTransform;
            _allPoints = allPoints;
            _pointSearchRadius = pointSearchRadius;
        }
 
        public bool TryGetNextPoint(Vector3 moveDirection, out Vector3 point)
        {
            if (_allPoints == null || _allPoints.Count == 0)
            {
                point = default;
                return false;
            }
 
            if (_currentPoint == null)
            {
                _currentPoint = FindRandomInRadius(_agentTransform.position, _pointSearchRadius, excluding: null)
                                ?? FindNearest(_agentTransform.position, excluding: null);
                
                if (_currentPoint == null)
                {
                    point = default;
                    return false;
                }
                
                _previousPoint = null;
                point = _currentPoint.Position;
                return true;
            }
 
            PatrolPoint next = PickNeighbor(_currentPoint, _previousPoint, moveDirection)
                               ?? FindRandomInRadius(_agentTransform.position, _pointSearchRadius, excluding: _currentPoint)
                               ?? FindNearest(_agentTransform.position, excluding: _currentPoint);
 
            if (next == null)
            {
                point = default;
                return false;
            }
 
            _previousPoint = _currentPoint;
            _currentPoint = next;
            point = next.Position;
            return true;
        }
        
        private PatrolPoint FindRandomInRadius(Vector3 currentPosition, float radius, PatrolPoint excluding)
        {
            float squareRadius = radius * radius;
            List<PatrolPoint> candidates = new();

            foreach (PatrolPoint point in _allPoints)
            {
                if (point == null || point == excluding)
                    continue;

                float squareDistance = (point.Position - currentPosition).sqrMagnitude;
                
                if (squareDistance <= squareRadius) 
                    candidates.Add(point);
            }

            if (candidates.Count == 0)
                return null;

            return candidates[Random.Range(0, candidates.Count)];
        }
 
        private PatrolPoint FindNearest(Vector3 currentPosition, PatrolPoint excluding)
        {
            float leastSquareDistance = float.MaxValue;
            PatrolPoint nearestPoint = null;
 
            foreach (PatrolPoint point in _allPoints)
            {
                if (point == null || point == excluding)
                    continue;
 
                float squareDistance = (point.Position - currentPosition).sqrMagnitude;
                if (squareDistance < leastSquareDistance)
                {
                    leastSquareDistance = squareDistance;
                    nearestPoint = point;
                }
            }
 
            return nearestPoint;
        }
 
        private static PatrolPoint PickNeighbor(PatrolPoint currentPoint, PatrolPoint avoidPoint, Vector3 incomingMoveDirection)
        {
            if (currentPoint.Neighbors == null || currentPoint.Neighbors.Count == 0)
                return null;
 
            List<PatrolPoint> suitablePoints = new(
                currentPoint.Neighbors.Where(point => point != avoidPoint && point != null));
 
            if (suitablePoints.Count == 0) 
                return null;
            
            if (suitablePoints.Count == 1) 
                return suitablePoints[0];
 
            List<PatrolPoint> forwardPoints = GetForwardNeighborPoints(currentPoint, incomingMoveDirection, suitablePoints);
            
            if (forwardPoints.Count > 0)
                suitablePoints = forwardPoints;

            return suitablePoints[Random.Range(0, suitablePoints.Count)];
        }

        private static List<PatrolPoint> GetForwardNeighborPoints(
            PatrolPoint currentPoint, 
            Vector3 incomingMoveDirection,
            List<PatrolPoint> suitablePoints)
        {
            incomingMoveDirection.y = 0f;
            List<PatrolPoint> forwardPoints = new(suitablePoints.Count);
            
            if (incomingMoveDirection.sqrMagnitude > 0.01f)
            {
                incomingMoveDirection.Normalize();
 
                foreach (PatrolPoint forwardPoint in suitablePoints)
                {
                    Vector3 directionToPoint = forwardPoint.Position - currentPoint.Position;
                    directionToPoint.y = 0f;
                    if (directionToPoint.sqrMagnitude < 0.0001f)
                        continue;
 
                    if (Vector3.Dot(directionToPoint.normalized, incomingMoveDirection) > 0f) // angle < 90°
                        forwardPoints.Add(forwardPoint);
                }
            }

            return forwardPoints;
        }
    }
}