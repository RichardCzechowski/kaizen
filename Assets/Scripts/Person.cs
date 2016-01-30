using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
	
	public enum State {settingPath, waiting, walking};
	public State state = State.settingPath;

	public Transform[] objects;

	int _currentStep = 0;
	private NavMeshAgent _agent;
	private LineRenderer _lineRenderer;

	void Start () {
		_agent = GetComponent<NavMeshAgent>();			
		_lineRenderer = GetComponentInChildren<LineRenderer> ();
		_lineRenderer.useWorldSpace = true;
		transform.position = CurrentDestination().position;
	}

	Transform CurrentDestination() {
		return objects [_currentStep];
	}

	int ObjectsSet() {
		var i = 0;
		for (i = 0; i < objects.Length; i++) {
			if (!objects [i]) {
				return i - 1;
			}
		}
		return i - 1;
	}

	bool HasCompletePath() {
		return ObjectsSet() == 6;
	}
	
	void Update () {

		if (CurrentDestination()) {
			if (Vector3.Distance(CurrentDestination().position, transform.position) < 3f) {
				MoveToNext ();
			}
		}
			
		UpdatePathPreview ();

	}

	public void MoveToNext() {

		if (_currentStep >= objects.Length - 1) {
			_currentStep = 0;
		} else if (objects [_currentStep + 1]) {
			_currentStep = _currentStep + 1;
		} else {
			return;
		}
			
		_agent.destination = CurrentDestination().position; 
	
	}

	Vector3[] GetCorners(Vector3 start, Vector3 end) {
		var segment = new NavMeshPath ();
		NavMesh.CalculatePath (start, end, NavMesh.AllAreas, segment);
		return segment.corners;
	}


	void UpdatePathPreview() {

		List<Vector3> path = new List<Vector3>();

		for (var i = 0; i < ObjectsSet(); i ++) {
			foreach (var point in GetCorners(objects [i].position, objects [i + 1].position)) {
				path.Add (point);
			}
			if (i == objects.Length - 2) {
				foreach (var point in GetCorners(objects [i + 1].position, objects [0].position)) {
					path.Add (point);
				}
			}
		}

		_lineRenderer.SetVertexCount (path.Count);
		_lineRenderer.SetPositions (path.ToArray());

	}

	void OnDrawGizmosSelected() {
		foreach (var o in objects) {
			if (o) {
				if (o == CurrentDestination()) {
					Gizmos.color = Color.green;
				}
				else {
					Gizmos.color = Color.red;
				}
				Gizmos.DrawWireSphere (o.position, 1);
			}
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

	///////////////////// STATE MACHINE

	public void setState (State newState) {
		state = newState;
		switch(state){
		case State.settingPath:
			// Don't move until path is set
			Debug.Log("settingPath");
			setState(State.waiting);
			break;
		case State.waiting:
			//Hangout for a length of time
			Debug.Log("waiting");
			StartCoroutine(wait(10));
			break;
		case State.walking:
			// Animate walking
			break;
		}
	}

	IEnumerator wait(int time) {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(time);
		Debug.Log("After Waiting 2 Seconds");
	}
}
