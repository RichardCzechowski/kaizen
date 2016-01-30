using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
	
	public enum State {settingPath, waiting, walking};
	public State state = State.settingPath;

	public Building[] objects;

	int _currentStep = 0;
	private NavMeshAgent _agent;
	private LineRenderer _lineRenderer;

	void Start () {
		_agent = GetComponent<NavMeshAgent>();			
		_lineRenderer = GetComponentInChildren<LineRenderer> ();
		_lineRenderer.useWorldSpace = true;
		transform.position = CurrentDestination().EntryPosition();
	}

	Building CurrentDestination() {
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

	Vector3 CurrentPathEnd() {
		return _agent.path.corners [_agent.path.corners.Length - 1];
	}
	
	void Update () {

		if (CurrentDestination()) {
			if (Vector3.Distance (CurrentPathEnd (), transform.position) < 0.7f) {
				if (state == State.walking) {
					MoveToNext ();
				}
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
	
		state = State.waiting;
		StartCoroutine (wait (10));
		_agent.destination = CurrentDestination().EntryPosition(); 
	
	}

	Vector3[] GetCorners(Vector3 start, Vector3 end) {
		var segment = new NavMeshPath ();
		NavMesh.CalculatePath (start, end, NavMesh.AllAreas, segment);
		return segment.corners;
	}

	float PathLength(Vector3[] path) {
		float pathLength = 0f;
		for(var i = 1 ;i < path.Length; i++)
		{
			pathLength += Mathf.Abs(Vector3.Distance(path[i-1], path[i]));
		}
		return pathLength;
	}
		
	void UpdatePathPreview() {

		List<Vector3> path = new List<Vector3>();

		for (var i = 0; i < ObjectsSet(); i ++) {
			foreach (var point in GetCorners(objects [i].EntryPosition(), objects [i + 1].EntryPosition())) {
				path.Add (point);
			}
			if (i == objects.Length - 2) {
				foreach (var point in GetCorners(objects [i + 1].EntryPosition(), objects [0].EntryPosition())) {
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
				Gizmos.DrawWireSphere (o.EntryPosition(), 1);
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

			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere (CurrentPathEnd (), 0.7f);

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
		state = State.walking;
	}
}
