using UnityEngine;

namespace Project.Scripts.UI.Elements
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        private void Start() => 
            _camera = Camera.main;

        private void LateUpdate()
        {
            Quaternion rotation = _camera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
        }
    }
}