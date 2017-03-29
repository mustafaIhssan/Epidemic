using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour {

	List<string> deck;
	public GameObject red, blue, black, yellow;
	public GameObject epidemic;
	public int numPlayer = 2;
	// Use this for initialization
	public virtual void Start () {
		InitDeck();
		if (gameObject.name == "playerdec")
		{
			InitPlayerDeck();
		}
	}
	public List<string> GetDeck() { return deck; }
	public void InitDeck() {
		deck = Cities.GetDeck();
	}
	public void InitPlayerDeck(int numPlayers=1) {
		//add epidemic cards
		Debug.Log("Initializing player deck");
		int numEpidemics = 5;
		int numCardsPerSec = deck.Count/numEpidemics;
		int offset = 0;
		//if (numPlayers == 1) 
		for (int section = 0; section < numEpidemics; section++)
		{
			int begin = section * numCardsPerSec + offset;
			int end = begin + numCardsPerSec - 1;
			//get random number from begin to ending
			int randIndex = UnityEngine.Random.Range(begin, end);
			deck.Insert(randIndex, "Epidemic");

			offset++;
		}

		//sanity checked
		int numSinceLastE = 0;
		for (int i = 0; i < deck.Count; numSinceLastE++, i++)
		{
            if (deck[i] == "Epidemic")
            {
                Debug.Log("num cards since last epidemic: " + numSinceLastE);
                numSinceLastE = 0;
            }
		}
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
   //auto remove card from deck
   public virtual GameObject Draw() { 
	   var ret = Draw(deck, 0);
	   if (deck.Count > 0)
	   {
		   deck.RemoveAt(deck.Count-1);
	   }
	   return ret;
   }
   public GameObject Draw(int lastOffset) {
	   var ret = Draw(deck, lastOffset);
        if (deck.Count > 0)
	   {
		   deck.RemoveAt(deck.Count-1);
	   }
	   return ret;
   }
   public string GetNameOfCard(GameObject obj) {
        var txtObj = obj.transform.FindChild("text");
        return txtObj.GetComponent<TextMesh>().text;
   }
    public GameObject Draw(List<string> deck, int lastOffset)
    {
        GameObject newCard = null;
        //if not empty, spawn a new card on top, with offset?
        if (deck.Count > 0)
        {
            //spawn a new card on top
            var cardDrawn = deck[deck.Count - 1 - lastOffset];
            var type = Cities.GetType(cardDrawn);
            Debug.Log("Got a card: " + cardDrawn + " and type: " + type);
            if (cardDrawn == "Epidemic")
            {
                newCard = Instantiate(epidemic, transform.position, transform.rotation);
            }
            else if (type == "invalid")
            {
                Debug.Log("invalid type");
                return null;
                //TODO epidemic or special event card
            }
            else
            {
                //spawn a city card
                GameObject cardType = GetCardByType(type);// = GameObject.Find(type);
                newCard = Instantiate(cardType, transform.position, transform.rotation);
            }
            if (newCard != null)
            {
                newCard.transform.Translate(new Vector3(0, 0, -1));
                var newOrder = newCard.GetComponent<Card>().SetTopMost();
                var txtObj = newCard.transform.FindChild("text");
                txtObj.GetComponent<TextMesh>().text = cardDrawn;

                Debug.Log("Spawned a new card with order # " + newOrder);
            }
        }
        else
        { //trying to draw from an empty deck, game over?
          //TODO game ending mechanic
            Debug.Log("empty deck!");
        }
        if (deck.Count == 0)
        {
            //TODO if deck is empty, show blank texture
        }
        return newCard;
    }
}
