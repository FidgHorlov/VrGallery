using System;
using Oculus.Interaction;
using UnityEngine;

namespace GalleryVr
{
    public class ImageHandler : MonoBehaviour
    {
        private const float SizeDecrease = 0.2f;
        private readonly Vector3 VerticalRotation = new Vector3(0f, 0f, 0f);
        private readonly Vector3 HorizontalRotation = new Vector3(0f, 0f, 90f);
        private readonly Vector2 DefaultTextureSize = new Vector2(512, 512);

        public event Action<ImageHandler> UserNearImage;

        [SerializeField] private Transform _pictureTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ImageDescription _imageDescription;
        [SerializeField] private InteractableUnityEventWrapper _teleportWrapper;

        private Material _currentMaterial;
        private bool _isHorizontal;

        private Material CurrentMaterial
        {
            get
            {
                if (_currentMaterial is null)
                {
                    _currentMaterial = new Material(Shader.Find("Unlit/Texture"));
                    _currentMaterial.EnableKeyword("_EMISSION");
                    _currentMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 100));
                    _currentMaterial.mainTexture = new Texture2D(512, 512);
                }

                return _currentMaterial;
            }
        }

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
            Vector2 targetSize;
            _isHorizontal = targetTexture.width > targetTexture.height;
            if (_isHorizontal)
            {
                _pictureTransform.localEulerAngles = HorizontalRotation;
                targetTexture = targetTexture.RotateTexture(false);
                targetSize = RecalculateSize(targetTexture);
                targetTexture.ResizeTexture2D((int) targetSize.x, (int) targetSize.y);
            }
            else
            {
                _pictureTransform.localEulerAngles = VerticalRotation;
                targetSize = RecalculateSize(targetTexture);
                targetTexture.ResizeTexture2D((int) targetSize.x, (int) targetSize.y);
            }

            CurrentMaterial.mainTexture = targetTexture;
        }

        private Vector2 RecalculateSize(Texture2D targetTexture)
        {
            float aspectRatio = (float) targetTexture.width / targetTexture.height;

            float widthToFit = DefaultTextureSize.x;
            float heightToFit = DefaultTextureSize.x / aspectRatio;

            if (heightToFit > DefaultTextureSize.y)
            {
                heightToFit = DefaultTextureSize.y;
                widthToFit = DefaultTextureSize.y * aspectRatio;
            }

            return new Vector2(widthToFit, heightToFit);
        }

        public void SetImageInformation(string imageName, string description)
        {
            _imageDescription.SetImageInfo(imageName, description);
            _imageDescription.SetDescriptionPosition(_isHorizontal);
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