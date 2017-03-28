using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour {
	public GameObject pullSource;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (pullSource == null) return;

		Debug.Log("pulling!");
		//continue
		var dir = pullSource.transform.position - transform.position;
		var dist2 = dir.sqrMagnitude;
		var vel = dir.normalized * 8f *dist2;
		if (dist2 > .02f)
            GetComponent<Rigidbody2D>().velocity = vel;
	}
}
