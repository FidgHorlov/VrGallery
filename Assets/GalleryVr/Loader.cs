using DG.Tweening;
using UnityEngine;

namespace GalleryVr
{
    public class Loader : MonoBehaviour
    {
        private const float RotateAnimationTime = 1f;
        private const float ScaleAnimationTime = 3f;
        
        private readonly Vector3 RotateValue = new Vector3(0, 0, -360f);
        private readonly Vector3 LogoScaleValue = new Vector3(2f, 2f, 2f);

        [SerializeField] private Transform _loaderTransform;
        [SerializeField] private Transform _logoTransform;
        [SerializeField] private GameObject _loaderGameObject;

        private void OnDestroy()
        {
            StopLoader();
        }

        public void StartLoader()
        {
            _loaderGameObject.SetActive(true);
            _loaderTransform.DOLocalRotate(RotateValue, RotateAnimationTime).SetLoops(-1).SetId(_loaderTransform);
            _logoTransform.DOScale(LogoScaleValue, ScaleAnimationTime).SetLoops(-1, LoopType.Yoyo).SetId(_logoTransform);
        }

        public void StopLoader()
        {
            DOTween.Kill(_loaderTransform);
            DOTween.Kill(_logoTransform);
            _loaderGameObject.SetActive(false);
        }
        
    }
}