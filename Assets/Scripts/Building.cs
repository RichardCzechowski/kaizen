using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public enum Type {Home, Work, Play}
	public Type type = Type.Home;

	public int capacity = 3;
	public int maxCapacity = 3;
	public List<Person> occupants = new List<Person>();

	public Transform entryPoint;
	public Transform resourcePoint;

	public int stars;

	public int upgradeCost = 5;

	public GameObject resourcePrefab;
	List<GameObject> resourceItems = new List<GameObject>();

	public GameObject[] onBeforePurchase;
	public GameObject[] onAfterPurchase;

	void Start(){
	}

	public List<Person> OccupantsIncludingPreview() {
		if (DayNightController.instance.paused) {
			List<Person> theseOccupants = new List<Person> ();
			foreach (var person in Person.All()) {
				if (person.BuildingForShift(DayNightController.instance.CurrentShift()) == this) {
					theseOccupants.Add (person);
				}
			}
			return theseOccupants;
		} else {
			return occupants;
		}
	}
		
	void Update() {

		if (resourcePrefab) {
			if (resourceItems.Count > stars) {
				var item = resourceItems [0];
				Destroy (item);
				resourceItems.RemoveAt (0);
			} else if (resourceItems.Count < stars) {
				resourceItems.Add(Instantiate(resourcePrefab, resourcePoint.position, resourcePoint.rotation) as GameObject);
			}
		}

		if (capacity == 0) {
			foreach (var obj in onBeforePurchase) {
				obj.SetActive (true);
			}
			foreach (var obj in onAfterPurchase) {
				obj.SetActive (false);
			}
		} else {
			foreach (var obj in onBeforePurchase) {
				obj.SetActive (false);
			}
			foreach (var obj in onAfterPurchase) {
				obj.SetActive (true);
			}
		}

	}

	public static Building[] All() {
		return FindObjectsOfType<Building>();
	}

	public bool CanUpgrade() {
		return scoreManager.instance.currentScore >= upgradeCost && capacity < maxCapacity;
	}

	public void Upgrade() {
		scoreManager.instance.DecrementPoints (upgradeCost);
		capacity++;
	}

	public bool Full() {
		return OccupantsIncludingPreview().Count == capacity;
	}

	public Vector3 EntryPosition() {
		return entryPoint.position;
	}

	public void AddPerson(Person newPerson){
		if (occupants.Count < capacity) {
			occupants.Add (newPerson);
		} else {
			Debug.LogError ("Can't add person to building because it's full");
		}
	}

	public void RemovePerson(Person newPerson){
		occupants.Remove(newPerson);
	}

	public int ComputeScore(int mood){
		// If it's a home, check if it's unoccupied, then add a point if so
		// If it's a play zone, check for other people. Other people are fun!
		// If it's work, we zero out the mood of the character and turn all that fun into stars!
		switch(type){
		case Type.Home:
			if (occupants.Count == 1) {
				return 1;
			} else {
				return -occupants.Count;
			}
		case Type.Play:
			if (occupants.Count > 1) {
				return occupants.Count;
			}else {
				return -3;
			}
		case Type.Work:
			if (mood > 0) {
				scoreManager.instance.AddPoints(mood);
				stars += mood;
				return -mood;
			}
			break;
		}
		return 0;
	}
	public Person.Status ComputeStatus(int mood){
		// If it's a home, check if it's unoccupied, then add a point if so
		// If it's a play zone, check for other people. Other people are fun!
		// If it's work, we zero out the mood of the character and turn all that fun into stars!
		switch(type){
		case Type.Home:
			if (occupants.Count == 1) {
				return Person.Status.rested;
			} else {
				return Person.Status.tired;
			}
		case Type.Play:
			if (occupants.Count > 1) {
				return Person.Status.fulfilled;
			}else {
				return Person.Status.lonely;
			}
		case Type.Work:
			if (mood > 0) {
				return Person.Status.excited;
			}else{
				return Person.Status.bored;
			}
		}
		return Person.Status.noStatus;
	}

}