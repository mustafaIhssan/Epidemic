using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDeck : PlayerDeck {
	List<string> lDiscard = new List<string>();
	Dictionary<string, GameObject> dDiscard = new Dictionary<string, GameObject>();
	GameObject discardPile;
	GameObject epidemicToken, IRParent;
	int cardsPopped = 0;
	SpawnInfection spawn;
	// Use this for initialization
	public override void Start () {
		epidemicToken = GameObject.Find("epidemic_token");
		if (epidemicToken == null)
			Debug.Log("epidemicToken not found");
		IRParent = GameObject.Find("InfectionRate");
		if (IRParent == null)
			Debug.Log("IRParent not found");
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
	public class Animation {
		public Animation(GameObject s, GameObject d, float t) {
			src = s; 
			dest = d;
			startTime = t;
		}
		public GameObject src, dest;
		public float startTime;
	}
	//where to check how many infection cards to draw??
	int infectionRate = 1; //# of cards to draw (proxy)
	public void Epidemic() {
		Debug.Log("EPIDEMIC!");

        //move epidemic marker by 1
#if true
        infectionRate++;
		if (infectionRate > 7) { Debug.Log("GAME OVER"); return; }
		Debug.Log("Finding " + infectionRate.ToString() + " from epidemic");
		var tdest = IRParent.transform.FindChild(infectionRate.ToString());
		if (tdest == null) { Debug.Log("can't find infection marker pos"); return; }

        Debug.Log(epidemicToken.name + " -> " + tdest.gameObject.name);
		animateQueue.Add(new Animation(epidemicToken, tdest.gameObject, Time.time));
#endif

        //draw card from bottom, put in discard pile, infect city 3 cubes
        var card = Draw(false); //automatically infects 3 w/ false param

		//shuffle discard pile, put back into infect deck
		Debug.Log("return discard");
		//ReturnDiscard();
		rdFlag = true;
	}
	bool rdFlag = false;
	//Dictionary<GameObject, float> animateQueue = new Dictionary<GameObject, float>();
	List<Animation> animateQueue = new List<Animation>();
	void Update() {
		//move drawn card over to discard pile
		List<Animation> elemToRemove = new List<Animation>();
		foreach(var e in animateQueue)
		//for (int i = 0; i < animateQueue.Count; i++)
		{
			//var e = animateQueue[i];
            float duration = 1f;
            float progress = (Time.time - e.startTime) / duration;
			Debug.Log("dest: " + e.dest.name);
			Debug.Log("time: " + Time.time + " start time: " + e.startTime);
			Debug.Log("src: " + e.src.name);
            if (progress > 1)
			{
				Debug.Log("animation over for " + e.src.name + " -> " + e.dest.name);
				elemToRemove.Add(e);
				Debug.Log("remove queue size: " + elemToRemove.Count);
				continue;
			}
                
			//Debug.Log(e.src.name + " -> " + e.dest.name);
            var newPos = (1 - progress) * e.src.transform.position + progress * e.dest.transform.position;
            e.src.transform.position = newPos;
        }
        Debug.Log("out of anim loop remove queue size: " + elemToRemove.Count);
		foreach(var elem in elemToRemove)
		{
			Debug.Log("removing item: " + elem);//.src.name);
			animateQueue.Remove(elem);
		}
		if (rdFlag == true && animateQueue.Count == 0)
		{
			rdFlag = false;
			ReturnDiscard();
		}

	}
	int drawCount = 0;
	public override GameObject Draw(bool DrawFromTop=true) {
		Debug.Log("infect deck draw");
        GameObject card = base.Draw(DrawFromTop);
		
        //push "top" onto discard
		string city = base.GetNameOfCard(card);
		lDiscard.Add(city); //ordered
		dDiscard.Add(city, card);
		animateQueue.Add(new Animation(card, discardPile, Time.time));

		//setup code
		int numInfect = 1;
		if (drawCount < 3) numInfect = 3;
		else if (drawCount < 6) numInfect = 2;
		drawCount++;

		//epidemic code
		if (!DrawFromTop) numInfect = 3; //only happens on epidemic

		spawn.InfectCity(city, numInfect);
		return card;
	}
	public void ReturnDiscard() {
		//take discard list, reshuffle them, then add back
		Cities.Shuffle(ref lDiscard, 7);
		base.Append(lDiscard);
		lDiscard.Clear();
		foreach(var item in dDiscard)
		{
			Destroy(item.Value);
		}
		dDiscard.Clear();
	}
}
