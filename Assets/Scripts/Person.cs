using UnityEngine;
using System;

public class Person : MonoBehaviour {

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
			if (Vector3.Distance(CurrentDestination().position, transform.position) < 4f) {
				MoveToNext ();
			}
		}
			
		_lineRenderer.SetVertexCount (_agent.path.corners.Length);
		_lineRenderer.SetPositions (_agent.path.corners);

	}

	public void MoveToNext() {
		Debug.Log ("Moving to next");
		var i = _currentStep;
		if (i >= objects.Length - 1) {
			_currentStep = 0;
		}
		else {
			_currentStep = i + 1;
		}
		_agent.destination = CurrentDestination().position; 
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

}
