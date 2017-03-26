using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[RequireComponent(typeof(BoxCollider2D))]
 
public class DragMe : MonoBehaviour
{
    SpriteRenderer render;
    Vector3 offset;
    bool firstClicked;
    void Start() {
        render = GetComponent<SpriteRenderer>();
        firstClicked = true;
    }
    void OnMouseDown2() {
        //do nothing, to override corresponding function in deck
    }
    void OnMouseDrag2()
    {
		//Debug.Log("on mouse drag");
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = gameObject.transform.position.z;
        Cursor.visible = false;
        
        if (firstClicked)
        {
            firstClicked = false;

            //render card as top most
            render.sortingOrder = SortingOrder.GetOrder();

            //remember offset so card doesn't jump to cursor location
            offset = gameObject.transform.position - point;
        }

        gameObject.transform.position = point + offset;
    }

    void OnMouseUp2()
    {
        firstClicked = true;
		//Debug.Log("on mouse up");
        Cursor.visible = true;
    }
    
}