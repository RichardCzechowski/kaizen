using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public enum Type {Home, Work, Play}
	public Type type = Type.Home;

	public int capacity = 3;
	public List<Person> occupants = new List<Person>();
	private int numOfOccupants = 0;

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
		return numOfOccupants == capacity;
	}

	public Vector3 EntryPosition() {
		return entryPoint.position;
	}

	public void AddPerson(Person newPerson){
		if (numOfOccupants < capacity) {
			occupants.Add (newPerson);
			numOfOccupants++;
		}
	}

	public void RemovePerson(Person newPerson){
		occupants.Remove(newPerson);
		numOfOccupants--;
	}



}