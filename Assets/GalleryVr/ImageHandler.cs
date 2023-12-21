using System;
using Oculus.Interaction;
using UnityEngine;

namespace GalleryVr
{
    public class ImageHandler : MonoBehaviour
    {
        private const float SizeDecrease = 0.2f;
        private readonly Vector3 DefaultScale = Vector3.one;
        private readonly Vector3 VerticalRotation = new Vector3(0f, 0f, 0f);
        private readonly Vector3 HorizontalRotation = new Vector3(0f, 0f, 90f);

        public event Action<ImageHandler> UserNearImage;
        [SerializeField] private Transform _pictureTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ImageDescription _imageDescription;
        [SerializeField] private InteractableUnityEventWrapper _teleportWrapper;

        private Vector2 _defaultTextureSize;
        private Material _currentMaterial;

        private Material CurrentMaterial
        {
            get
            {
                if (_currentMaterial is null)
                {
                    _currentMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    _currentMaterial.EnableKeyword("_EMISSION");
                    _currentMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 100));
                }

                return _currentMaterial;
            }
        }

        private void Awake()
        {
            _defaultTextureSize = new Vector2(CurrentMaterial.mainTexture.width, CurrentMaterial.mainTexture.height);
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

            if (targetTexture.width > targetTexture.height)
            {
                _pictureTransform.localEulerAngles = HorizontalRotation;
                targetTexture.RotateTexture(90f);
                if (targetTexture.width > _defaultTextureSize.x)
                {
                    targetTexture.ResizeTexture2D((int)_defaultTextureSize.x / targetTexture.width, targetTexture.height);
                }
                else if (targetTexture.height > _defaultTextureSize.y)
                {
                    targetTexture.ResizeTexture2D((int)_defaultTextureSize.x / targetTexture.height, targetTexture.width);
                }
            }
            else 
            {
                _pictureTransform.localEulerAngles = VerticalRotation;
                if (targetTexture.width > _defaultTextureSize.x)
                {
                    targetTexture.ResizeTexture2D((int)_defaultTextureSize.x / targetTexture.width, targetTexture.height);
                }
                else if (targetTexture.height > _defaultTextureSize.y)
                {
                    targetTexture.ResizeTexture2D((int)_defaultTextureSize.x / targetTexture.height, targetTexture.width);
                }
            }

            CurrentMaterial.mainTexture = targetTexture;

            // if (targetTexture.width >= targetTexture.height)
            // {
            //     targetScale.x *= (float) targetTexture.width / targetTexture.height;
            // }
            // else
            // {
            //     targetScale.y *= (float) targetTexture.height / targetTexture.width;
            // }

            //
            // Debug.Log($"Material: X: {targetTexture.width}; Y: {targetTexture.height}\r\nTarget Scale: {targetScale}. * Size decreaser: {targetScale * SizeDecrease}");
            // _pictureTransform.localScale = targetScale;// * SizeDecrease;
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


    public static class TextureExtension
    {
        public static Texture2D RotateTexture(this Texture2D originalTexture, float angle)
        {
            float phi = angle / (180f / Mathf.PI);
            float sn = Mathf.Sin(phi);
            float cs = Mathf.Cos(phi);
            Color32[] rotatedPixels = new Color32[originalTexture.width * originalTexture.height];
            Color32[] originalPixels = originalTexture.GetPixels32();

            int xc = originalTexture.width / 2;
            int yc = originalTexture.height / 2;

            for (int index = 0; index < originalTexture.height; index++)
            {
                for (int secondIndex = 0; secondIndex < originalTexture.width; secondIndex++)
                {
                    int x = (int) (cs * (secondIndex - xc) + sn * (index - yc) + xc);
                    int y = (int) (-sn * (secondIndex - xc) + cs * (index - yc) + yc);

                    if ((x > -1) && (x < originalTexture.width) && (y > -1) && (y < originalTexture.height))
                    {
                        rotatedPixels[index * originalTexture.width + secondIndex] = originalPixels[y * originalTexture.width + x];
                    }
                }
            }

            Texture2D rotatedTexture = new Texture2D(originalTexture.width, originalTexture.height);
            rotatedTexture.SetPixels32(rotatedPixels);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

        public static Texture2D ResizeTexture2D(this Texture2D originalTexture, int width, int height)
        {
            // Ensure the original texture is readable
            if (!originalTexture.isReadable)
            {
                Debug.LogError("Texture is not readable");
                return null;
            }

            // Create a copy of the original texture
            Texture2D resizedTexture = new Texture2D(width, height, originalTexture.format, false);

            // Resize the texture
            originalTexture?.Reinitialize(width, height);
            originalTexture.Apply();

            // Copy the resized pixels to the new texture
            Color[] pixels = originalTexture.GetPixels();
            resizedTexture.SetPixels(pixels);
            resizedTexture.Apply();

            return resizedTexture;
        }
    }
}