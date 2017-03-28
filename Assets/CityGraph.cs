using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	public Node (GameObject o) {
		obj = o;
		links = new List<Node>();
	}
	public List<Node> GetNeighbors() {
		return links;
	}
	static int addCount = 0;
	public void AddOneWay(Node newNode) {
        //if not already there?
        if (newNode == null)
		{
			Debug.Log(obj.name + " trying to add invalid node given");
		}
		Debug.Log(obj.name + "->" + newNode.obj.name);
		if (!links.Contains(newNode))
            links.Add(newNode);
	}
	public void Add(Node newNode) {
        AddOneWay(newNode);
		//Debug.Log("reverse link: " + newNode.obj.name + "->" + obj.name );
		newNode.AddOneWay(this);
	}
	public GameObject GetObj() { return obj; }
	List<Node> links;
	GameObject obj;
}
public class CityGraph : MonoBehaviour {
	Dictionary<string, Node> cities = new Dictionary<string, Node>();
	Node dummy;
	// Use this for initialization
	void Start () {
		dummy = new Node(transform.gameObject);
        //get list of all cities
		Debug.Log("Cities has " + transform.childCount);
        foreach (Transform tChild in transform) {
			var child = tChild.gameObject;
			if (child == null)
			{
				Debug.Log("empty child");
				continue;
			}
			cities.Add(child.name, new Node(child));
			child.tag = "NoDrag";
		}
		InitGraph();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Node> GetNeighbors(GameObject input) {
		var node = cities[input.name];
		return node.GetNeighbors();

	}
	void InitGraph() {
		

        dummy.AddOneWay(cities["Atlanta"]);
		cities["Atlanta"].Add(cities["Chicago"]);
		cities["Atlanta"].Add(cities["Washington"]);
		cities["Atlanta"].Add(cities["Miami"]);
		
		cities["Chicago"].Add(cities["Montreal"]);
		cities["Chicago"].Add(cities["San Francisco"]);
		cities["Chicago"].Add(cities["Los Angeles"]);
		cities["Chicago"].Add(cities["Mexico City"]);

		cities["Montreal"].Add(cities["New York"]);
		cities["Montreal"].Add(cities["Washington"]);

		cities["Washington"].Add(cities["New York"]);
		cities["Washington"].Add(cities["Miami"]);

		cities["New York"].Add(cities["London"]);
		cities["New York"].Add(cities["Madrid"]);

		cities["San Francisco"].Add(cities["Tokyo"]);
		cities["San Francisco"].Add(cities["Manila"]);
		cities["San Francisco"].Add(cities["Los Angeles"]);

		cities["Los Angeles"].Add(cities["Mexico City"]);
		cities["Los Angeles"].Add(cities["Sydney"]);
		cities["Los Angeles"].Add(cities["Lima"]);

		cities["Mexico City"].Add(cities["Bogota"]);
		cities["Mexico City"].Add(cities["Lima"]);
		cities["Mexico City"].Add(cities["Miami"]);

		cities["Bogota"].Add(cities["Miami"]);
		cities["Bogota"].Add(cities["Lima"]);
		cities["Bogota"].Add(cities["Buenos Aires"]);
		cities["Bogota"].Add(cities["Sao Paulo"]);

		cities["Lima"].Add(cities["Santiago"]);

		cities["Santiago"].Add(cities["Buenos Aires"]);

		cities["Buenos Aires"].Add(cities["Sao Paulo"]);
		cities["Buenos Aires"].Add(cities["Johannesburg"]);

		cities["Lagos"].Add(cities["Khartoum"]);
		cities["Lagos"].Add(cities["Kinshasa"]);

		cities["Kinshasa"].Add(cities["Khartoum"]);
		cities["Kinshasa"].Add(cities["Johannesburg"]);

		cities["Khartoum"].Add(cities["Johannesburg"]);
		cities["Khartoum"].Add(cities["Cairo"]);

		cities["Cairo"].Add(cities["Algiers"]);
		cities["Cairo"].Add(cities["Istanbul"]);
		cities["Cairo"].Add(cities["Baghdad"]);
		cities["Cairo"].Add(cities["Riyadh"]);

		cities["Algiers"].Add(cities["Madrid"]);
		cities["Algiers"].Add(cities["Paris"]);
		cities["Algiers"].Add(cities["Istanbul"]);

		cities["Istanbul"].Add(cities["Milan"]);
		cities["Istanbul"].Add(cities["St. Petersburg"]);
		cities["Istanbul"].Add(cities["Moscow"]);
		cities["Istanbul"].Add(cities["Baghdad"]);

		cities["Baghdad"].Add(cities["Tehran"]);
		cities["Baghdad"].Add(cities["Riyadh"]);

		cities["London"].Add(cities["Madrid"]);
		cities["London"].Add(cities["Paris"]);
		cities["London"].Add(cities["Essen"]);
		
		cities["Madrid"].Add(cities["Paris"]);
		
		cities["Paris"].Add(cities["Essen"]);
		cities["Paris"].Add(cities["Milan"]);

		cities["Essen"].Add(cities["Milan"]);
		cities["Essen"].Add(cities["St. Petersburg"]);

		cities["Moscow"].Add(cities["St. Petersburg"]);
		cities["Moscow"].Add(cities["Tehran"]);

		cities["Tehran"].Add(cities["Delhi"]);
		cities["Tehran"].Add(cities["Karachi"]);

		cities["Karachi"].Add(cities["Delhi"]);
		cities["Karachi"].Add(cities["Riyadh"]);
		cities["Karachi"].Add(cities["Mumbai"]);

		cities["Delhi"].Add(cities["Mumbai"]);
		cities["Delhi"].Add(cities["Chennai"]);
		cities["Delhi"].Add(cities["Kolkata"]);

		cities["Chennai"].Add(cities["Kolkata"]);
		cities["Chennai"].Add(cities["Mumbai"]);
		cities["Chennai"].Add(cities["Jakarta"]);

		cities["Bangkok"].Add(cities["Jakarta"]);
		cities["Bangkok"].Add(cities["Kolkata"]);
		cities["Bangkok"].Add(cities["Hong Kong"]);
		cities["Bangkok"].Add(cities["Ho Chi Minh City"]);

		cities["Hong Kong"].Add(cities["Ho Chi Minh City"]);
		cities["Hong Kong"].Add(cities["Kolkata"]);
		cities["Hong Kong"].Add(cities["Shanghai"]);
		cities["Hong Kong"].Add(cities["Taipei"]);
		cities["Hong Kong"].Add(cities["Manila"]);

		cities["Shanghai"].Add(cities["Taipei"]);
		cities["Shanghai"].Add(cities["Beijing"]);
		cities["Shanghai"].Add(cities["Seoul"]);
		cities["Shanghai"].Add(cities["Tokyo"]);

		cities["Seoul"].Add(cities["Tokyo"]);
		cities["Seoul"].Add(cities["Beijing"]);

		cities["Osaka"].Add(cities["Tokyo"]);
		cities["Osaka"].Add(cities["Taipei"]);

		cities["Jakarta"].Add(cities["Ho Chi Minh City"]);
		cities["Jakarta"].Add(cities["Sydney"]);

		cities["Manila"].Add(cities["Ho Chi Minh City"]);
		cities["Manila"].Add(cities["Taipei"]);
		cities["Manila"].Add(cities["Sydney"]);
	}
}
