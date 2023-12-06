using UnityEngine;

namespace GalleryVr
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;

        private Transform _currentTransform;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void Update()
        {
            _currentTransform.position = _targetTransform.position;
        }
    }
}