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

	public GameObject dRed, dYellow, dBlue, dBlack;
    Vector3 offset;
    bool firstClicked;
	GameObject mouseSelection;
	CityGraph cg;
	GameObject cities;
	// Use this for initialization
	void Start () {
        firstClicked = true;
		cities = GameObject.Find("Cities");
		if (cities == null)
			Debug.Log("Can't find Cities!?!");
		cg = cities.GetComponent<CityGraph>();
		if (cg == null)
			Debug.Log("Can't find citygraph!?!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	GameObject selectedCity; //only set when a color has been changed
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
				var infectDeck = mouseSelection.GetComponent<InfectDeck>();
				if (deck != null)
				{
					deck.Draw();
					return;
				}
				else if (infectDeck != null)
				{
					Debug.Log("Got infect deck");
					PlayerDeck bDeck = infectDeck;
					bDeck.Draw();
				}
				else if (mouseSelection.tag == "InfectCity") 
				{
					InfectCity();
				}
				else if (mouseSelection.transform.parent.name == "Cities")
				{
					var sr = mouseSelection.GetComponent<SpriteRenderer>();
					selectedCity = mouseSelection;
					sr.color = new Color(.1f, 1f, .1f, 1f); //bright green
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
        }
        else //if (Input.GetMouseButtonUp(0))
        {
			if (selectedCity != null)
			{
				Debug.Log("changing city color back to white");
                var sr = selectedCity.GetComponent<SpriteRenderer>();
                sr.color = Color.white;
			}
            Cursor.visible = true;
            firstClicked = true;
			mouseSelection = null;
			selectedCity = null;
		}
    }
	void InfectCity()
	{
        //draw an infect card, move card to discard pile
        //infect city
        string target = "Madrid";
        string type = Cities.GetType(target);
        GameObject diseaseType;
        if (type == "blue") diseaseType = dBlue;
        else if (type == "red") diseaseType = dRed;
        else if (type == "black") diseaseType = dBlack;
        else if (type == "yellow") diseaseType = dYellow;
        else diseaseType = dRed;

        //find location to spawn
        GameObject targetCity = null;
        foreach (Transform tCity in cities.transform)
        {
            if (tCity.gameObject.name == target)
            {
                //found location
                targetCity = tCity.gameObject;
            }
        }
        if (targetCity == null)
        {
            Debug.Log("can't find city to infect??");
            return;
        }
        var newDisease = Instantiate(diseaseType, targetCity.transform.position, targetCity.transform.rotation);
        newDisease.transform.parent = targetCity.transform;
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
