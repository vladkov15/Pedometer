  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaTest : MonoBehaviour
{
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    // Start is called before the first frame update
    void Start()
    {
        InitializePlugin("com.example.test.PluginInstance");  
    }

    void InitializePlugin(string plaginName) 
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(plaginName);
        if (_pluginInstance == null)
        {
            Debug.Log("plugin instance error");
        }
        _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
    }

    public void Add()
    {
        if (_pluginInstance != null)
        {
            var result = _pluginInstance.Call<int>("Add", 2, 4);
            Debug.Log("Add result: " + result);
        }   
    }

    public void Toast() 
    {
        if (_pluginInstance != null)
        {
            _pluginInstance.Call("Toast", "working!!!!");
        }
    }
}
