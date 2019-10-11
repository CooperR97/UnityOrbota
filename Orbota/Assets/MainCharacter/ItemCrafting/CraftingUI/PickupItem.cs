using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour
{
    protected BoxCollider2D myCollider;
    protected SpriteRenderer myRenderer;
    protected Rigidbody2D myRigidBody;

    [SerializeField]
    protected int itemID;
    [SerializeField]
    protected int itemCount;

    // Start is called before the first frame update
    void Start()
    {
        setupVariables();
    }
    
    void setupVariables()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    public void setupItem(int id, Sprite newSprite)
    {
        if(myCollider == null || myRenderer == null || myRigidBody == null)
        {
            setupVariables();
        }
        itemID = id;
        itemCount = 1;
        myRenderer.sprite = newSprite;
        myCollider.size = new Vector2(1, 1);
        myRigidBody.AddForce(new Vector2(0.5f, 0.5f) * 500);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        InventoryHandler myObject = collision.gameObject.GetComponent<InventoryHandler>();
        bool result;
        if(myObject != null)
        {
            result = myObject.pickupItem(itemID);
            if(result)
            {
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("nope");
            }
        }
    }
}
