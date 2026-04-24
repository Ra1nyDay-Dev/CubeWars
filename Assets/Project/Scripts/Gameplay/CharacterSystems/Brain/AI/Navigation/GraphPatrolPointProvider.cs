using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public class GraphPatrolPointProvider : IPatrolPointProvider
    {
        private readonly Transform _agent;
        private readonly IReadOnlyList<PatrolPoint> _allPoints;
 
        private PatrolPoint _current;
        private PatrolPoint _previous;
        private float _pointSerchRadius = 30f;

        public GraphPatrolPointProvider(Transform agent, IReadOnlyList<PatrolPoint> allPoints)
        {
            _agent = agent;
            _allPoints = allPoints;
        }
 
        public bool TryGetNextPoint(Vector3 forwardHint, out Vector3 point)
        {
            if (_allPoints == null || _allPoints.Count == 0)
            {
                point = default;
                return false;
            }
 
            if (_current == null)
            {
                _current =  _current = FindRandomInRadius(_agent.position, _pointSerchRadius, excluding: null)
                                       ?? FindNearest(_agent.position, excluding: null);
                if (_current == null)
                {
                    point = default;
                    return false;
                }
                _previous = null;
                point = _current.Position;
                return true;
            }
 
            Vector3 incoming = _previous != null
                ? _current.Position - _previous.Position
                : forwardHint;
 
            PatrolPoint next = PickNeighbor(_current, _previous, incoming)
                               ?? FindNearest(_agent.position, excluding: _current);
 
            if (next == null)
            {
                point = default;
                return false;
            }
 
            _previous = _current;
            _current = next;
            point = next.Position;
            return true;
        }
        
        private PatrolPoint FindRandomInRadius(Vector3 from, float radius, PatrolPoint excluding)
        {
            float radiusSqr = radius * radius;
            List<PatrolPoint> candidates = null;

            foreach (PatrolPoint p in _allPoints)
            {
                if (p == null || p == excluding)
                    continue;

                float d = (p.Position - from).sqrMagnitude;
                if (d <= radiusSqr)
                {
                    candidates ??= new List<PatrolPoint>();
                    candidates.Add(p);
                }
            }

            if (candidates == null || candidates.Count == 0)
                return null;

            return candidates[Random.Range(0, candidates.Count)];
        }
 
        private PatrolPoint FindNearest(Vector3 from, PatrolPoint excluding)
        {
            float bestSqr = float.MaxValue;
            PatrolPoint best = null;
 
            foreach (PatrolPoint p in _allPoints)
            {
                if (p == null || p == excluding)
                    continue;
 
                float d = (p.Position - from).sqrMagnitude;
                if (d < bestSqr)
                {
                    bestSqr = d;
                    best = p;
                }
            }
 
            return best;
        }
 
        private static PatrolPoint PickNeighbor(PatrolPoint node, PatrolPoint avoid, Vector3 incomingDir)
        {
            IReadOnlyList<PatrolPoint> nbs = node.Neighbors;
            if (nbs == null || nbs.Count == 0)
                return null;
 
            List<PatrolPoint> pool = new(nbs.Count);
            foreach (PatrolPoint n in nbs)
                if (n != null && n != avoid)
                    pool.Add(n);
 
            if (pool.Count == 0)
                foreach (PatrolPoint n in nbs)
                    if (n != null)
                        pool.Add(n);
 
            if (pool.Count == 0) return null;
            if (pool.Count == 1) return pool[0];
 
            incomingDir.y = 0f;
            if (incomingDir.sqrMagnitude > 0.01f)
            {
                Vector3 inNorm = incomingDir.normalized;
                List<PatrolPoint> forward = new(pool.Count);
 
                foreach (PatrolPoint n in pool)
                {
                    Vector3 to = n.Position - node.Position;
                    to.y = 0f;
                    if (to.sqrMagnitude < 0.0001f)
                        continue;
 
                    if (Vector3.Dot(to.normalized, inNorm) > 0f)   // angle < 90°
                        forward.Add(n);
                }
 
                if (forward.Count > 0)
                    pool = forward;
            }
 
            return pool[Random.Range(0, pool.Count)];
        }
    }
}