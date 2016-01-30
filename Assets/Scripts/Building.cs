using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public enum Type {Home, Work, Play}
	public Type type = Type.Home;

	public int capacity = 3;
	public Person[] occupants;

	public Transform entryPoint;

	public bool Full() {
		return occupants.Length == capacity;
	}

	public Vector3 EntryPosition() {
		return entryPoint.position;
	}

}
