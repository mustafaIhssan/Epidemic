using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour {

	List<string> deck;
	public GameObject red, blue, black, yellow;
	// Use this for initialization
	void Start () {
		deck = Cities.GetDeck();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
   GameObject GetCardByType(string type) {
	   if (type == "red") return red;
	   if (type == "blue") return blue;
	   if (type == "black") return black;
	   if (type == "yellow") return yellow;

	   return red;
   }
   void OnMouseDown()
   {
	   //if not empty, spawn a new card on top, with offset?
	   if (deck.Count > 0)
	   {
		   //spawn a new card on top
		   var cardDrawn = deck[deck.Count-1];
		   var type = Cities.GetType(cardDrawn);
		   Debug.Log("Got a card: " + cardDrawn + " and type: " + type);
		   if (type == "invalid")
		   {
			   //TODO epidemic or special event card
		   } else {
			   //spawn a city card
			   GameObject cardType = GetCardByType(type);// = GameObject.Find(type);
			   var newCard = Instantiate(cardType, transform.position, transform.rotation);
			   var newOrder = SortingOrder.GetOrder();
			   newCard.GetComponent<SpriteRenderer>().sortingOrder = newOrder;
			   Debug.Log("Spawned a new card with order # " + newOrder);
		   }

		   //optional: just remember "top of deck" and leave deck unmodified
		   deck.RemoveAt(deck.Count-1);
	   }
	   if (deck.Count == 0)
	   {
	   	    //TODO if deck is empty, show blank texture
	   }
   }
}
