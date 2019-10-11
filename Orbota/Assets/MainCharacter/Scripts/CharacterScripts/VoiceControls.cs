using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControlsPause : CharacterPause
{
    public static void VoiceTriggerPause()
    {
        VoiceControlsPause pause = new VoiceControlsPause();
        pause.TriggerPause();
    }
}

public class VoiceControls : CharacterHorizontalMovement
{
    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    //public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    protected string word;
    
    public enum movementState { left, right, still }

    private movementState moveState;

    CharacterJump characterJump;
    CharacterDash characterDash;
    CharacterHandleWeapon characterHandleWeapon;
    CharacterPause characterPause;
    CharacterHorizontalMovement characterHorizontalMovement;
    CharacterGravity characterGravity;
    CharacterButtonActivation characterButtonActivation;

    int pauseState = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Initialization();
        InitializeAnimatorParameters();

        moveState = movementState.still;
        characterJump = GetComponent<CharacterJump>();
        characterDash = GetComponent<CharacterDash>(); 
        characterPause = GetComponent<CharacterPause>();
        characterHandleWeapon = GetComponent<CharacterHandleWeapon>();
        characterHorizontalMovement = GetComponent<CharacterHorizontalMovement>();
        characterGravity = GetComponent<CharacterGravity>();
        characterButtonActivation = GetComponent<CharacterButtonActivation>();

        actions.Add("jump", Jump);
        actions.Add("enter", Enter);
        actions.Add("break", Pause);
        actions.Add("unbreak", UnPause);
        actions.Add("start", Start);
        actions.Add("dash", Dash);
        actions.Add("fire", Shoot);
        actions.Add("left", MoveLeft);
        actions.Add("right", MoveRight);
        actions.Add("stop", Stop);

        recognizer = new KeywordRecognizer(actions.Keys.ToArray());
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;

        //if user selects 'VOICE RECOGNITION'
        //{
        recognizer.Start();
        //}
    }

    private void Update()   //Test that firing works.
    {

        switch (moveState)
        {
            case movementState.left:
                if (!characterGravity.ShouldReverseInput())
                {
                    SetHorizontalMove(-3f);
                }
                else
                {
                    SetHorizontalMove(3f);
                }
                HandleHorizontalMovement();
                break;
            case movementState.right:
                if (!characterGravity.ShouldReverseInput())
                {
                    SetHorizontalMove(3f);
                }
                else
                {
                    SetHorizontalMove(-3f);
                }
                HandleHorizontalMovement();
                break;
            case movementState.still:
                break;

           
            default:
                break;
        }

    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log(args.text);
        actions[args.text].Invoke();
    }

    private void Shoot()
    {
        if (characterHandleWeapon != null)
        {
            characterHandleWeapon.ShootStart();
            
        }

    }

    private void Jump()
    {
        Debug.Log("jump");
        if (characterJump != null)
        {
            characterJump.JumpStart();
        } 
    }

    private void Pause()
    {
        Debug.Log("pause");
        if (characterPause != null)
        {
            VoiceControlsPause.VoiceTriggerPause();
            pauseState = 1; //Game is now Paused
        }
    }

    private void UnPause()
    {
        Debug.Log("unpause");
        if (characterPause != null && pauseState == 1)
        {
            VoiceControlsPause.VoiceTriggerPause();
            pauseState = 0; //Game is now Unpaused
        }
    }

    private void Dash()
    {
        Debug.Log("dash");
        if (characterDash != null)
        {
            characterDash.StartDash();
        }
    }
    

    private void MoveLeft()
    {
        _movement.ChangeState(CharacterStates.MovementStates.Walking);
        moveState = movementState.left;
    }

    private void MoveRight()
    {
        _movement.ChangeState(CharacterStates.MovementStates.Walking);
        moveState = movementState.right;
    }

    private void Stop()
    {
        moveState = movementState.still;
        SetHorizontalMove(0);
        HandleHorizontalMovement();
        _movement.ChangeState(CharacterStates.MovementStates.Walking);
    }

    private void Enter()
    {
        Debug.Log("Enter");
        if (characterButtonActivation.ButtonActivatedZone != null)
        {
            characterButtonActivation.ButtonActivatedZone.TriggerButtonAction();
        }
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
