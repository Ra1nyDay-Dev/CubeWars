using System;
using UnityEngine;

namespace Project.Scripts.Characters.CubeGuy.Animations
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Animator))]
    public class CubeGuyAnimations : MonoBehaviour
    {
        private Animator _animator;
        private Character _character;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
        }
    }
}