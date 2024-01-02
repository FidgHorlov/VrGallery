using System;
using System.IO;
using System.Threading.Tasks;
using GalleryVr;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace GalleryVr
{
    public class SettingsModel
    {
        public ImageModel[] ImageModel;
    }

    [Serializable]
    public class ImageModel
    {
        public string Name;
        public string Description;
        public string ImageName;
    }

    public static class Settings
    {
        private static readonly string DefaultFolderLocation = Application.persistentDataPath;
        private static readonly string ImagesFolderName = "Images";
        private static readonly string DefaultFileName = "ApplicationSettings";

        private static string _fullSettingsPath;
        private static string _imageFolder;

        public static SettingsModel GetSettingsFile()
        {
            string settingsJson = FileManager.Load(GetSettingsFilePath());
            if (string.IsNullOrEmpty(settingsJson))
            {
                return null;
            }

            SettingsModel settings = FileManager.GetModelData<SettingsModel>(settingsJson);
            if (settings == null)
            {
                Debug.LogError($"Can't parse settings data!");
            }

            return settings;
        }

        [ItemCanBeNull]
        public static async Task<Texture2D> GetTexture(string fileName)
        {
            string path = Path.Combine(GetImageFolderPath(), fileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"{fileName} - image not exist!");
                return null;
            }

            Task<byte[]> bytes = FileManager.LoadTextureBytes(path);

            while (!bytes.IsCompleted)
            {
                await Task.Yield();
            }

            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(bytes.Result, false);
            texture2D.Apply();
            return texture2D;
        }


        private static string GetSettingsFilePath()
        {
            if (!string.IsNullOrEmpty(_fullSettingsPath))
            {
                return _fullSettingsPath;
            }

            string path = Path.Combine(DefaultFolderLocation, DefaultFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"Settings file not exist");
                return null;
            }

            _fullSettingsPath = path;
            return _fullSettingsPath;
        }

        private static string GetImageFolderPath()
        {
            if (!string.IsNullOrEmpty(_imageFolder))
            {
                return _imageFolder;
            }

            string folderPath = Path.Combine(DefaultFolderLocation, ImagesFolderName);
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError($"Image folder not exist");
                return null;
            }

            _imageFolder = folderPath;
            return _imageFolder;
        }

#if UNITY_EDITOR
        public static void CreateSettingsFileEditor()
        {
            SettingsModel settingsModel = new SettingsModel();
            ImageModel imageModel = new ImageModel {Name = "ImageName", Description = "Description", ImageName = "FileName.jpg"};
            settingsModel.ImageModel = new[] {imageModel};
            FileManager.Save(settingsModel, Path.Combine(DefaultFolderLocation, DefaultFileName));
            Debug.Log($"File created! {Path.Combine(DefaultFolderLocation, DefaultFileName)}");
        }
#endif
    }
}