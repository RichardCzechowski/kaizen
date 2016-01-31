using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
	
	public enum State {settingPath, waiting, walking, readyToMove};
	State state = State.settingPath;

	public Texture2D portrait;
	public Color color = Color.black;
	public Color[] possibleColors;

	public Building home;
	Building[] objects;

	public MeshRenderer[] recolorableClothes;

	private NavMeshAgent _agent;
	private LineRenderer _lineRenderer;

	public bool selected = false;

	private float nextShiftStart;
	public bool paused = false;
	bool timeIsPaused;

	static int peopleCreated = 0;

	public int mood = 0;

	void Start () {
		timeIsPaused = DayNightController.instance.paused;
		ClearPaths ();
		objects [0] = home;

		_agent = GetComponent<NavMeshAgent>();			
		_lineRenderer = GetComponentInChildren<LineRenderer> ();
		_lineRenderer.useWorldSpace = true;
		transform.position = CurrentDestination().EntryPosition();


		color = possibleColors [peopleCreated % possibleColors.Length];
		peopleCreated++;

		foreach (var renderer in recolorableClothes) {
			var mat = new Material(renderer.sharedMaterial);
			mat.color = color;
			renderer.material = mat;
		}

		var lineMat = new Material(_lineRenderer.sharedMaterial);
		lineMat.color = color;
		_lineRenderer.material = lineMat;

	}

	public static Person[] All() {
		return FindObjectsOfType<Person>();
	}

	public Building BuildingForShift(int shift) {
		return objects [shift];
	}

	Building CurrentDestination() {
		if (objects == null) {
			return null;
		}
		return objects [DayNightController.instance.CurrentShift()];
	}

	Building PreviousDestination() {
		if (objects == null) {
			return null;
		}
		return objects [DayNightController.instance.PreviousShift()];
	}

	public Building[] Buildings() {
		return objects;
	}

	public void ClearPaths() {
		objects = new Building[DayNightController.instance.NumberOfShifts()];
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
		return ObjectsSet() == DayNightController.instance.NumberOfShifts();
	}

	Vector3 CurrentPathEnd() {
		if (_agent.path.corners.Length != 0) {
			return _agent.path.corners [_agent.path.corners.Length - 1];
		} else {
			return Vector3.zero;
		}
	}
	
	void Update () {
		if (DayNightController.instance.paused != timeIsPaused) {
			if (DayNightController.instance.paused == true) {
				Pause ();
			} else if (DayNightController.instance.paused == false) {
				Resume ();
			}
		}

		if (CurrentDestination() && state != State.settingPath) {
			if (Vector3.Distance (CurrentPathEnd (), transform.position) < 0.7f && state != State.waiting) {
				if (DayNightController.instance.TimeOfDayActual () + DayNightController.instance.daysElapsed < nextShiftStart) {
					SetState (State.waiting);
				}
			}
			else if (DayNightController.instance.TimeOfDayActual() + DayNightController.instance.daysElapsed >= nextShiftStart) {
				SetState (State.walking);
			}
		}

		if (selected) {
			UpdatePathPreview ();
			_lineRenderer.gameObject.SetActive (true);
		} else {
			_lineRenderer.gameObject.SetActive (false);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			paused = !paused;
			if (paused) {
				Pause ();
			} else {
				Resume ();
			}
		}
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

		if (objects != null) {
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

	private Vector3 lastAgentVelocity;
	private NavMeshPath lastAgentPath;
	public void Pause (){
		paused = true;
		lastAgentVelocity = _agent.velocity;
		lastAgentPath = _agent.path;
		_agent.velocity = Vector3.zero;
		_agent.ResetPath();
	}

	public void Resume (){
		paused = false;
		_agent.velocity = lastAgentVelocity;
		if (lastAgentPath != null) {
			_agent.SetPath (lastAgentPath);
		}
	}



	public void SetWaitTime(){
		nextShiftStart = DayNightController.instance.ShiftStartHour(DayNightController.instance.CurrentShift() + 1) / 24F + DayNightController.instance.daysElapsed;
	}

	void HideMesh() {
		foreach (var renderer in GetComponentsInChildren<MeshRenderer> ()) {
			renderer.enabled = false;
		}
	}

	void ShowMesh() {
		foreach (var renderer in GetComponentsInChildren<MeshRenderer> ()) {
			renderer.enabled = true;
		}
	}

	// Fo calculating mood
	float start;
	float end;

	///////////////////// STATE MACHINE
	private void OnEnterState(State state){
		switch(state){
		case State.settingPath:
			// Don't move until path is set
			//SetState(State.waiting);
			break;
		case State.waiting:
			//Hangout for a length of time
			this.GetComponent<NavMeshAgent> ().radius = .01F;
			this.GetComponent<Collider> ().isTrigger = true;
			CurrentDestination ().AddPerson (this);
			start = DayNightController.instance.TimeOfDayActual ();
			HideMesh ();
			break;
		case State.walking:
			ShowMesh ();
			SetWaitTime();
			_agent.destination = CurrentDestination().EntryPosition(); 
			// Animate walking
			break;
		case State.readyToMove:
			// Animate walking
			break;
		}
	}
		
	private void OnExitState(State state){
		switch(state){
		case State.settingPath:
			// Don't move until path is set
			break;
		case State.waiting:
			// Hangout for a length of time
			end = DayNightController.instance.TimeOfDayActual ();
			mood += PreviousDestination ().ComputeScore ();
			PreviousDestination().RemovePerson(this);
			this.GetComponent<NavMeshAgent>().radius = .5F;
			this.GetComponent<Collider>().isTrigger = false;
			break;
		case State.walking:
			// Animate walking
			break;
		case State.readyToMove:
			// Animate walking
			break;
		}
	}

	public void SetState (State newState) {
		OnExitState (state);
		state = newState;
		OnEnterState (newState);
	}
}
