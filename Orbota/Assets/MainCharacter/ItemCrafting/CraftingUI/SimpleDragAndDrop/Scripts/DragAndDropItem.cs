using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Drag and Drop item.
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(BoxCollider2D))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;                                      // Item that is dragged now
	public static GameObject icon;                                                  // Icon of dragged item
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 100;										// Sort order for canvas

    //Reference to the scriptable object holding the data values.
    [SerializeField]
    protected OrbotaItem itemReference;

    //Simple integer referencing the index value of this item.
    [SerializeField]
    protected int myItemID;

    protected InventoryUI terriblePracticeUIReference;

    //Reference to the cell holding this item.
    protected DragAndDropCell currentCell;

    //Method for retrieving this item's SO reference.
    public OrbotaItem getItemReference()
    {
        return itemReference;
    }

    //Fool's folly.
    public void UpdateItemPlain(InventoryUI bankRef)
    {
        if (itemReference == null)
        {
            itemReference = (OrbotaItem)bankRef.getItemBank().getReference(myItemID);
        }

        name = itemReference.getName();

        Image imag = GetComponent<Image>();
        imag.sprite = itemReference.getSprite();
    }

    //A method for updating the visual display of an item.
    public bool UpdateItemInfo(int newItem, ReferenceBank newBank)
    {
        myItemID = newItem;
        if (icon != null)
        {
            itemReference = (OrbotaItem)newBank.getReference(myItemID);
            if (itemReference != null)
            {

                name = itemReference.getName();


                Image imag = icon.GetComponent<Image>();
                if (imag != null)
                {
                    //Debug.Log("why");
                    imag.sprite = itemReference.getSprite();
                }
                return true;
            }
        }
        return false;
    }

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
            icon = gameObject;
        }
        terriblePracticeUIReference = GetComponentInParent<InventoryUI>();

    }

    private void Start()
    {
        if (itemReference != null)
        {
            name = itemReference.getName();
            Debug.Log(name);
            icon = gameObject;
            Image imag = icon.GetComponent<Image>();
            if (imag != null)
            {
                imag.sprite = itemReference.getSprite();
            }
        }
        
    }


    /// <summary>
    /// This item started to drag.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
	{
        if (dragDisabled == false)
        {
            sourceCell = GetCell();                                                 // Remember source cell
            draggedItem = this;                                                     // Set as dragged item
                                                                                    // Create item's icon
            icon = new GameObject();
            icon.transform.SetParent(canvas.transform);
            icon.name = "Icon";
            Image myImage = GetComponent<Image>();
            //Debug.Log(itemReference.name);
            myImage.sprite = itemReference.getSprite();
            myImage.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
            myImage.color = new Color(myImage.color.r, myImage.color.b, myImage.color.g, 0.5f);
            Image iconImage = icon.AddComponent<Image>();
            iconImage.raycastTarget = false;
            iconImage.sprite = myImage.sprite;
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            // Set icon's dimensions
            RectTransform myRect = GetComponent<RectTransform>();
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this);                                         // Notify all items about drag start for raycast disabling
            }

            
        }
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (icon != null)
		{
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		ResetConditions();
	}

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	private void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;
        Image myImage = GetComponent<Image>();

        if (myImage != null)
        {
            myImage.color = new Color(myImage.color.r, myImage.color.b, myImage.color.g, 1f);
            myImage.sprite = itemReference.getSprite();
        }
    }

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		Image image = GetComponent<Image>();
		if (image != null)
		{
			image.raycastTarget = condition;
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("pain");
        //display item name
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("agony");
        //clear item name
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("my hatred burns through the cavernous deeps");
        terriblePracticeUIReference.UpdateDetailBox(getItemReference());
    }
}
