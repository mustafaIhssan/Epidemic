using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//tracks player/pawns
public class Player {
	public Player(GameObject o, int m=4) { obj = o; moves = 4; }
	public int moves;
	public GameObject obj;
}
public class Players : MonoBehaviour {

	// Use this for initialization
	List<GameObject> pawns = new List<GameObject>();
	List<Player> playersInGame = new List<Player>();
    TextMeshPro textPlayers;
	int curPlayerIdx = 0;
	CityGraph cg;
	int NextPlayer() 
	{ 
		curPlayerIdx = (curPlayerIdx + 1) % playersInGame.Count;
        return curPlayerIdx;
    }
	Player curPlayer() { return playersInGame[curPlayerIdx]; }
	void Start () {
		//add all pawns to list
		foreach(Transform tchild in transform) {
			pawns.Add(tchild.gameObject);
		}
		var tp = GameObject.Find("textPlayers");
		if (tp == null) { Debug.Log("textPlayers not found!"); }
		textPlayers = tp.GetComponent<TextMeshPro>();
		var cities = GameObject.Find("Cities");
		if (cities == null) { Debug.Log("cities not found"); }
		cg = cities.GetComponent<CityGraph>();

	}
	public bool setup = true;
	public bool ExitSetup(bool playerDeck) {
		//either playerdeck or infect deck
		if (playerDeck == false)
		{
			return setup;
		}
		if (playersInGame.Count > 1)
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
		} else {
            //check if legal move (immediate neighbor, has the right card for flight, within turn # of neighbors)
            //check number of moves to city on foot
            Debug.Log("finding shortest path");
			var pawnCity = pawn.transform.parent.gameObject;
			int numMoves = cg.FindShortestDistanceToCity(pawnCity, city, 4);
			Debug.Log("Shortest path found: " + numMoves);
			ret = (numMoves <= 4);

			//find distance from pawn.parent in city graph
            //check if it's dispatcher's move and she has enough turns for above
        }
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
                playersInGame.Add(new Player(pawn));
				string stats = "Players {0}\nTurn: " + curPlayer().obj.name
								+ "\nMoves: {1}";
                textPlayers.SetText(stats, playersInGame.Count, curPlayer().moves);
        // The text displayed will be:
        // The first number is 4 and the 2nd is 6.35 and the 3rd is 4.
		//HACK revert after testing shortest path
				if (true) {//transform.childCount == 0) {
					setup = false;
					Debug.Log("Exiting player setup");
				}
				ret = true;
            } else {
				ret = false;
			}
		} else {
			//check number of moves to city
			var pawnCity = pawn.transform.parent.gameObject;
			int numMoves = cg.FindShortestDistanceToCity(pawnCity, city, 4);
			ret = (numMoves <= 4);
            pawn.transform.parent = city.transform;
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
			float scaler = .4f;
			if (pawn.transform.parent.tag == "City")
				scaler *= 5;
			if (true)//
			{
				float progress = (4 * Time.deltaTime);
				var localPos = pawn.transform.localPosition;
				var dist = localPos.magnitude;
				Vector3 newPos;
				if (dist < scaler+.1f)
				{
					//Debug.Log("diff: " + dist);
					newPos = localPos.normalized * scaler;//progress * (localPos.normalized * 0.4f - localPos) + localPos;
				}
				else
                    newPos = (1 - progress) * localPos;
				pawn.transform.localPosition = newPos;
			}
		}
	}
}
