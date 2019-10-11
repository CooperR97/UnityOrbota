using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller of the Inventory System's MVC. Responsible for adding items to a character's inventory from external sources.
[RequireComponent(typeof(MoreMountains.CorgiEngine.CharacterHandleWeapon))]
[RequireComponent(typeof(MoreMountains.CorgiEngine.CharacterHandleSecondaryWeapon))]
public class InventoryHandler : MoreMountains.CorgiEngine.CharacterAbility
{
    //Used to toggle an active display.
    protected bool DisplayOn;
    //Reference to the UI this component manages, is assigned to
    protected InventoryUI myInventoryUI;

    //Reference to a gameobject that contains a collider for holding items.
    protected GameObject GrabSpot;

    //A small collider held in front of the character for picking up items.
    protected BoxCollider2D GrabCollider;

    //A Vector3 for holding the values of a new position.
    protected Vector3 tempVector = new Vector3(0,0,0);

    //A bool for detecting if the character has changed direction
    protected bool hasChanged;

    //Value for controlling how far in front the grab box is.
    protected float changeValue = 10;

    //Variable holding the current setup of abilities for this handler.
    protected OrbotaItem[] currentAbilities;

    //Variable for tracking if the slots of a new 
    protected bool[] conflictCheck;

    //Variable for a reference to the left weapon of the character
    [SerializeField]
    protected MoreMountains.CorgiEngine.CharacterHandleWeapon myLeftWeapon;

    //Variable for a reference to the right weapon of the character
    [SerializeField]
    protected MoreMountains.CorgiEngine.CharacterHandleSecondaryWeapon myRightWeapon;

    [SerializeField]
    protected MoreMountains.CorgiEngine.OrbotaMelee defaultWeapon;

    //Reference to the left weapon equiped by the character
    [SerializeField]
    protected MoreMountains.CorgiEngine.Weapon leftWeapon;

    //Reference to the right weapon equiped by the character
    [SerializeField]
    protected MoreMountains.CorgiEngine.Weapon rightWeapon;

    //Method for connecting Item pickup & drops within the game environment.
    public bool RegisterUI(InventoryUI newUI)
    {
        myInventoryUI = newUI;
        if(!myInventoryUI.Equals(null))
        { return true; }
        return false;
    }

    //Method for checking if player has item needed.
    public bool ScanForItem(string item)
    {
        return myInventoryUI.FindInventoryItem(item);
    }

    //Method for removing an item after it has been used.
    public void RemoveItem(string item)
    {
        myInventoryUI.RemoveItem(item);

        return;
    }

    protected override void Initialization()
    {
        base.Initialization();
        GrabSpot = Instantiate(new GameObject(), null, true);
        GrabCollider = GrabSpot.AddComponent<BoxCollider2D>();
        GrabSpot.transform.parent = gameObject.transform;
        GrabSpot.transform.position = transform.position;
        currentAbilities = new OrbotaItem[2];
        leftWeapon = myLeftWeapon.CurrentWeapon;
        rightWeapon = myRightWeapon.CurrentWeapon;
        defaultWeapon.updateActiveMelee(leftWeapon);
        defaultWeapon.updateActiveMelee(rightWeapon);
    }


    protected override void HandleInput()
    {
        if (_inputManager.InventoryButton.State.CurrentState == MoreMountains.Tools.MMInput.ButtonStates.ButtonDown && !DisplayOn)
        {
            myInventoryUI.DisplayInventoryUI();
            myInventoryUI.updateAllItemDisplays();
            DisplayOn = true;
            Time.timeScale = 0;
            _character.Freeze();
        }
        else if(_inputManager.InventoryButton.State.CurrentState == MoreMountains.Tools.MMInput.ButtonStates.ButtonDown
                 || _inputManager.PauseButton.State.CurrentState == MoreMountains.Tools.MMInput.ButtonStates.ButtonDown)
        {
            myInventoryUI.HideInventoryUI();
            DisplayOn = false;
            Time.timeScale = 1;
            myInventoryUI.moveCraftingItemsToInventory();
            _character.UnFreeze();
        }
    }

