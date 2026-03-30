using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.CubeGuy.Animations
{
    public class HitTest : MonoBehaviour
    {
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Sequence _hitSequence;
        
        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();

            _hitSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .SetUpdate(UpdateType.Late);
                
            float flash = 0;
            
            _hitSequence.Append(
                DOTween.To(
                        () => flash,
                        x => {
                            flash = x;
                            SetFlash(x);
                        },
                        1f,
                        0.1f
                    ).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad)
            );
            
            
            _hitSequence.Join(
                transform.DOPunchPosition(Vector3.forward * 0.5f, 0.4f, 10, 1)
            );
            
            _hitSequence.Join(
                transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1)
            );
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                TakeDamage();
        }
        
        private void SetFlash(float value)
        {
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(FlashAmount, value);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        public void TakeDamage() => 
            _hitSequence.Restart();
    }
}
