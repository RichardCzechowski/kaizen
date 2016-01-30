using UnityEngine;
using System;

public class Person : MonoBehaviour {

	public Transform[] objects;


	private Transform _currentDestination;
	private NavMeshAgent _agent;


	void Start () {
		_agent = GetComponent<NavMeshAgent>();
		_currentDestination = objects[0];
		_agent.destination = _currentDestination.position; 
	}
	
	void Update () {
		if (Vector3.Distance(_currentDestination.position, transform.position) < 1) {
			MoveToNext ();
		}
	}

	void MoveToNext() {
		var i = Array.IndexOf (objects, _currentDestination);
		if (i >= objects.Length - 1) {
			_currentDestination = objects[0];
		}
		else {
			_currentDestination = objects [i + 1];
		}
		_agent.destination = _currentDestination.position; 
	}

	void OnDrawGizmos() {
		foreach (var o in objects) {
			if (o == _currentDestination) {
				Gizmos.color = Color.green;
			}
			else {
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere (o.position, 1);
		}
	}

}
