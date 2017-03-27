using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDeck : PlayerDeck {
	List<string> lDiscard;
	Dictionary<string, GameObject> dDiscard;
	int cardsPopped = 0;
	// Use this for initialization
	public override void Start () {
		//deck = base.GetDeck();
		//deck = Cities.GetDeck();
		base.InitDeck();
	}
	
	public override GameObject Draw() {
		Debug.Log("infect deck draw");
        GameObject card = base.Draw(0);
		
        //push "top" onto discard
		string city = base.GetNameOfCard(card);
		lDiscard.Add(city); //ordered
		dDiscard.Add(city, card);

		return card;
	}
	public void ReturnDiscard() {
		//take discard list, reshuffle them, then add back
		Cities.Shuffle(ref lDiscard, 7);
		var deck = base.GetDeck();
		foreach(var city in lDiscard)
		{
            deck.Add(city);
		}
		lDiscard.Clear();
		foreach(var item in dDiscard)
		{
			Destroy(item.Value);
		}
		dDiscard.Clear();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
