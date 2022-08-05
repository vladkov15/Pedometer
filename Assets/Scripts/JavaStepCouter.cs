using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JavaStepCouter : MonoBehaviour
{
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;
    public Text steps_text;

    // Start is called before the first frame update
    void Start()
    {
        InitializePlugin("com.example.pedometr.StepCounter");
        
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
        _pluginInstance.Call("OnResume");
        int steps = _pluginInstance.Get<int>("steps");
        steps_text.text = "number of steps" + steps;
    }
}
