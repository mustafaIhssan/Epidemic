using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDeck : PlayerDeck {
	List<string> deck;
	// Use this for initialization
	public override void Start () {
		//deck = base.GetDeck();
		//deck = Cities.GetDeck();
		base.InitDeck();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
