using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GalleryVr
{
    public class GalleryController : MonoBehaviour
    {
        [SerializeField] private ImageHandler[] _imageHandlers;
        [SerializeField] private Loader _loader;
        [SerializeField] private GameObject _loaderEnvironment;
        [SerializeField] private GameObject _mainEnvironment;
        
        private Texture2D[] _targetTextures;
        private ImageModel[] _imageModels;
        private ImageHandler _currentImageHandler;

        public event Action ImagesDownloadedEvent;

        private void Awake()
        {
            SetActiveMainEnvironment(false);
            foreach (ImageHandler imageHandler in _imageHandlers)
            {
                imageHandler.SetActive(false);
                imageHandler.UserNearImage += UserCameToImage;
            }
        }

        private void OnEnable()
        {
            ImagesDownloadedEvent += ImagesDownloadedEventHandler;
        }

        private void OnDisable()
        {
            ImagesDownloadedEvent -= ImagesDownloadedEventHandler;
        }

        private void OnDestroy()
        {
            foreach (ImageHandler imageHandler in _imageHandlers)
            {
                imageHandler.UserNearImage -= UserCameToImage;
            }
        }

        private IEnumerator Start()
        {
            if (_imageHandlers == null)
            {
                Debug.LogError($"You don't have image handlers!");
                yield break;
            }
            
            GetImageInformation();

            while (_imageModels == null)
            {
                yield return null;
            }

            Debug.LogError($"START SPAWN: {DateTime.Now}");
            int index = 0;
            foreach (ImageModel targetTexture in _imageModels)
            {
                yield return InitImage(index, targetTexture);
                index++;
            }

            ImagesDownloadedEvent?.Invoke();
            Debug.LogError($"Finish SPAWN: {DateTime.Now}");
            SetActiveMainEnvironment(true);
        }

        private void SetActiveMainEnvironment(bool isActive)
        {
            _mainEnvironment.SetActive(isActive);
            _loaderEnvironment.SetActive(!isActive);
            if (isActive)
            {
                _loader.StopLoader();
            }
            else
            {
                _loader.StartLoader();
            }
        }

        private void UserCameToImage(ImageHandler targetImageHandler)
        {
            if (_currentImageHandler != null)
            {
                _currentImageHandler.PlayerGone();
            }

            _currentImageHandler = targetImageHandler;
        }

        private void GetImageInformation()
        {
            SettingsModel settings = Settings.GetSettingsFile();
            if (settings == null)
            {
                return;
            }

            _imageModels = settings.ImageModel;
        }

        private IEnumerator InitImage(int index, ImageModel imageModel)
        {
            if (index > _imageHandlers?.Length)
            {
                Debug.LogError($"Please add more image handlers!");
                yield break;
            }

            Task<Texture2D> texture = Settings.GetTexture(imageModel.ImageName);
            while (!texture.IsCompleted)
            {
                yield return null;
            }

            _imageHandlers[index].SetImage(texture.Result);
            _imageHandlers[index].SetImageInformation(imageModel.Name, imageModel.Description);
        }
        
        private void ImagesDownloadedEventHandler()
        {
            foreach (ImageHandler imageHandler in _imageHandlers)
            {
                imageHandler.SetActive(true);
            }
        }


#if UNITY_EDITOR
        [ContextMenu("Create settings")]
        private void CreateSettingsFile()
        {
            Settings.CreateSettingsFileEditor();
        }

        [ContextMenu("Find all image handlers")]
        private void FindAllImageHandlers()
        {
            _imageHandlers = GetComponentsInChildren<ImageHandler>();
        }

#endif
    }
}