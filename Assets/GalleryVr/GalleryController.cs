using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GalleryVr
{
    public class GalleryController : MonoBehaviour
    {
        [SerializeField] private ImageHandler[] _imageHandlers;
        private Texture2D[] _targetTextures;
        private ImageModel[] _imageModels;
        private ImageHandler _currentImageHandler;

        private void Awake()
        {
            foreach (ImageHandler imageHandler in _imageHandlers)
            {
                //imageHandler.SetActive(false);
                imageHandler.UserNearImage += UserCameToImage;
            }
        }

        private void OnDestroy()
        {
            foreach (ImageHandler imageHandler in _imageHandlers)
            {
                imageHandler.UserNearImage -= UserCameToImage;
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
            
            int index = 0;
            foreach (ImageModel targetTexture in _imageModels)
            {
                yield return InitImage(index, targetTexture);
                index++;
            }
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
            //_imageHandlers[index].SetActive(true);
            yield return null;
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