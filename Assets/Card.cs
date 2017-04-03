using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	public int SetTopMost() {
		var top = SortingOrder.GetNewTop();
		SetOrder(top);
		return top;
	}
	public void SetOrder(int num) {
		//set sprite render order
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sortingOrder = num;

		//also set text render order
		var text = gameObject.transform.FindChild("text");
		if (text != null)
		{
            var tRender = text.GetComponent<Renderer>();
            if (tRender != null)
            {
                tRender.sortingOrder = num;
            }
        }
	}
	// Update is called once per frame
	void Update () {
		
	}
}
