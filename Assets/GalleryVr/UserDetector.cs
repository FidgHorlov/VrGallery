using System;
using UnityEngine;

namespace GalleryVr
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class UserDetector : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        
        
        public event Action PlayerReached;
        public event Action PlayerGone;
        
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                PlayerReached?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                PlayerGone?.Invoke();
            }
        }
    }
}