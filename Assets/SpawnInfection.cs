using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInfection : MonoBehaviour {

	public GameObject dBlue, dRed, dBlack, dYellow;
	GameObject cities;
	CityGraph cg;
	// Use this for initialization
	void Start () {
		cities = GameObject.Find("Cities");
		if (cities == null)
			Debug.Log("Can't find Cities!?!");
		cg = cities.GetComponent<CityGraph>();
		if (cg == null)
			Debug.Log("Can't find citygraph!?!");

#if false
        infectDec = GameObject.Find("infectDec").GetComponent<InfectDeck>();
		if (infectDec == null)
			Debug.Log("Can't find infectDec");
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void InfectCity(string target = "Milan", int infectCount=1)
	{
        //find location to spawn
        GameObject targetCity = null;
        foreach (Transform tCity in cities.transform)
        {
            if (tCity.gameObject.name == target)
            {
                //found location
                targetCity = tCity.gameObject;
            }
        }
		InfectCity(targetCity, infectCount);
	}
	public void InfectCity(GameObject targetCity, int infectCount=1, string type=null)
	{
        if (targetCity == null)
        {
            Debug.Log("can't find city to infect??");
            return;
        }
		
        //draw an infect card, move card to discard pile
        //infect city
        //string target = "Madrid";
		if (type == null)
            type = Cities.GetType(targetCity.name);

        GameObject diseaseType;
        if (type == "blue") diseaseType = dBlue;
        else if (type == "red") diseaseType = dRed;
        else if (type == "black") diseaseType = dBlack;
        else if (type == "yellow") diseaseType = dYellow;
        else diseaseType = dRed;

        //check number of infections already in city
        int diseaseCount = 0;
		foreach (Transform tChild in targetCity.transform)
		{
			if (tChild.gameObject.tag == "disease" 
				&& tChild.gameObject.name.Contains("disease_"+type))
			{
				diseaseCount++;
			}
		}

        bool outbreak = ((diseaseCount + infectCount) > 3);
		if ((3 - diseaseCount) < infectCount)
		{
			infectCount = 3 - diseaseCount;
		}
        
		var cityPosition = targetCity.transform.position;
		for (int i = 0; i < infectCount; i++)
		{
            //add offset to position
            float x = (float)UnityEngine.Random.Range(-30, 30);
            x = x / 3f + Mathf.Sign(x) * 20f;
            float y = (float)UnityEngine.Random.Range(-30, 30);
            y = y / 3f + Mathf.Sign(y) * 20f;
            cityPosition += new Vector3(x / 100f, y / 100f, 0);
            var newDisease = Instantiate(diseaseType, cityPosition, targetCity.transform.rotation);
            newDisease.transform.parent = targetCity.transform;
            newDisease.GetComponent<Attraction>().pullSource = targetCity;
        }
		if (outbreak && !cg.GetNode(targetCity.name).hasOutbreak)
        {
            cg.GetNode(targetCity.name).hasOutbreak = true;
			//set self as having outbreak
            //find neighbors and add infect 1 to each of them
            var neighbors = cg.GetNeighbors(targetCity.name);
            foreach (var node in neighbors)
            {
                Debug.Log("Infecting Neighbor: " + node.GetObj().name);
				InfectCity(node.GetObj(), 1, type);
            }
        }
    }
}
