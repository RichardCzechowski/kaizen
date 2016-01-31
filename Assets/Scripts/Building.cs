using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public enum Type {Home, Work, Play}
	public Type type = Type.Home;

	public int capacity = 3;
	public List<Person> occupants = new List<Person>();

	public Transform entryPoint;

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

	public bool Full() {
		Debug.Log ("occupnts " + OccupantsIncludingPreview ().Count + " capacity " + capacity);
		return OccupantsIncludingPreview().Count == capacity;
	}

	public Vector3 EntryPosition() {
		return entryPoint.position;
	}

	public void AddPerson(Person newPerson){
		if (OccupantsIncludingPreview ().Count < capacity) {
			occupants.Add (newPerson);
		} else {
			Debug.LogError ("Can't add person to building because it's full");
		}
	}

	public void RemovePerson(Person newPerson){
		occupants.Remove(newPerson);
	}



}