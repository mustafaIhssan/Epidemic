using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class Cities {
	public static List<List<string>> GetCityLists() {
		List<List<string>> types = new List<List<string>>();
		types.Add(blue);
		types.Add(red);
		types.Add(yellow);
		types.Add(black);
		
		return types;
	}
	public static void Shuffle(ref List<string> deck, int iterations) {
        for (int it = 0; it < iterations; it++)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                string temp = deck[i];
                int randomIndex = UnityEngine.Random.Range(i, deck.Count);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }
	}
	public static List<string> GetDeck() {
		var types = GetCityLists();
		//get all cities in one list
		List<string> cities = new List<string>();
		cities.AddRange(blue);
		cities.AddRange(red);
		cities.AddRange(yellow);
		cities.AddRange(black);

        Shuffle(ref cities, 17);
#if false
        foreach (var city in cities) {
			var type = GetType(city);
			Debug.Log(type + ": " + city);
		}
#endif
        return cities;
	}
	public static string GetType(string c) {
		List<string> typeName = new List<string>(new string[] { 
			"blue", 
			"red", 
			"yellow",
			"black"
		});

		var types = GetCityLists();
		int i = 0;
		foreach(var type in types) {
			foreach(var city in type) {
				if (city.Equals(c, StringComparison.Ordinal)) {
					return typeName[i];
				}
			}
			i++;
		}
		Debug.Log("not a valid city given!");
		return "invalid";
	}
	public static List<string> blue = new List<string>(new string[] { 
		"San Francisco",
		"Chicago",
		"Atlanta",
		"Montreal",
		"New York",
		"Washtingon",
		"London",
		"Madrid",
		"Paris",
		"Essen",
		"Milan",
		"St. Petersburg"
	});
	public static List<string> yellow = new List<string>(new string[] { 
		"Lagos",
		"Khartoum",
		"Kinshasa",
		"Johannesburg",
		"Sao Paulo",
		"Bueno Aires",
		"Santiago",
		"Lima",
		"Bogota",
		"Miami",
		"Mexico City",
		"Los Angeles"
	});
	public static List<string> black = new List<string>(new string[] { 
		"Algiers",
		"Istanbul",
		"Cairo",
		"Baghdad",
		"Moscow",
		"Tehran",
		"Karachi",
		"Riyadh",
		"Delhi",
		"Kolkata",
		"Mumbai",
		"Chennai"
	});

	public static List<string> red = new List<string>(new string[] { 
		"Bangkok",
		"Beijing",
		"Shanghai",
		"Hong Kong",
		"Seoul",
		"Tokyo",
		"Osaka",
		"Taipei",
		"Manila",
		"Ho Chi Minh City",
		"Jakarta",
		"Sydney"
	});


}

