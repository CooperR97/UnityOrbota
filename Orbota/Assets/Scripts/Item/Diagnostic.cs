using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diagnostic : MonoBehaviour
{
    public string Message;
    public string NameOfObject;
    public bool isTriggered = false;
    public GUIStyle guiStyle;
    public int xOffset;
    public int yOffset;
    private enum State { Start, Update }
    //private string InputText;
    private bool Functioning = false;
    private bool hasItem = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        InventoryHandler inventoryHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();

        if (col.gameObject.tag == "Player" && !Functioning) //If object is already functioning we do not need to display anything.
        {
            Debug.Log("Displaying Diagnostic Text");
            isTriggered = true;
            //InputText = "";

            //Check if user has correct item - Check inside the inventory handler
            if (inventoryHandler.ScanForItem(NameOfObject)) //Player has the correct item
            {
                hasItem = true;
            }
            else
            {
                hasItem = false;
            }
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Stopped Displaying Diagnostic Text");
            isTriggered = false;
            //InputText = "";

        }
    }


    private void OnGUI()
    {
        if (isTriggered)
        {
            Camera cam = GameObject.Find("Regular Camera").GetComponent<Camera>();
            var position = cam.WorldToScreenPoint(gameObject.transform.position);
            guiStyle.fontSize = 15;
            guiStyle.font = Resources.Load<Font>("8-BIT WONDER");
            guiStyle.normal.textColor = Color.green;
            guiStyle.fixedWidth = 300;
            var boxHeight = guiStyle.CalcHeight(new GUIContent(Message + "\n\n"), 300);
            guiStyle.wordWrap = true;
            guiStyle.normal.background = CreateTexture(300, (int)boxHeight, new Color(0, 0, 0));
            guiStyle.padding.left = 10;
            guiStyle.border.top = 80;
            if (!Functioning)
            {
                if (Time.time % 2 < 1)
                {
                    GUI.Label(new Rect(position.x + xOffset, (Screen.height - position.y) + yOffset, 300, boxHeight), Message + "\n\n>" + "USE [ R ] | CANCEL [ T ]", guiStyle);
                }
                else
                {
                    char square = '\u2588';
                    GUI.Label(new Rect(position.x + xOffset, (Screen.height - position.y) + yOffset, 300, boxHeight), Message + square + "\n\n>" + "USE [ R ] | CANCEL [ T ]", guiStyle);
                }
            }
            else
            {
                if (Time.time % 2 < 1)
                {
                    GUI.Label(new Rect(position.x + xOffset, (Screen.height - position.y) + yOffset, 300, boxHeight), Message, guiStyle);
                }
                else
                {
                    char square = '\u2588';
                    GUI.Label(new Rect(position.x + xOffset, (Screen.height - position.y) + yOffset, 300, boxHeight), Message + square, guiStyle);
                }
            }

        }

    }



    //Helper -> OnGUI 
    private Texture2D CreateTexture(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = color;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    
    }

    private void Update()
    {
        //Capture all input - in case we want to use actual commands...
        //if (Input.anyKey == true && isTriggered)
        //{
        //    foreach (char i in Input.inputString)
        //    {
        //        InputText = char.IsLetter(i) ? InputText + i : InputText;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.R) && hasItem == true)
        {
            InventoryHandler inventoryHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();

            //make functional and use item                                                                                   
            if (inventoryHandler.ScanForItem(NameOfObject))
            {
                //Grab the inventoryhandler for the gameobject and destroy the pickup from inside there.
                inventoryHandler.RemoveItem(NameOfObject);
                                                                                                                
            }                                                                                    
            else                                                                                 
            {                                                                                                                                                         
            }        
            
            StartCoroutine(Wait());
        }

        if (Input.GetKeyDown(KeyCode.T)) //CLOSE
        {
            isTriggered = false;
        }
    }

    public IEnumerator Wait()
    {
        Functioning = true;
        Message = "Applying Item.";
        yield return new WaitForSecondsRealtime(1);
        Message = "Applying Item..";
        yield return new WaitForSecondsRealtime(1);
        Message = "Applying Item...";
        yield return new WaitForSecondsRealtime(1);

        isTriggered = false;
    }
}
