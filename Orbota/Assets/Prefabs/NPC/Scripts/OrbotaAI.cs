using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;

/**
 * USE INSTRUCTIONS
 *      CREATING CUSTOM TEXT
 *          Simply go into the attached text objects in the Editor, and edit the text of each message
 *          The messages will appear in order 1->2->3->4->5
 *          There's currently not a way to add more than 5 messages
 *       
 *      REMOVING TEXT
 *          *If you want to REMOVE a message, make it BLANK, don't remove the text object
 *      
 *      CHANGING COLOR OF TEXT
 *          The textColor field in THIS script should be used, it will change all text colors
 *          You can change the individual text's colors but it won't do anything, because this script resets it
 *      
 *      CHANGING THE BACKGROUND
 *          In the canvas, there is a SpriteRenderer called Background that can be changed to the Sprite of your liking
 * 
 **/
public class OrbotaAI : MonoBehaviour
{
    public Vector2 textOffset;
    public Color textColor;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text nextText;
    public GameObject panel;

    private float textTimer;
    private int displaying = 1;
    private CharacterHorizontalMovement movement;
    private Character character;
    private OrbotaAIWalk aiWalk;
    private BoxCollider2D aiCollider;
    private float previousMovementSpeed;

    void Start()
    {
        movement = GetComponent<CharacterHorizontalMovement>();
        character = GetComponent<Character>();
        aiWalk = GetComponent<OrbotaAIWalk>();
        aiCollider = GetComponent<BoxCollider2D>();
    }

    private void showDialogue(Text text)
    {
        panel.SetActive(true);
        panel.transform.position = new Vector2(character.transform.position.x + textOffset.x + .1f, character.transform.position.y + textOffset.y - .3f);
        text.gameObject.SetActive(true);
        nextText.gameObject.SetActive(true);
        nextText.color = textColor;
        text.transform.position = new Vector2(character.transform.position.x + textOffset.x, character.transform.position.y + textOffset.y);
        text.color = textColor;
        nextText.transform.position = new Vector2(character.transform.position.x + textOffset.x + 1.7f, character.transform.position.y + textOffset.y - 1.2f);
    }

    private void hideDialogue()
    {
        text1.gameObject.SetActive(false);
        if (text2)
        {
            text2.gameObject.SetActive(false);
        }
        if (text3)
        {
            text3.gameObject.SetActive(false);
        }
        if (text4)
        {
            text4.gameObject.SetActive(false);
        }
        if (text5)
        {
            text5.gameObject.SetActive(false);
        }
        nextText.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        displaying = 1;
    }

    private void faceCharacter(Collider2D collisionObject)
    {
        //Face AI towards character
        if (collisionObject.gameObject.transform.position.x + .5f < transform.position.x && character.IsFacingRight)
        {
            //Set character moving the other way
            aiWalk.FlipAI();
        }
        if (collisionObject.gameObject.transform.position.x - .5f > transform.position.x && !character.IsFacingRight)
        {
            //Set character moving the other way
            aiWalk.FlipAI();
        }
    }


    void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player") && movement != null)
        {
            previousMovementSpeed = movement.MovementSpeed;
            faceCharacter(collisionObject);
            showDialogue(text1);

            //Stop character and save the previous movement speed to resume later
            movement.MovementSpeed = 0;
            aiWalk.SetEngagedWithPlayer(true);
        }
    }

    void OnTriggerStay2D(Collider2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player") && movement != null)
        {
            textTimer += Time.deltaTime;
            updateDialogue();
            if(collisionObject.transform.position.x < transform.position.x + (aiCollider.size.x/6)
                && collisionObject.transform.position.x > transform.position.x - (aiCollider.size.x / 6))
            faceCharacter(collisionObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player") && movement != null)
        {
            hideDialogue();
            //Face AI away from character
            if (collisionObject.gameObject.transform.position.x < transform.position.x && !character.IsFacingRight)
            {
                aiWalk.FlipAI();
            }
            if (collisionObject.gameObject.transform.position.x > transform.position.x && character.IsFacingRight)
            {
                aiWalk.FlipAI();
            }
            //Resume previous movement speed
            movement.MovementSpeed = 1;
            aiWalk.SetEngagedWithPlayer(false);
        }
    }

    private void updateDialogue()
    {
        //Get rid of nextText if there is no next text or no words in the next text
        if (displaying == 2 && (!text3 || !(text3.text.Length > 0))
            || displaying == 3 && (!text4 || !(text4.text.Length > 0))
            || displaying == 4 && (!text5 || !(text5.text.Length > 0))
            || displaying == 5)
        {
            nextText.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.M) && textTimer > .05f)
        {
            if (displaying == 1 && text2 && text2.text.Length > 0)
            {
                text1.gameObject.SetActive(false);
                showDialogue(text2);
                displaying = 2;
                textTimer = 0;
            } else if (displaying == 2 && text3 && text3.text.Length > 0)
            {
                text2.gameObject.SetActive(false);
                showDialogue(text3);
                displaying = 3;
                textTimer = 0;
            }
            else if (displaying == 3 && text4 && text4.text.Length > 0)
            {
                text3.gameObject.SetActive(false);
                showDialogue(text4);
                displaying = 4;
                textTimer = 0;
            }
            else if (displaying == 4 && text5 && text5.text.Length > 0)
            {
                text4.gameObject.SetActive(false);
                showDialogue(text5);
                displaying = 5;
                textTimer = 0;
            }
        }
    }
}
