using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDeck : PlayerDeck {
	List<string> lDiscard = new List<string>();
	Dictionary<string, GameObject> dDiscard = new Dictionary<string, GameObject>();
	GameObject discardPile;
	int cardsPopped = 0;
	SpawnInfection spawn;
	// Use this for initialization
	public override void Start () {
		//deck = base.GetDeck();
		//deck = Cities.GetDeck();
		base.InitDeck();
		discardPile = GameObject.Find("infectDiscard");
		if (discardPile == null)
			Debug.Log("no infectDiscard found");

        spawn = GetComponent<SpawnInfection>();
        if (spawn == null)
			Debug.Log("no SpawnInfection found");
	}
	
	Dictionary<GameObject, float> animateQueue = new Dictionary<GameObject, float>();
	void Update() {
		//move drawn card over to discard pile
		List<GameObject> cardsToRemove = new List<GameObject>();
		foreach(var pair in animateQueue)
		{
            var card = pair.Key;
			var drawTime = pair.Value;

            float duration = 1f;
            float progress = (Time.time - drawTime) / duration;
            if (progress > 1)
			{
				cardsToRemove.Add(card);
				continue;
			}
                
            var newPos = (1 - progress) * card.transform.position + progress * discardPile.transform.position;
            card.transform.position = newPos;
        }
		foreach(var card in cardsToRemove)
		{
			animateQueue.Remove(card);
		}

	}
	int drawCount = 0;
	public override GameObject Draw() {
		Debug.Log("infect deck draw");
        GameObject card = base.Draw(0);
		
        //push "top" onto discard
		string city = base.GetNameOfCard(card);
		lDiscard.Add(city); //ordered
		dDiscard.Add(city, card);
		animateQueue.Add(card, Time.time);

		//infect!
		int numInfect = 1;
		if (drawCount < 3) numInfect = 3;
		else if (drawCount < 6) numInfect = 2;
		drawCount++;
		spawn.InfectCity(city, numInfect);
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
}
