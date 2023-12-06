using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GalleryVr
{
    public class ImageDescription : MonoBehaviour
    {
        private const float ShowAnimationDuration = 1.5f;
        private const float HideAnimationDuration = 0.5f;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _imageName;
        [SerializeField] private TextMeshProUGUI _imageDescription;

        // maybe close description by hand touching

        private void Awake()
        {
            SetActiveImmediately(false);
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

        public void SetImageInfo(string imageName, string description)
        {
            _imageName.text = imageName;
            _imageDescription.text = description;
        }

        private void SetActiveImmediately(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;
            _canvasGroup.gameObject.SetActive(isActive);
        }
    }
}