    //Method called to add an item to the inventory.
    public bool pickupItem(int itemID)
    {
        return myInventoryUI.addNewInventoryItem(itemID);
    }


    //
    // Methods below are to be worked on!
    //


    //Method called to load a saved array of equipped items from file.
    public bool loadSavedEquiped(int[] inventoryItems)
    {
        return true;
    }

    //Method called to save an array of equipped items to file.
    public int[] saveEquiped()
    {
        return new int[3];
    }

    //Method called to load a saved array of inventory items to file.
    public bool loadSavedInventory(int[] inventoryItems)
    {
        return true;
    }

    //Method called to save an array of inventory items to file.
    public int[] saveInventory()
    {
        return new int[3];
    }

    //Method called to check if a new item 
    public bool checkItemAbilities(DragAndDropCell[] refItem)   //MAJOR ALGORITHM MISHAP HERE, NEEDS FIXING, BUT OTHERWISE SEEMS TO WORK(ISH).
    {
        bool result = true;
        bool slotEmpty = false;
        for(int i = 0; i < currentAbilities.Length; i++) //Checking each slot of the equiped items
        {
            //Check if the current slot is empty
            if(currentAbilities[i] == null)  { slotEmpty = true; }

            //Run through the current cells of the equipment controller
            for(int j = i; j < refItem.Length; j++)
            {
                //If this control cell has an item
                if(refItem[j].GetItem() != null)
                {
                    //And the handler slot is empty
                    if(slotEmpty)
                    {
                        //Set this slot to fill be filled by the new item.
                        currentAbilities[i] = refItem[j].GetItem().getItemReference();

                        //If the left slot is the current slot
                        if (i == 0)
                        {
                            applyAbilities(currentAbilities[i].getItemAbilities(),true); //generate abilities for the left inputs
                        }
                        else //If the right slit is the current slot
                        {
                            applyAbilities(currentAbilities[i].getItemAbilities(),false); //generate abilities for the right inputs
                        }
                    }
                    else //If the left slot is not empty
                    {
                        if (i == 0) //If the left slot is not empty
                        {
                            removeAbilities(true); //remove abilities for the left inputs
                            applyAbilities(currentAbilities[i].getItemAbilities(), true); //and add the new abilities to the left input
                        }
                        else //If the right slot is not current slot
                        {
                            removeAbilities(false); //remove abilities for the right inputs
                            applyAbilities(currentAbilities[i].getItemAbilities(), false); //and add the new abilities to the right input
                        }
                    }
                }
                else //If this control slot is empty
                {
                    if (i == 0) //If the left slot the newly empty slot
                    {
                        removeAbilities(true); //remove abilities for the left inputs
                    }
                    else //If the right slot is the newly empty slot
                    {
                        removeAbilities(false); //remove abilities for the right inputs
                    }
                }
            }
        }



        return result;
    }

    //Method called for applying abilities that will be activated by LMB
    protected bool applyAbilities(MoreMountains.CorgiEngine.OrbotaWeapon newAbilities, bool side)
    {
        bool result = true;
        if (newAbilities != null)
        {
            if (side)
            {
                newAbilities.UpdateActiveWeapon(leftWeapon);
                Debug.Log(newAbilities.name + " is equipped.");
            }
            else
            {
                newAbilities.UpdateActiveWeapon(rightWeapon);
                Debug.Log(newAbilities.name + " is equipped.");
            }
           
        }
        return result;
    }

    //Method called for removing abilities from a given mouse input.
    protected bool removeAbilities(bool side)
    {
        bool result = true;

        if (side == true && myLeftWeapon == null)  { return result; }

        else if(side == false && myRightWeapon == null) { return result;  }

        if (side)
        {
            defaultWeapon.updateActiveMelee((leftWeapon));
        }
        else
        {
            defaultWeapon.updateActiveMelee(rightWeapon);
        }
        
        return result;
    }
}
