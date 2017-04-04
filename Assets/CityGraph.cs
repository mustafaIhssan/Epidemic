using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	public Node (GameObject o) {
		obj = o;
		links = new List<Node>();
		hasOutbreak = false;
	}
	public List<Node> GetNeighbors() {
		return links;
	}
	public void AddOneWay(Node newNode) {
        //if not already there?
        if (newNode == null)
		{
			Debug.Log(obj.name + " trying to add invalid node given");
		}
		//Debug.Log(obj.name + "->" + newNode.obj.name);
		if (!links.Contains(newNode))
            links.Add(newNode);
	}
	public void Add(Node newNode) {
        AddOneWay(newNode);
		//Debug.Log("reverse link: " + newNode.obj.name + "->" + obj.name );
		newNode.AddOneWay(this);
	}
	public GameObject GetObj() { return obj; }
	public List<Node> links;
	GameObject obj;
	public bool hasOutbreak;
}
public class CityGraph : MonoBehaviour {
	Dictionary<string, Node> cities = new Dictionary<string, Node>();
	Node dummy;
	GameObject outbreakToken, outbreaks;
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
			//child.tag = "NoDrag";
		}
		InitGraph();
		outbreakToken = GameObject.Find("outbreak_token");
		if (outbreakToken == null)
		{ Debug.Log("can't find outbreak token!");}
		outbreaks = GameObject.Find("Outbreaks");
		if (outbreaks == null)
		{ Debug.Log("can't find outbreaks!");}
	}
	// Update is called once per frame
	bool animateFlag = false;
	int numOutbreaks = 0;
	int curOutbreak = 0;
	float moveTime = 0;
	GameObject moveDest;
	void Update () {
		foreach(var item in cities)
		{
			//if has outbreak, move marker!
			if (item.Value.hasOutbreak)
			{
				numOutbreaks++;
                if (numOutbreaks > 7)
                {
                    Debug.Log("GAME OVER");
					numOutbreaks = 8;
                }
				moveTime = Time.time;
				//get target
				Debug.Log("moving outbreak token to " + curOutbreak);
				moveDest = outbreaks.transform.Find((1+curOutbreak).ToString()).gameObject;
			}

			item.Value.hasOutbreak = false;
		}
		if (curOutbreak < numOutbreaks) animate();
	}
    void animate()
    {
		if (moveDest == null) return;
        float duration = 1f;
        float progress = (Time.time - moveTime) / duration;
        if (progress > 1)
        {
			curOutbreak++;
            moveDest = outbreaks.transform.Find(curOutbreak.ToString()).gameObject;
			moveTime = Time.time;
			return;
        }

        var newPos = (1 - progress) * outbreakToken.transform.position + progress * moveDest.transform.position;
        outbreakToken.transform.position = newPos;
    }

	public List<Node> GetNeighbors(GameObject input) {
		return GetNeighbors(input.name);
	}
	public List<Node> GetNeighbors(string name) {
		var node = cities[name];
		if (node == null) {
			Debug.Log("can't find target city to get neighbors");
			return null;
		}
		return node.GetNeighbors();
	}
	public Node GetNode(string name) {
		return cities[name];
	}
	//find shortestpath
	void Retrace(Node src, Node tgt, ref List<List<Node>> allPaths, int numMoves)
	{
		List<Node> path = new List<Node>();
		//iterate from last layer to 0
		Node match = tgt;
		Debug.Log("Retracing: ");
		for(int index = allPaths.Count-2; index >= 0; index--)
		{
			//Debug.Log("Layer: " + index);
			foreach(Node n in allPaths[index])
			{
				bool found = false;
				foreach(Node l in n.links)
				{
					if (l == match)
					{
						found = true;
						//Debug.Log(n.GetObj().name);
						path.Add(n);
						match = n;
						break;
					}
				}
				if (found == true)
				{
					break;
				}
			}
		}
		string trace = tgt.GetObj().name + "<-";
		int numInvalid = path.Count - numMoves;
		foreach (Node n in path) 
		{
			var green = new Color(.1f, 1f, .1f, 1f); //bright green
			var red = new Color(1f, .1f, .1f, 1f); //bright red
			var selectedCity = n.GetObj();
            var sr = selectedCity.GetComponent<SpriteRenderer>();
			if (numInvalid-- > 1)
                sr.color = red;
            else
                sr.color = green;

			trace += n.GetObj().name + "<-";
		}
		Debug.Log(trace);
	}
	public void SetAllCityColor()
	{
		foreach (var entry in cities)
		{
			var sr = entry.Value.GetObj().GetComponent<SpriteRenderer>();
			sr.color = Color.white;
		}
	}
	public int FindShortestDistanceToCity(GameObject source, GameObject target, int numMoves) {
		//breadth first search from source
		if (source == target) return 0;

		List<Node> traversed = new List<Node>();
		Node src = cities[source.name];
		Node tgt = cities[target.name];
		List<List<Node>> allPaths = new List<List<Node>>();
		List<Node> l = new List<Node>();
		l.Add(src);
		allPaths.Add(l);

		//iterate across layers/dist from root
		for (int qIndex = 0; qIndex < allPaths.Count; qIndex++)
		{
			if (qIndex > 12) //HACK nothing should take more than 12 moves...
				return -1;
			//Debug.Log("Layer: " + qIndex);
            //prepare next layer
            allPaths.Add(new List<Node>());
			//for each node in current layer
            foreach (Node n in allPaths[qIndex])
            {
                //Debug.Log("checking src: " + n.GetObj().name);
                foreach (Node newNode in n.links)
                {
					//Debug.Log("checking node: " + newNode.GetObj().name);
                    if (newNode == tgt)
                    {
                        Debug.Log("target found: " + n.GetObj().name + " from " + newNode.GetObj().name);
						Retrace(src, tgt, ref allPaths, numMoves);
                        return qIndex+1;
                    }
                    else
                    {
                        //check if l is in allPaths
                        bool uniqueNode = true;
                        foreach (var layer in allPaths)
                        {
                            foreach (Node prevNode in layer)
                            {
                                if (newNode == prevNode)
                                {
                                    uniqueNode = false;
                                    break;
                                }
                            }
                        }
                        //push uniqueNode into next layer
                        if (uniqueNode == true)
                        {
                            //check if queue available
                            allPaths[qIndex + 1].Add(newNode);
							//Debug.Log("adding unique node: " + newNode.GetObj().name);
                        }
                    }
                }
            }
        }
#if false
		while (false)
		{
			foreach (Node n in current.links)
			{
				if (n == tgt) { 
					Debug.Log("target found: " + n.GetObj().name + " from " + current.GetObj().name);
					return dist; }
				if (!q.Contains(n))
				{
					q.Enqueue(n);
					
				}
			}
			if (allPaths[qIndex].Count == 0)
			{
				Debug.Log("nodes in next layer: " + q.Count);
				dist++;
				numNodesCurLayer = q.Count;
			}
		}
#endif
        return -1;

	}
	int oldshortestpath(Node source, Node tgt, ref Queue<Node> q) 
	{
		int dist = 1; //layers
		int numNodesCurLayer = 1;
        string addedNodes = "";
		while (q.Count > 0)
		{
			var current = q.Dequeue();
			foreach (Node n in current.links)
			{
				if (n == tgt) { 
					Debug.Log("target found: " + n.GetObj().name + " from " + current.GetObj().name);
					return dist; }
				if (!q.Contains(n))
				{
					q.Enqueue(n);
					addedNodes = addedNodes + " " +n.GetObj().name;
				}
			}
			numNodesCurLayer--;
			if (numNodesCurLayer == 0)
			{
				Debug.Log("nodes in next layer: " + q.Count);
				Debug.Log(addedNodes);
				addedNodes = "";
				dist++;
				numNodesCurLayer = q.Count;
			}
		}
		return -1;
		//return FindShortestDistanceToCity(src, tgt, ref traversed);
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

		cities["Lagos"].Add(cities["Sao Paulo"]);
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
		cities["Madrid"].Add(cities["Sao Paulo"]);
		
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
