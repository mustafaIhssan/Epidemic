using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[RequireComponent(typeof(SphereCollider))]
 
public class DragMe : MonoBehaviour
{
    void OnMouseDrag()
    {
		Debug.Log("on mouse drag");
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = gameObject.transform.position.z;
        gameObject.transform.position = point;
        Cursor.visible = false;
    }
   
    void OnMouseUp()
    {
		Debug.Log("on mouse up");
        Cursor.visible = true;
    }
}