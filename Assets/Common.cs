using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class SortingOrder {
	static int order = 4;
	public static int GetNewTop() {
		order += 1;
		return order;
	}

}


public class Common : MonoBehaviour {

    Vector3 offset;
    bool firstClicked;
	GameObject mouseSelection;
	CityGraph cg;
	// Use this for initialization
	void Start () {
        firstClicked = true;
		cg = GameObject.Find("Cities").GetComponent<CityGraph>();
		if (cg == null)
			Debug.Log("Can't find citygraph!?!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseSelection = CheckForObjectUnderMouse();
            if(mouseSelection == null)
			{
                Debug.Log("nothing selected by mouse");
			}
            else {
				//onMouseDown
                Debug.Log("picked: " + mouseSelection.gameObject);
				//MouseDrag(mouseSelection);
				var deck = mouseSelection.GetComponent<PlayerDeck>();
				if (deck != null)
				{
					deck.Draw();
					return;
				}
				var infectDeck = mouseSelection.GetComponent<InfectDeck>();
				if (infectDeck != null)
				{
					Debug.Log("Got infect deck");
					PlayerDeck bDeck = infectDeck;
					bDeck.Draw();
					return;
				}
				if (mouseSelection.transform.parent.name == "Cities")
				{
					Debug.Log("clicked on city: " + mouseSelection.name);
					var neighbors = cg.GetNeighbors(mouseSelection);
					foreach (var node in neighbors) {
						Debug.Log("Neighbor: " + node.GetObj().name);
					}
				}
			}
        } else if (Input.GetMouseButton(0)) 
		{
				MouseDrag(mouseSelection);
		} else {
            //onMouseUp
			if (firstClicked == false)
                Debug.Log("mouse up!");
            Cursor.visible = true;
            firstClicked = true;
			mouseSelection = null;
		}
    }
	void MouseDrag(GameObject obj)
	{
		if (obj == null) return;
		if (obj.tag == "NoDrag") return;

        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = obj.transform.position.z;
        Cursor.visible = false;

        if (firstClicked)
        {
            firstClicked = false;

			obj.GetComponent<Card>().SetTopMost();
            
            //remember offset so card doesn't jump to cursor location
            offset = obj.transform.position - point;
        }

        obj.transform.position = point + offset;
    }
    private GameObject CheckForObjectUnderMouse()
    {
        Vector2 touchPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] allCollidersAtTouchPosition = Physics2D.RaycastAll(touchPostion, Vector2.zero);

        SpriteRenderer closest = null; //Cache closest sprite reneder so we can assess sorting order
        foreach(RaycastHit2D hit in allCollidersAtTouchPosition)
        {
            if(closest == null) // if there is no closest assigned, this must be the closest
            {
                closest = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                continue;
            }

            var hitSprite = hit.collider.gameObject.GetComponent<SpriteRenderer>();

            if(hitSprite == null)
                continue; //If the object has no sprite go on to the next hitobject

            if(hitSprite.sortingOrder > closest.sortingOrder)
                closest = hitSprite;
        }

        return closest != null ? closest.gameObject : null;
    }
}
