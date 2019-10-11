using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Example of control application for drag and drop events handle
/// </summary>
public class DummyControlUnit : MonoBehaviour
{
    [SerializeField]
    protected bool isEquipedUnit;

    [SerializeField]
    protected bool isCraftingUnit;

    [SerializeField]
    protected bool isDropUnit;

    [SerializeField]
    protected Button craftButton;

    [SerializeField]
    protected Button toggleLeft;

    [SerializeField]
    protected Button toggleRight;

    [SerializeField]
    protected Image itemDisplay;

    protected OrbotaItem tempItem;

    protected ReferenceBank itemRefs;

    protected int tempItemIndex;

    protected bool canCraftItem = false;

    protected bool[] craftingSolution;

    protected bool[] craftingItemRefs;

    static int INVENTORY_COUNT = 8;
    static int EQUIPMENT_COUNT = 2;

    protected InventoryUI myUIReference;


    //An array of cells 
    [SerializeField]
    protected DragAndDropCell[] myCells;

    //Method called to retrive references to this unit's DaD cells.
    public DragAndDropCell[] getMyCells() { return myCells; }

    //Number of items held in this unit.
    protected int itemCount = 0;

    //Integer representing the unique ID number of this unit.
    [SerializeField]
    protected int unitID;

    private void Start()
    {
        Initialize();
        
    }

    /// <summary>
    /// Called on start to detect all cells within this unity and place it in an array.
    /// </summary>
    protected bool Initialize()
    {
        if(isEquipedUnit)
        {
            myCells = new DragAndDropCell[EQUIPMENT_COUNT];
        }
        else if(isDropUnit)
        {
            myCells = new DragAndDropCell[1];
        }
        else
        {
            myCells = new DragAndDropCell[INVENTORY_COUNT];
        }
        bool result = false;
        int count = 0;
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (count < myCells.Length)
            {
                //Debug.Log(cell.name);
                myCells[count] = cell;
                cell.setControlUnit(this);
                result = true;
                count++;
            }
            else
            {
                result = false;
                Debug.Log("Unit " + name + " is not properly set up");
            } 
        }
        unitID = name.GetHashCode()%100;

        myUIReference = GetComponentInParent<InventoryUI>();
        if(myUIReference == null)
        {
            Debug.Log("Things are really messed up here.");
        }
        if(isCraftingUnit)
        {
            itemRefs = FindObjectOfType<InventoryUI>().getItemBank();
            tempItemIndex = 0;
            tempItem = (OrbotaItem)itemRefs.getReference(tempItemIndex);
            itemDisplay.sprite = tempItem.getSprite();
            itemDisplay.color = Color.gray;
            craftingItemRefs = new bool[myCells.Length];
            resetCraftingRefs();
            AdjustCraftingDisplay();
        }



