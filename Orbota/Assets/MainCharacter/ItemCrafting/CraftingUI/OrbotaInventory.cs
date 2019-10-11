using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Model of the Inventory's MVC. Responsible for the storage and logic of items.
public class OrbotaInventory : MonoBehaviour
{
    [SerializeField]
    protected int DEFAULT_ITEM_COUNT = 10;
    protected int DEFAULT_ROW_COUNT = 2;

    protected struct ItemSlot
    {
        int ItemID;
        int ItemCount;

        public void setSlotID(int newID)
        {
            ItemID = newID;
        }

        public void setSlotCount(int value)
        {
            ItemCount = value;
        }


        public int getCount() {return ItemID; }

        public int getID() { return ItemCount; }

        public bool IsEmpty()
        {
            if (ItemCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    //Sets the number of slots in the array.
    protected int ItemSlotCount;
    //Keeps track of the latest slot adjusted.
    protected int LastAddedSlot = 0;
    //Reference to all slots.
    protected ItemSlot[] myItems;
    //Reference to a slot representing an empty slot.
    protected ItemSlot emptySlot;
    //Reference to the structure containing all item data.
    [SerializeField]
    protected ReferenceBank itemRef;

    //Looks for an open slot in the inventory, checking the last slot + 1. Returns the "empty" slot if full.
    protected ItemSlot GetOpenSlot()
    {
        if(myItems[LastAddedSlot].IsEmpty())
        {
            return myItems[LastAddedSlot];
        }
        if (LastAddedSlot + 1 > myItems.Length)
        {
            if (myItems[LastAddedSlot + 1].IsEmpty())
            {
                LastAddedSlot = LastAddedSlot + 1;
                return myItems[LastAddedSlot];
            }
        }
        else
        {
            for (int i = 0; i < myItems.Length; i++)
            {
                if (myItems[i].IsEmpty())
                {
                    LastAddedSlot = i;
                    return myItems[i];
                }
            }
        }
        Debug.Log("Empty Slot returned.");
        return emptySlot;
    }

    //Returns the value of the last added slot.
    public int getLastAdded() { return LastAddedSlot; }

    //Attempts to add an item to the inventory, returning true if a space is available.
    public bool addItem(int itemID, int count)
    {
        //foreach(ItemSlot slot in myItems) {Debug.Log("Slot " + slot.ToString() + " is " + slot.IsEmpty());}

        bool result;
        if (GetOpenSlot().getID() > -1)
        {
            myItems[LastAddedSlot].setSlotID(itemID);
            myItems[LastAddedSlot].setSlotCount(count);
            result = true;
        }
        else { result = false; }
        //Debug.Log("OrbotaInventory is " + result);
        return result;
    }

    //Called from Inventory UI or other components to remove an Item, returning true if successfuly removed.
    public bool removeItem(int index)
    {
        if(index < ItemSlotCount)
        {
            myItems[index].setSlotID(-1);
            myItems[index].setSlotCount(0);
            return true;
        }
        return false;
        
    }

    //Called from Inventory UI to get the data needed for UI elements.
    public OrbotaItem ExtractItemInfo(int currentSlot)
    {
        if (currentSlot > -1 && currentSlot < myItems.Length)
        {
            int itemID = myItems[currentSlot].getID();
            if (!itemRef.Equals(null))
            {
                return (OrbotaItem)itemRef.getReference(itemID);
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    //Called from Inventory UI to shift an item from one slot to another.
    public void moveItem(int index1, int index2)
    {
        emptySlot.setSlotID(myItems[index1].getID());
        emptySlot.setSlotCount(myItems[index1].getCount());

    }

    //Called from Inventory UI to switch the locations of two items.
    public void swapItems(int index1, int index2)
    {
        //Store the first index in the empty slot
        emptySlot.setSlotID(myItems[index1].getID());
        emptySlot.setSlotCount(myItems[index1].getCount());

        //Store the second index values in index 1
        myItems[index1].setSlotID(myItems[index2].getID());
        myItems[index1].setSlotCount(myItems[index2].getCount());

        //Store the first index values from empty slot into index 2
        myItems[index2].setSlotID(emptySlot.getID());
        myItems[index2].setSlotID(emptySlot.getID());
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemSlotCount = DEFAULT_ITEM_COUNT;
        myItems = new ItemSlot[ItemSlotCount];

        //Create valid item slots.
        for (int i = 0; i < ItemSlotCount; i++)
        {
            myItems[i] = new ItemSlot();
        }
        //Create the empty item slot.
        emptySlot = new ItemSlot();
        emptySlot.setSlotCount(-1);

        if(itemRef == null)
        {
            Debug.Log("Add an item reference in the editor! Orbota Inventory will not function without it.");
            FindObjectOfType<InventoryUI>().enabled = false;
            enabled = false;
        }
    }
}
