using UnityEngine;
using System;
using System.Collections;

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

	bool HasCompletePath() {
		return objects [1] && objects [2] && objects [3] && objects [4] && objects [5];
	}
	
	void Update () {

		if (CurrentDestination()) {
			if (Vector3.Distance(CurrentDestination().position, transform.position) < 3f) {
				MoveToNext ();
			}
		}
			
		_lineRenderer.SetVertexCount (_agent.path.corners.Length);
		_lineRenderer.SetPositions (_agent.path.corners);

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
		UpdatePathPreview ();
	
	}

	NavMeshPath[] pathPreviewSegments = new NavMeshPath[6];

	void UpdatePathPreview() {
		for (var i = 0; i < objects.Length; i ++) {
			if (i < objects.Length - 1) {
				if (objects [i + 1] && objects [i]) {
					var segment = new NavMeshPath ();

					NavMesh.CalculatePath (objects [i].position, objects [i + 1].position, NavMesh.AllAreas, segment);
					pathPreviewSegments [i] = segment;
				}
			}
		}
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
