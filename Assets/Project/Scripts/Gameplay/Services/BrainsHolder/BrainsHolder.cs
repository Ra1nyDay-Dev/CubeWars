using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.BrainsHolder
{
    public class BrainsHolder : ITickable, IBrainsHolder, IDisposable
    {
        private readonly List<CharacterBrain> _brains = new();

        public void Add(CharacterBrain brain) => _brains.Add(brain);
        public void Remove(CharacterBrain brain) => _brains.Remove(brain);
        public void Clear() => _brains.Clear();

        public void Tick()
        {
            float delta = Time.deltaTime;

            foreach (var brain in _brains)
                brain.Tick();
        }
        
        public void Dispose()
        {
            foreach (CharacterBrain brain in _brains) 
                brain.Dispose();
            
            _brains.Clear();
        }
    }
}