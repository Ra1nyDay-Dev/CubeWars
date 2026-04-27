using System;
using System.Collections.Generic;

namespace Project.Scripts.Infrastructure.StateMachine
{
    public class StateMachine
    {
        public IState Current { get; private set; }
 
        private readonly Dictionary<IState, List<Transition>> _transitions = new();
        private List<Transition> _currentTransitions = new();
        private readonly List<Transition> _anyTransitions = new();
        
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        public void Tick(float deltaTime)
        {
            Transition triggered = GetTriggeredTransition();
            if (triggered != null)
                SetState(triggered.To);
 
            Current?.Tick(deltaTime);
        }

        public void SetState(IState state)
        {
            if (state == null || state == Current)
                return;
 
            Current?.Exit();
            Current = state;
            _currentTransitions = _transitions.TryGetValue(Current, out List<Transition> list)
                ? list
                : EmptyTransitions;
            Current.Enter();
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!_transitions.TryGetValue(from, out List<Transition> list))
            {
                list = new List<Transition>();
                _transitions[from] = list;
            }
            list.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition) =>
            _anyTransitions.Add(new Transition(to, condition));
        
        public void RestartState(IState state)
        {
            if (state == null)
                return;
 
            Current?.Exit();
            Current = state;
            _currentTransitions = _transitions.GetValueOrDefault(Current, EmptyTransitions);
            Current.Enter();
        }

        private Transition GetTriggeredTransition()
        {
            foreach (Transition anyTransition in _anyTransitions)
                if (anyTransition.Condition())
                    return anyTransition;
 
            foreach (Transition transition in _currentTransitions)
                if (transition.Condition())
                    return transition;
 
            return null;
        }
    }
}