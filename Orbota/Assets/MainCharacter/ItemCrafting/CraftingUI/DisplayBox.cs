using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayBox : MonoBehaviour
{

    protected Text myDetails;

    [SerializeField]
    protected Image myImage;

    // Start is called before the first frame update
    void Start()
    {
        myDetails = GetComponent<Text>();
        UpdateDisplay("Title", "Info", null);
    }

    public void UpdateDisplay(string title, string details, Sprite newSprite)
    {
        myDetails.text = title + "\n\n" + details;
        if(newSprite != null)
        {
            myImage.enabled = true;
            myImage.sprite = newSprite;
        }
        else
        {
            myImage.enabled = false;
        }
    }
}
