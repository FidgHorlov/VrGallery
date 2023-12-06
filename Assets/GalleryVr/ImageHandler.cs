using System;
using Oculus.Interaction;
using UnityEngine;

namespace GalleryVr
{
    public class ImageHandler : MonoBehaviour
    {
        private const float SizeDecrease = 0.2f;
        private readonly Vector3 DefaultScale = Vector3.one;

        public event Action<ImageHandler> UserNearImage;
        [SerializeField] private Transform _pictureTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ImageDescription _imageDescription;
        [SerializeField] private InteractableUnityEventWrapper _teleportWrapper;

        private Material _currentMaterial;
        private Material CurrentMaterial => _currentMaterial ??= new Material(Shader.Find("Unlit/Texture"));

        private void OnEnable()
        {
            _teleportWrapper.WhenSelect.AddListener(PlayerReachedEventHandler);
        }

        private void OnDisable()
        {
            _teleportWrapper.WhenSelect.RemoveListener(PlayerReachedEventHandler);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetImage(Texture2D targetTexture)
        {
            _meshRenderer.material = CurrentMaterial;
            CurrentMaterial.mainTexture = targetTexture;
            Vector3 targetScale = DefaultScale;
            if (targetTexture.width >= targetTexture.height)
            {
                targetScale.x *= (float) targetTexture.width / targetTexture.height;
            }
            else
            {
                targetScale.z *= (float) targetTexture.height / targetTexture.width;
            }

            Debug.Log($"Material: X: {targetTexture.width}; Y: {targetTexture.height}\r\nTarget Scale: {targetScale}. * Size decreaser: {targetScale * SizeDecrease}");
            _pictureTransform.localScale = targetScale * SizeDecrease;
        }

        public void SetImageInformation(string imageName, string description)
        {
            _imageDescription.SetImageInfo(imageName, description);
        }

        public void PlayerGone()
        {
            _imageDescription.SetActive(false);
        }

        private void PlayerReachedEventHandler()
        {
            _imageDescription.SetActive(true);
            UserNearImage?.Invoke(this);
        }

#if UNITY_EDITOR
        [ContextMenu("Come closer")]
        private void ComeCloserEditor()
        {
            PlayerReachedEventHandler();
        }
#endif
    }
}