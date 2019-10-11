using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PIDI_2DReflection))]
public class ActiveReflect : MonoBehaviour
{
    BoxCollider2D myCollider;

    [SerializeField]
    PIDI_2DReflection myReflection;
    int playerMask = 512;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myReflection.renderLayers.value = playerMask;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myReflection.renderLayers.value = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        myReflection.renderLayers.value = playerMask;
        
    }
}
