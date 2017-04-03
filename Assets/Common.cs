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
	InfectDeck infectDec;
	Players pawns;
	void Start () {
		Physics.queriesHitTriggers = true;
        firstClicked = true;
		cities = GameObject.Find("Cities");
		if (cities == null)
			Debug.Log("Can't find Cities!?!");
		cg = cities.GetComponent<CityGraph>();
		if (cg == null)
			Debug.Log("Can't find citygraph!?!");

        pawns = GameObject.Find("Pawns").GetComponent<Players>();
		if (pawns == null)
			Debug.Log("Can't find Pawns");

        infectDec = GameObject.Find("infectDec").GetComponent<InfectDeck>();
		if (infectDec == null)
			Debug.Log("Can't find infectDec");
	}
	
	// Update is called once per frame
		
	GameObject selectedCity; //only set when a color has been changed
	void Update () 
    {
		MouseUpdate();
		//PullAnimation();
	}
	
	int playerCardsDrawn = 0;
	void MouseDown(ref GameObject mouseSelection)
    {
        mouseSelection = CheckForObjectUnderMouse();
        if (mouseSelection == null)
        {
            Debug.Log("nothing selected by mouse");
        }
        else
        {
            //onMouseDown
            //Debug.Log("picked: " + mouseSelection.gameObject);
            var deck = mouseSelection.GetComponent<PlayerDeck>();
            var infectDeck = mouseSelection.GetComponent<InfectDeck>();
            if (mouseSelection.tag == "Player")
            {
                //no need to record position, just move back to parent on illegal move
            }
            else if (infectDeck != null)
            {
                Debug.Log("Got infect deck");
                PlayerDeck bDeck = infectDeck;
                //if <= 9 cards drawn OR no longer in setup mode
                if (playerCardsDrawn <= 9 || pawns.ExitSetup(false))
                {
                    playerCardsDrawn++;
                    bDeck.Draw();
                }
            }
            else if (deck != null)
            {
                Debug.Log("picked playerDeck");
                if (pawns.ExitSetup(true) == false)
                {
                    Debug.Log("Can't draw cards until players chosen");
                    return;
                }
                deck.Draw();
                return;
            }
            else if (mouseSelection.tag == "InfectCity")
            {
                //setup
                //SetupGame();

                //InfectCity("Madrid", 3);
            }
            else if (mouseSelection.transform.parent != null
                  && mouseSelection.transform.parent.name == "Cities")
            {
                var sr = mouseSelection.GetComponent<SpriteRenderer>();
                selectedCity = mouseSelection;
                sr.color = new Color(.1f, 1f, .1f, 1f); //bright green
                Debug.Log("clicked on city: " + mouseSelection.name);
                var neighbors = cg.GetNeighbors(mouseSelection);
                foreach (var node in neighbors)
                {
                    Debug.Log("Neighbor: " + node.GetObj().name);
                }
                //testing
                var cube = GameObject.Find("disease_blue");
                InfectCity(mouseSelection, 1);
            }
        }
    }
    GameObject FindOverCity()
    {
        Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] col = Physics2D.OverlapPointAll(v);

        foreach (Collider2D c in col)
        {
            //Debug.Log("Collided with: " + c.gameObject.name + " tag: " + c.gameObject.tag);
            if (c.gameObject.tag == "City")
            {
                //Debug.Log("Collided with city: " + c.gameObject.name);
                return c.gameObject;
            }
        }
        return null;
    }
	void MouseUpdate()
	{
        if(Input.GetMouseButtonDown(0))
		{
			//first frame where left mouse button is clicked
			MouseDown(ref mouseSelection);
		} else if (Input.GetMouseButton(0)) 
		{
			//subseqpent frames where left mouse button is still held down
            MouseDrag(ref mouseSelection);

            //check if mouse over city
            var overCity = FindOverCity();
            if (overCity != null) //mouse dragged over city while dragging something
            {
                if (selectedCity == overCity) { return; }
                selectedCity = overCity;
                Debug.Log("2Collided with: " + selectedCity.name);
                var sr = selectedCity.GetComponent<SpriteRenderer>();
                if (pawns.CanMoveToCity(mouseSelection, selectedCity) == true)
				{
                    sr.color = new Color(.1f, 1f, .1f, 1f); //bright green
                } else {
                    sr.color = new Color(1f, .1f, .1f, 1f); //bright red
                }
            } else {
				//pawn not collided with city anymore
				DeselectCity();
			}
        }
        else //if (Input.GetMouseButtonUp(0))
        {
            if (mouseSelection != null && selectedCity != null
			&& pawns.CanMoveToCity(mouseSelection, selectedCity) == true)
			{
				pawns.MoveToCity(mouseSelection, selectedCity);
			}
			DeselectCity(); //mouse up when over a city
			//clean up
            Cursor.visible = true;
            firstClicked = true;
			mouseSelection = null;
		}
    }
	void DeselectCity()
	{
        if (selectedCity != null)
        {
            Debug.Log("changing city color back to white");
            var sr = selectedCity.GetComponent<SpriteRenderer>();
            sr.color = Color.white;
			selectedCity = null;
        }
    }
	void SetupGame()
	{
		//draw a card from infection deck, 
		PlayerDeck bDeck = infectDec;
		bDeck.Draw();
		//slide it over to discard deck
		//spawn 3 infect on city
	}
	void InfectCity(string target = "Milan", int infectCount=1)
	{
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
		InfectCity(targetCity, infectCount);
	}	void InfectCity(GameObject targetCity, int infectCount=1, string type=null)
	{
        if (targetCity == null)
        {
            Debug.Log("can't find city to infect??");
            return;
        }
		
        //draw an infect card, move card to discard pile
        //infect city
        //string target = "Madrid";
		if (type == null)
            type = Cities.GetType(targetCity.name);

        GameObject diseaseType;
        if (type == "blue") diseaseType = dBlue;
        else if (type == "red") diseaseType = dRed;
        else if (type == "black") diseaseType = dBlack;
        else if (type == "yellow") diseaseType = dYellow;
        else diseaseType = dRed;

        //check number of infections already in city
        int diseaseCount = 0;
		foreach (Transform tChild in targetCity.transform)
		{
			if (tChild.gameObject.tag == "disease" 
				&& tChild.gameObject.name.Contains("disease_"+type))
			{
				diseaseCount++;
			}
		}

        bool outbreak = ((diseaseCount + infectCount) > 3);
		if ((3 - diseaseCount) < infectCount)
		{
			infectCount = 3 - diseaseCount;
		}
        
		var cityPosition = targetCity.transform.position;
		for (int i = 0; i < infectCount; i++)
		{
            //add offset to position
            float x = (float)UnityEngine.Random.Range(-30, 30);
            x = x / 3f + Mathf.Sign(x) * 20f;
            float y = (float)UnityEngine.Random.Range(-30, 30);
            y = y / 3f + Mathf.Sign(y) * 20f;
            cityPosition += new Vector3(x / 100f, y / 100f, 0);
            var newDisease = Instantiate(diseaseType, cityPosition, targetCity.transform.rotation);
            newDisease.transform.parent = targetCity.transform;
            newDisease.GetComponent<Attraction>().pullSource = targetCity;
        }
		if (outbreak && !cg.GetNode(targetCity.name).hasOutbreak)
        {
            cg.GetNode(targetCity.name).hasOutbreak = true;
			//set self as having outbreak
            //find neighbors and add infect 1 to each of them
            var neighbors = cg.GetNeighbors(targetCity.name);
            foreach (var node in neighbors)
            {
                Debug.Log("Infecting Neighbor: " + node.GetObj().name);
				InfectCity(node.GetObj(), 1, type);
            }
        }
    }
	void MouseDrag(ref GameObject obj)
	{
        //check if player's turn yet
        //moving an object
        if (obj == null) return;
        if (obj.tag == "Player"
        && pawns.CanSelectPawn(obj) == false)
        {
            Debug.Log("Not your turn yet!"); //how many turns to wait?
            return;
        }
		if (obj.tag == "City") return;

        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = obj.transform.position.z;
        Cursor.visible = false;

        if (firstClicked)
        {
            firstClicked = false;

			{
                var card = obj.GetComponent<Card>();
                if (card)
                    card.SetTopMost();
            }
            
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
