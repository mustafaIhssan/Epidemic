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

public static class Cities {
	public static List<List<string>> GetCityLists() {
		List<List<string>> types = new List<List<string>>();
		types.Add(blue);
		types.Add(red);
		types.Add(yellow);
		types.Add(black);
		
		return types;
	}
	static void Shuffle(ref List<string> deck) {
		for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
	}
	public static List<string> GetDeck() {
		var types = GetCityLists();
		//get all cities in one list
		List<string> cities = new List<string>();
		cities.AddRange(blue);
		cities.AddRange(red);
		cities.AddRange(yellow);
		cities.AddRange(black);

		for (int i = 0; i < 17; i++)
            Shuffle(ref cities);
#if true
        foreach (var city in cities) {
			var type = GetType(city);
			Debug.Log(type + ": " + city);
		}
#endif
        return cities;
	}
	public static string GetType(string c) {
		List<string> typeName = new List<string>(new string[] { 
			"blue", 
			"red", 
			"yellow",
			"black"
		});

		var types = GetCityLists();
		int i = 0;
		foreach(var type in types) {
			foreach(var city in type) {
				if (city.Equals(c, StringComparison.Ordinal)) {
					return typeName[i];
				}
			}
			i++;
		}
		Debug.Log("not a valid city given!");
		return "invalid";
	}
	public static List<string> blue = new List<string>(new string[] { 
		"San Francisco",
		"Chicago",
		"Atlanta",
		"Montreal",
		"New York",
		"Washtingon",
		"London",
		"Madrid",
		"Paris",
		"Essen",
		"Milan",
		"St. Petersburg"
	});
	public static List<string> yellow = new List<string>(new string[] { 
		"Lagos",
		"Khartoum",
		"Kinshasa",
		"Johannesburg",
		"Sao Paulo",
		"Bueno Aires",
		"Santiago",
		"Lima",
		"Bogota",
		"Miami",
		"Mexico City",
		"Los Angeles"
	});
	public static List<string> black = new List<string>(new string[] { 
		"Algiers",
		"Istanbul",
		"Cairo",
		"Baghdad",
		"Moscow",
		"Tehran",
		"Karachi",
		"Delhi",
		"Kolkata",
		"Mumbai",
		"Chennai"
	});

	public static List<string> red = new List<string>(new string[] { 
		"Bangkok",
		"Beijing",
		"Shanghai",
		"Hong Kong",
		"Seoul",
		"Tokyo",
		"Osaka",
		"Taipei",
		"Manila",
		"Ho Chi Minh City",
		"Jakarta",
		"Sydney"
	});


}
public class Common : MonoBehaviour {

    Vector3 offset;
    bool firstClicked;
	GameObject mouseSelection;
	// Use this for initialization
	void Start () {
        firstClicked = true;
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
                Debug.Log(mouseSelection.gameObject);
				//MouseDrag(mouseSelection);
				var deck = mouseSelection.GetComponent<PlayerDeck>();
				if (deck != null)
				{
					deck.Draw();
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
