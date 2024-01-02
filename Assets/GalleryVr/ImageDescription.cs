using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GalleryVr
{
    public class ImageDescription : MonoBehaviour
    {
        private const float ShowAnimationDuration = 1.5f;
        private const float HideAnimationDuration = 0.5f;

        private readonly Vector3 VerticalPosition = new Vector3(-1.05f, 0f, 0.04f);
        private readonly Vector3 HorizontalPosition = new Vector3(-1.25f, 0.2f, 0.04f);

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _imageName;
        [SerializeField] private TextMeshProUGUI _imageDescription;
        
        private Transform _currentTransform;
        private Transform CurrentTransform => _currentTransform ??= transform;

        private void Awake()
        {
            SetActiveImmediately(false);
        }

        public void SetDescriptionPosition(bool isHorizontal)
        {
            CurrentTransform.localPosition = isHorizontal ? HorizontalPosition : VerticalPosition;
        }

        public void SetImageInfo(string imageName, string description)
        {
            _imageName.text = imageName;
            _imageDescription.text = description;
        }

        public void SetActive(bool isActive)
        {
            float animationDuration = 0f;
            float targetAlpha = 0f;

            if (isActive)
            {
                _canvasGroup.gameObject.SetActive(true);
                animationDuration = ShowAnimationDuration;
                targetAlpha = 1f;
            }
            else
            {
                animationDuration = HideAnimationDuration;
                targetAlpha = 0f;
            }

            _canvasGroup.DOFade(targetAlpha, animationDuration).OnComplete(() =>
            {
                if (!isActive)
                {
                    _canvasGroup.gameObject.SetActive(false);
                }
            });
        }

        private void SetActiveImmediately(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;
            _canvasGroup.gameObject.SetActive(isActive);
        }
    }
}