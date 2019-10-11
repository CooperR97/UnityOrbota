using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondoDoor : MonoBehaviour
{
    public bool branch1Required;
    public bool branch2Required;
    public bool basementRequired;
    public bool activeAfterRequirementsMet;

    private TriggerHandler triggerHandler;

    // Start is called before the first frame update
    void Start()
    {
        triggerHandler = FindObjectsOfType<TriggerHandler>()[0];
        if(branch1Required && triggerHandler.IsBranch1Complete()
            && branch2Required && triggerHandler.IsBranch2Complete() 
            && basementRequired && triggerHandler.IsBasementComplete())
        {
            gameObject.SetActive(activeAfterRequirementsMet);
        } else if(branch1Required && triggerHandler.IsBranch1Complete()
            && branch2Required && triggerHandler.IsBranch2Complete()
            && !basementRequired)
        {
            gameObject.SetActive(activeAfterRequirementsMet);
        }
        else if(branch1Required && triggerHandler.IsBranch1Complete() && !branch1Required && !basementRequired)
        {
            gameObject.SetActive(activeAfterRequirementsMet);
        }
    }
}
