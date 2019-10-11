using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerHandler : MonoBehaviour
{
    private bool branch1Complete;
    private bool powerhouseCompleted;
    private bool branch2Complete;
    private bool basementVisited;
    private bool basementComplete;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += UpdateTriggers;
    }

    void UpdateTriggers(Scene scene, LoadSceneMode mode)
    {
        //On powerhouse
        if(scene.name.ToLower().Contains("power"))
        {
            branch1Complete = true;
        }

        //On branch2
        if (scene.name.ToLower().Contains("branch2"))
        {
            powerhouseCompleted = true;
        }

        //On return to hub, after branch2
        if(scene.name.ToLower().Contains("hub") && branch1Complete)
        {
            branch2Complete = true;
        }

        //On visit to basement
        if(scene.name.ToLower().Contains("basement"))
        {
            basementVisited = true;
        }

        if(scene.name.ToLower().Contains("hub") && basementVisited)
        {
            basementComplete = true;
        }

        Debug.Log("branch1: " + branch1Complete);
        Debug.Log("branch2: " + branch2Complete);
        Debug.Log("basement: " + basementComplete);
    }

    public bool IsBasementComplete()
    {
        return basementComplete;
    }

    public bool IsBranch1Complete()
    {
        return branch1Complete;
    }

    public bool IsBranch2Complete()
    {
        return branch1Complete;
    }
}
