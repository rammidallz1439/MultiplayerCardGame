using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vault
{
    public class ObjectPoolManager : IController
    {
        private static ObjectPoolManager instance;
        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        public static ObjectPoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPoolManager();
                }
                return instance;

            }
        }

        /// <summary>
        /// Creates a pool with the given type of object and capacity
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initialCapacity"></param>
        /// <param name="parent"></param>
        public void InitializePool(GameObject prefab, int initialCapacity, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is null. Cannot create a pool.");
                return;
            }

            string key = prefab.name;

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<GameObject>();
                prefabDictionary[key] = prefab;

                for (int i = 0; i < initialCapacity; i++)
                {
                    GameObject newObj = MonoHelper.Instance.InstantiateObject(prefab, parent);

                    if (newObj == null)
                    {
                        Debug.LogError($"Failed to instantiate object: {key}");
                        continue;
                    }

                    if (parent != null)
                        newObj.transform.position = parent.transform.position;

                    newObj.SetActive(false);
                    newObj.name = prefab.name;
                    poolDictionary[key].Enqueue(newObj);
                }
            }
            else
            {
                Debug.LogWarning($"Pool for {key} already exists!");
            }
        }


        /// <summary>
        /// Gets an object from the pool
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="activeState"></param>
        /// <returns></returns>
        public GameObject Get(string prefabName, bool activeState, Transform parent = null)
        {
            string key = prefabName.Replace("(Clone)", "").Trim();

            // Check if a pool exists and has objects
            if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
            {
                GameObject obj;

                // Ensure the dequeued object is not destroyed
                do
                {
                    obj = poolDictionary[key].Dequeue();
                }
                while (obj == null && poolDictionary[key].Count > 0); // Keep dequeuing if null

                if (obj != null)
                {
                    obj.SetActive(activeState);
                    return obj;
                }
            
            }

            // If no object in the pool, try to create a new one
            if (prefabDictionary.ContainsKey(key))
            {
                Debug.Log($"Creating a new instance for {key} since the pool is empty.");

                GameObject newObj = MonoHelper.Instance.InstantiateObject(prefabDictionary[key], parent);

                if (newObj == null)
                {
                    Debug.LogError($"Failed to instantiate object: {key}");
                    return null;
                }

                if (parent != null)
                    newObj.transform.position = parent.transform.position;

                newObj.SetActive(true);
                return newObj;
            }

            Debug.LogError($"No pool or prefab exists for: {key}");
            return null;
        }


        /// <summary>
        /// Returns Object to pool
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnToPool(GameObject obj)
        {
            string key = obj.name.Replace("(Clone)", "").Trim();

            if (poolDictionary.ContainsKey(key))
            {
                obj.SetActive(false);
                poolDictionary[key].Enqueue(obj);
            }
            else
            {
                MonoHelper.Instance.DestroyObject(obj);
            }
        }

        public void ClearPools()
        {
            prefabDictionary.Clear();
            poolDictionary.Clear();
        }


        #region contract 
        public void OnInitialized()
        {
        }

        public void OnRegisterListeners()
        {
        }

        public void OnRelease()
        {
        }

        public void OnRemoveListeners()
        {
        }

        public void OnStarted()
        {
        }

        public void OnVisible()
        {
        }
        #endregion
    }
}

