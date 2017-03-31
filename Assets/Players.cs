using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tracks player/pawns
public class Players : MonoBehaviour {

	// Use this for initialization
	List<GameObject> pawns = new List<GameObject>();
	void Start () {
		//add all pawns to list
		foreach(Transform tchild in transform) {
			pawns.Add(tchild.gameObject);
		}
	}
	public bool setup = true;
	int playersInGame = 0;
	public bool ExitSetup(bool playerDeck) {
		//either playerdeck or infect deck
		if (playerDeck == false)
		{
			return setup;
		}
		if (playersInGame > 1)
		{
			setup = false;
			return true;
		} else {
			return false;
		}
	}
	public bool CanSelectPawn(GameObject pawn)
	{
		bool ret = setup;
        //check if it's pawn's turn or a dispatcher's turn and she has moves left
        if (true)
        {
            ret = true;
        }
		return ret;
	}
    public bool CanMoveToCity(GameObject pawn, GameObject city)
    {
		bool ret = setup;
		if (setup == true)
		{
            //if moving to atlanta, and pawn hasn't been moved
			Debug.Log(pawn.transform.parent.name + "." + pawn.name 
			+ " over " + city.name);
            if (city.name == "Atlanta" 
		    && pawn.transform.parent.name == "Pawns")
            {
				return true;
            } else {
				return false;
			}
		}
		//check if it's pawn's turn
		//check if legal move (immediate neighbor, has the right card for flight, within turn # of neighbors)
		//check if it's dispatcher's move and she has enough turns for above
		return ret;
	}
	public bool MoveToCity(GameObject pawn, GameObject city)
	{
		bool ret = setup;
		if (setup == true)
		{
            //if moving to atlanta, and pawn hasn't been moved
			Debug.Log(pawn.transform.parent.name + "." + pawn.name 
			+ " over " + city.name);
            if (city.name == "Atlanta" 
		    && pawn.transform.parent.name == "Pawns")
            {
				pawn.transform.parent = city.transform;
                playersInGame++;
				if (transform.childCount == 0) {
					setup = false;
				}
				return true;
            } else {
				return false;
			}
		}
		//check if it's pawn's turn
		//check if legal move (immediate neighbor, has the right card for flight, within turn # of neighbors)
		//check if it's dispatcher's move and she has enough turns for above
		return ret;
	}
	// Update is called once per frame
	void Update () {
		//until first card is drawn, setup will be true
		//monitor location of pawns, if one is moved to atlanta, 
		//move it to be a child of atlanta and incr players in game

		//force pawn back to its city if it has one
		foreach(var pawn in pawns)
		{
			if (pawn.transform.parent.tag == "City")
			{
				float progress = (4 * Time.deltaTime);
				var parentPos = pawn.transform.parent.transform.position;	
				if ((parentPos - pawn.transform.position).sqrMagnitude < .1f)
					continue;
				var newPos = (1-progress) * pawn.transform.position + progress * parentPos;
				pawn.transform.position = newPos;
			}
		}
	}
}
