using System.IO;
using System.Threading.Tasks;
using Meta.WitAi;
using UnityEngine;

namespace GalleryVr
{
    public static class FileManager
    {
        internal static string Load(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                return null;
            }

            StreamReader streamReader = File.OpenText(pathFile);
            string fileData = streamReader.ReadToEnd();
            streamReader.Close();
            return fileData;
        }

        internal static async Task<byte[]> LoadTextureBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            Task<byte[]> task = Task.Run(()=> File.ReadAllBytesAsync(filePath));
            return await task;
        }

        internal static void Save<T>(T settingsModel, string filePath)
        {
            string jsonData = GetJsonData(settingsModel);
            StreamWriter streamWriter = File.CreateText(filePath);
            streamWriter.Write(jsonData);
            streamWriter.Close();
        }

        public static T GetModelData<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }

        private static string GetJsonData<T>(T savedFile)
        {
            return JsonUtility.ToJson(savedFile);
        }

        private static string GetFileName(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }

            FileInfo file = new FileInfo(path);
            return file.Name;
        }
    }
}