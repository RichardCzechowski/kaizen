using UnityEngine;
using System;

public class Person : MonoBehaviour {

	public Transform[] objects;


	private Transform _currentDestination;
	private NavMeshAgent _agent;
	private LineRenderer _lineRenderer;

	void Start () {
		_agent = GetComponent<NavMeshAgent>();
		_currentDestination = objects[0];
		_agent.destination = _currentDestination.position; 
		_lineRenderer = GetComponentInChildren<LineRenderer> ();
		_lineRenderer.useWorldSpace = true;
	}
	
	void Update () {
		if (Vector3.Distance(_currentDestination.position, transform.position) < 1) {
			MoveToNext ();
		}

		_lineRenderer.SetVertexCount (_agent.path.corners.Length);
		_lineRenderer.SetPositions (_agent.path.corners);

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

	void OnDrawGizmosSelected() {
		foreach (var o in objects) {
			if (o == _currentDestination) {
				Gizmos.color = Color.green;
			}
			else {
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere (o.position, 1);
		}

		if (_agent) {
			Gizmos.color = Color.blue;
			for (var i = 0; i < _agent.path.corners.Length; i ++) {
				Gizmos.DrawWireSphere (_agent.path.corners[i], 0.5f);
				if (i < _agent.path.corners.Length - 1) {
					Gizmos.DrawLine (_agent.path.corners [i], _agent.path.corners [i + 1]);
				}
			}
		
		}
	}

}