        return result;
    }

    //Method called to reset the values of items that can be used for crafting.
    protected void resetCraftingRefs()
    {
        if (isCraftingUnit)
        {
            for (int i = 0; i < craftingItemRefs.Length; i++)
            {
                Debug.Log(name);
                craftingItemRefs[i] = false;
            }
        }

    }

    /// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        DummyControlUnit sourceSheet = desc.sourceCell.GetComponentInParent<DummyControlUnit>();
        // Get control unit of destination cell
        DummyControlUnit destinationSheet = desc.destinationCell.GetComponentInParent<DummyControlUnit>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                //Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    //Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    if (isCraftingUnit)
                    {
                        
                    }
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    //Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);

                }
                break;
            case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                //Debug.Log("Item " + desc.item.name + " added into " + destinationSheet.name);
                
                break;
            case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                //Debug.Log("Item " + desc.item.name + " will be destroyed from " + sourceSheet.name);
                break;
            default:
                //Debug.Log("Unknown drag and drop event");
                break;
        }
    }

    

    /// <summary>
    /// Add item in first free cell
    /// </summary>
    /// <param name="item"> new item </param>
    public bool AddItemInFreeCell(DragAndDropItem item)
    {
        bool result = false;
        foreach (DragAndDropCell cell in myCells)
        {
            if (cell != null)
            {
				if (cell.GetItem() == null)
                {
                    if (isDropUnit)
                    {
                        InventoryUI inventoryRef = GetComponentInParent<InventoryUI>();
                        inventoryRef.dropItemInWorld(item.getItemReference());
                        break;
                    }
                    Debug.Log(item.name);
                    cell.AddItem(item);
                    result = true;
                    itemCount++;
                    if(isEquipedUnit)
                    {
                        
                    }
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Removes an item from first not empty cell
    /// </summary>
    public void RemoveFirstItem()
    {
        foreach (DragAndDropCell cell in myCells)
        {
            if (cell != null)
            {
				if (cell.GetItem() != null)
                {
                    cell.RemoveItem();
                    itemCount--;
                    break;
                }
            }
        }
    }

    //Method called to shift items from one control unit to another.
    public void MoveItemsToUnit(DummyControlUnit newUnit, InventoryUI targetUI)
    {
        foreach (DragAndDropCell cell in myCells)
        {
            if (cell != null)
            {
                if (cell.GetItem() != null)
                {
                    if (!newUnit.AddItemInFreeCell(cell.GetItem()))
                    {
                        targetUI.dropItemInWorld(cell.GetItem().getItemReference());
                    }
                    cell.RemoveItem();
                    itemCount--;
                }
            }
        }
    }


    //Method called via button to look at a new item. Automatically checks if the new item ref can be crafted with the present items.
    public void toggleCraftingLeft()
    {
        if(tempItemIndex > 0)
        {
            tempItemIndex--;
        }
        else
        {
            tempItemIndex = itemRefs.getReferences().Length-1;
        }
        tempItem = (OrbotaItem)itemRefs.getReference(tempItemIndex);
        craftingSolution = new bool[tempItem.getItemCraftingValues().Length];
        canCraftItem = checkMatchCraftingComponent();
        AdjustCraftingDisplay();
    }

    //Method called via button to look at a new item. Automatically checks if the new item ref can be crafted with the present items.
    public void toggleCraftingRight()
    {
        if (tempItemIndex < itemRefs.getReferences().Length-1)
        {
            tempItemIndex++;
        }
        else
        {
            tempItemIndex = 0;
        }
        tempItem = (OrbotaItem)itemRefs.getReference(tempItemIndex);
        craftingSolution = new bool[tempItem.getItemCraftingValues().Length];
        for (int i = 0; i < craftingSolution.Length; i++)
        {
            craftingSolution[i] = false;
        }
        canCraftItem = checkMatchCraftingComponent();
        AdjustCraftingDisplay();
    }

    //Method called via button. Used to eliminate old items and 
    public void craftCurrentItem()
    {
        if(canCraftItem)
        {
            foreach(DragAndDropCell cell in myCells)
            {
                if (cell.enabled == true)
                {
                    cell.RemoveItem();
                }
            }
        }
        GameObject newItem = Instantiate(new GameObject(), transform);
        DragAndDropItem temp = newItem.AddComponent<DragAndDropItem>();
        temp.UpdateItemInfo(tempItem.getItemID(), itemRefs);
        temp.UpdateItemPlain(FindObjectOfType<InventoryUI>());
        myUIReference.getInventoryReference().AddItemInFreeCell(temp);
        canCraftItem = checkMatchCraftingComponent();
        canCraftItem = false;
        AdjustCraftingDisplay();
        
    }

    //Method called to check if the items in this crafting unit match
    protected bool checkMatchCraftingComponent()
    {   
        if (isCraftingUnit)
        {
            int count = 0;
            int subCount = 0;
            resetCraftingRefs();
            foreach (int item in tempItem.getItemCraftingValues())
            {
                subCount = 0;
                foreach (DragAndDropCell cell in myCells)
                {
                    DragAndDropItem tempItem = cell.GetItem();
                    if (tempItem != null)
                    {
                        if (item == tempItem.getItemReference().getItemID())
                        {
                            if (craftingItemRefs[subCount] == false)
                            {
                                craftingSolution[subCount] = true;
                                craftingItemRefs[count] = true;
                            }
                        }
                    }
                    subCount++;
                }
                if (craftingSolution[count] == false) { break; }
                count++;
            }

            bool result = true;
            foreach (bool val in craftingSolution)
            {
                if (val == false)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
        return true;
    }

    protected void AdjustCraftingDisplay()
    {
        itemDisplay.sprite = tempItem.getSprite();
        int count = 0;
        Image temp;
        OrbotaItem subTemp;
        foreach(DragAndDropCell cell in myCells)
        {
            if(count < tempItem.getItemCraftingValues().Length)
            {
                temp = cell.GetComponent<Image>();
                if(temp != null)
                {
                    cell.gameObject.SetActive(true);
                    subTemp = (OrbotaItem)itemRefs.getReference(tempItem.getItemCraftingValues()[count]);
                    temp.sprite = subTemp.getSprite();
                    temp.color = Color.grey;
                }
            }
            else
            {
                cell.gameObject.SetActive(false);
                if(cell.GetItem() != null)
                {
                    MoveItemsToUnit(myUIReference.getInventoryReference(), myUIReference);
                    cell.RemoveItem();
                }
            }
            count++;
        }

        if (canCraftItem == true && tempItem.getItemCraftingValues().Length > 0)
        {
            craftButton.gameObject.SetActive(true);
        }
        else
        {
            craftButton.gameObject.SetActive(false);
        }
    }

    //Method for removing all items from a unit.
    public void ClearItems()
    {
        foreach (DragAndDropCell cell in myCells)
        {
            if (cell != null)
            {
                if (cell.GetItem() != null)
                {
                    cell.RemoveItem();
                    itemCount--;
                }
            }
        }
    }
    
    //Method for checking if a unit is filled with items.
    public bool isFull()
    {
        bool result = true;
        foreach (DragAndDropCell cell in myCells)
        {
            if (cell != null)
            {
                if (cell.GetItem() == null)
                {
                    result = false;
                    break;
                }
            }
        }
        return result;
    }

    //Method called from activated cells to call special functions based on these items.
    public bool checkItem(OrbotaItem itemCheck, DragAndDropCell cellReference)
    {
        bool result = true;

        if (isCraftingUnit)
        {
            canCraftItem = checkMatchCraftingComponent();
            AdjustCraftingDisplay();
        }

        if (!isDropUnit && !isEquipedUnit)
        {
            return result;
        }
        if(isDropUnit)
        {
            result = myUIReference.dropItemInWorld(itemCheck);
            cellReference.RemoveItem();
            Debug.Log(itemCheck.name + " should be dropped."); 
        }
        else if(isEquipedUnit)
        {
            result = myUIReference.updateInventoryHandler(myCells);
            Debug.Log(itemCheck.name + " equipped.");
        }
        

        return result;
    }
}
