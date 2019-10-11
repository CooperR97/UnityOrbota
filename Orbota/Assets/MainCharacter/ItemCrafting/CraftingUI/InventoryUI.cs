using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;



//View of the Inventory's MVC. Responsible for displaying the information for 
public class InventoryUI : MonoBehaviour
{
    //Reference to the scriptable object driving the data displayed by the UI.
    [SerializeField]
    protected ReferenceBank itemBank;

    //Method called when things are annoying and need to be fucking fixed goddamn it all to hell.
    public ReferenceBank getItemBank() { return itemBank; }

    //Reference to the character object this inventory belongs to. Should be assigned to by the level manager, and will not activate without it.
    protected MoreMountains.CorgiEngine.Character myCharacter;

    //Reference to the handler of the character, only accessed once the character is confirmed to exist.
    protected InventoryHandler myHandler;

    //A prefab of a gameObject for containing the details of an item.
    [SerializeField]
    protected DisplayBox myDetails;

    [SerializeField]
    protected DummyControlUnit[] controlUnits;

    protected static int EQUIPPED = 0;
    protected static int INVENTORY = 1;
    protected static int CRAFTING = 2;
    protected static int DROPBOX = 3;

    //Method called to turn on the Inventory's detail box.
    public void UpdateDetailBox(OrbotaItem information)
    {
        myDetails.UpdateDisplay(information.getName(), information.getDescription(), information.getSprite());
    }



    //Method called to start showing the Inventory's UI.
    public void DisplayInventoryUI()
    {
        transform.parent.GetComponent<Canvas>().enabled = true;
    }

    //Method called to hid the Inventory's UI.
    public void HideInventoryUI()
    {
        transform.parent.GetComponent<Canvas>().enabled = false;
    }

    //Method called from InventoryHandler to process adding new items.
    public bool addNewInventoryItem(int newItem)
    {
        if (controlUnits[INVENTORY].isFull() == false)
        {
            GameObject temp = Instantiate(new GameObject(), controlUnits[INVENTORY].gameObject.transform);
            temp.AddComponent<DragAndDropItem>();

            DragAndDropItem newDragItem = temp.GetComponent<DragAndDropItem>();

            if (controlUnits[INVENTORY].AddItemInFreeCell(newDragItem))
            {
                newDragItem.UpdateItemInfo(newItem, itemBank);
                return true;
            }
            else
            {
                Destroy(temp.gameObject);
                return false;
            }
        }
        return false;
    }
    
    //Check if user has the item needed.
    public bool FindInventoryItem(string item)
    {
        bool result = false;
        foreach (DragAndDropCell cell in controlUnits[INVENTORY].getMyCells())
        {

            if (cell.GetItem() != null)
            {
                if (cell.GetItem().ToString() == item + " (DragAndDropItem)")
                {
                    return result = true;
                }
            }
            else
            {
                break;
            }

        }
        foreach (DragAndDropCell cell in controlUnits[EQUIPPED].getMyCells())
        {

            if (cell.GetItem() != null)
            {
                if (cell.GetItem().ToString() == item + " (DragAndDropItem)")
                {
                    return result = true;
                }
            }
            else
            {
                break;
            }

            
        }

        return result;
    }

    //Find the needed item and remove it.
    public void RemoveItem(string item)
    {
        foreach (DragAndDropCell cell in controlUnits[INVENTORY].getMyCells())
        {
            //we need a check here - if cell has no item in it then we STOP.
            if (cell.GetItem() != null)
            {
                if (cell.GetItem().ToString() == item + " (DragAndDropItem)")
                {
                    cell.RemoveItem();
                    break;
                }
            }
            else
            {
                break;
            }

        }
        foreach (DragAndDropCell cell in controlUnits[EQUIPPED].getMyCells())
        {
            //we need a check here - if cell has no item in it then we STOP.

            if (cell.GetItem() != null)
            {
                if (cell.GetItem().ToString() == item + " (DragAndDropItem)")
                {
                    cell.RemoveItem();
                    break;
                }
            }
            else
            {
                break;
            }

        }
    }
    

    private void OnMouseUp()
    {
        
    }

    private void Update()
    {
        
    }

    //Method called to automatically shift items from the crafting unit to the inventory unit
    public void moveCraftingItemsToInventory()
    {
        controlUnits[CRAFTING].MoveItemsToUnit(controlUnits[INVENTORY], this);
    }

    //Method called when an item needs to be moved easily to the inventory slot
    public DummyControlUnit getInventoryReference() { return controlUnits[INVENTORY]; }

    private void Start()
    {
        myCharacter = FindObjectOfType<LevelManager>().Players[0];
        if (myCharacter != null)
        {
            myHandler = myCharacter.GetComponent<InventoryHandler>();
            if(myHandler != null)
            {
                myHandler.RegisterUI(this);
            }
        }
        transform.parent.GetComponent<Canvas>().enabled = false;
    }

    //Method called upon toggling the inventory UI to ensure all visual data is up-to-date.
    public void updateAllItemDisplays()
    {
        foreach(DummyControlUnit unit in controlUnits)
        {
            foreach(DragAndDropCell cell in unit.getMyCells())
            {
                cell.UpdateItemData(this);
            }
        }
    }

    //Method called to drop an item from the inventory
    public bool dropItemInWorld(OrbotaItem itemDropped)
    {
        //Debug.Log(itemDropped.name + " removed from inventory via running outta dang space, yall.");
        GameObject newItem = Instantiate(new GameObject(), myCharacter.transform);
        newItem.transform.position = new Vector2(newItem.transform.position.x, newItem.transform.position.y + 1f);
        PickupItem newThing = newItem.AddComponent<PickupItem>();
        newThing.setupItem(itemDropped.getItemID(), itemDropped.getSprite());
        newItem.transform.parent = null;
        return true;
    }

    public bool updateInventoryHandler(DragAndDropCell[] newItems)
    {
        return myHandler.checkItemAbilities(newItems);
    }

    
}



