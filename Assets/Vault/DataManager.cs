using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Vault
{
    public class DataManager : IController
    {
        private static DataManager instance;
        private readonly string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");


        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }
        public void Save<T>(T data, string fileName)
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string filePath = saveDirectory + fileName;
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fileStream, data);
            }
        }

        public T Load<T>(string fileName)
        {
            string filePath = saveDirectory + fileName;

            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    return (T)formatter.Deserialize(fileStream);
                }
            }
            else
            {

                Debug.LogWarning("Save file not found CReating path: " + filePath);
                return default;
            }
        }

        public bool SaveExists(string fileName)
        {
            string filePath = Path.Combine(saveDirectory, fileName);
            return File.Exists(filePath);
        }

        public void DeleteSave(string fileName)
        {
            string filePath = saveDirectory + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Debug.LogWarning("Save file not found: " + filePath);
            }
        }


        public void SaveJson<T>(T data, string fileName)
        {
            try
            {
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }

                string filePath = Path.Combine(saveDirectory, fileName);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(filePath, json);
                Debug.Log($"Data saved as JSON at: {filePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save JSON data: {ex.Message}");
            }
        }



        public T LoadJson<T>(string fileName)
        {
            try
            {
                string filePath = Path.Combine(saveDirectory, fileName);

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    Debug.Log($"Data loaded as JSON from: {filePath}");
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    Debug.LogWarning($"Save file not found: {filePath}");
                    return default;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load JSON data: {ex.Message}");
                return default;
            }
        }


        #region LocalJson
        public T LoadJsonFromResources<T>(string fileName)
        {
            try
            {
                TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

                if (jsonFile != null)
                {
                    string json = jsonFile.text;
                    Debug.Log($"Data loaded as JSON from Resources: {fileName}");
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    Debug.LogWarning($"Resource file not found: {fileName}");
                    return default;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load JSON data from Resources: {ex.Message}");
                return default;
            }
        }

        #endregion
        #region Contract methods
        public void OnInitialized()
        {
        }

        public void OnVisible()
        {
        }

        public void OnStarted()
        {
        }

        public void OnRegisterListeners()
        {
        }

        public void OnRemoveListeners()
        {
        }

        public void OnRelease()
        {
        }
        #endregion
    }
}
