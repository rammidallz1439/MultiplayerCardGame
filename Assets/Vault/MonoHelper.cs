using System;
using System.Collections;
using System.Collections.Generic;
using Vault;
using UnityEngine;

public class MonoHelper : MonoBehaviour
{
    public static MonoHelper Instance;

    public bool UseLogs;

    private static bool _isShuttingDown = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }



    private void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }

    private void OnDestroy()
    {
        _isShuttingDown = true;
    }


    /// <summary>
    /// Use this to Instantiating an object at runtime and also set the parent at same time
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T InstantiateObject<T>(T obj, Transform parent = null) where T : MonoBehaviour
    {
        return Instantiate(obj, parent);
    }

    /// <summary>
    /// Use this to Instantiating an object at runtime and also set the parent at same time
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject InstantiateObject(GameObject obj, Transform parent = null)
    {
        return Instantiate(obj, parent);
    }

    /// <summary>
    /// Use this to Instantiating an object at runtime and set position and rotation to the object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public GameObject InstantiateObject(GameObject obj, Vector3 pos, Quaternion quaternion)
    {
        return Instantiate(obj, pos, quaternion);
    }

    /// <summary>
    /// Use this to Instantiating an object at runtime and set position and rotation to the object and Parent
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public GameObject InstantiateObject(GameObject obj, Vector3 pos, Quaternion quaternion, Transform parent = null)
    {
        return Instantiate(obj, pos, quaternion, parent);
    }

    /// <summary>
    /// Use this to Destroy an Gameobject
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="delay"></param>
    public void DestroyObject(GameObject obj, float delay = 0)
    {
        Destroy(obj, delay);
    }

    /// <summary>
    /// Use this to Start Coroutine
    /// </summary>
    /// <param name="coroutineCall"></param>
    public Coroutine RunCouroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }
    /// <summary>
    /// Use this to Stop or remove a  Coroutine
    /// </summary>
    /// <param name="coroutineCall"></param>
    public void KillCoroutine(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
    }


    /// <summary>
    /// use this to make a Transform to face towards camera
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="transform"></param>
    public void FaceCamera<T>(Camera camera, T transform) where T : Transform
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        /* Vector3 cameraPosition = camera.transform.position;
         Vector3 targetPosition = transform.position;

         Vector3 directionToCamera = cameraPosition - targetPosition;
         directionToCamera.y = 0;  // Keep the y-axis unchanged to prevent distortion

         Quaternion rotation = Quaternion.LookRotation(directionToCamera);
         transform.rotation = rotation * Quaternion.Euler(0, 180, 0);*/
    }

    /// <summary>
    /// Finds an Object with the given tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public GameObject FindTag(string tag)
    {
        return GameObject.FindWithTag(tag);
    }

    /// <summary>
    /// Stops the Current Action and Starts the Called method in given time if given or stars immedeiatly
    /// </summary>
    /// <param name="name"></param>
    /// <param name="Delay"></param>
    public void InvokeMethod<T>(String name, float Delay = 0)
    {
        Invoke(name, Delay);
    }




    /// <summary>
    /// Destroys all the GameObjects in a list and clears the list.
    /// </summary>
    /// <param name="list">The list of GameObjects to destroy.</param>
    public void DestroyGameObjectInList(List<GameObject> list)
    {
        if (list == null)
        {
            Debug.LogWarning("The list is null and cannot be processed.");
            return;
        }

        // Iterate over each GameObject in the list
        foreach (GameObject item in list)
        {
            // Check if the GameObject is not null before trying to destroy it
            if (item != null)
            {
                // Destroy the GameObject
                Destroy(item);
            }
        }

        // Clear the list
        list.Clear();
    }

    /// <summary>
    /// Destroys all the child objects in a transform
    /// </summary>
    /// <param name="parent"></param>
    public void DestroyAllTheChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }


    }

    /// <summary>
    /// Use this to print a log in the console.
    /// </summary>
    /// <param name="message">The object to be logged.</param>
    /// <param name="color">The color of the log in the console.</param>
    public void PrintMessage(object message, string color = "white")
    {
        if (UseLogs)
        {
            string logMessage = $"<color={color}>{message}</color>";
            Debug.Log("#" + logMessage);
        }

    }

  

}
