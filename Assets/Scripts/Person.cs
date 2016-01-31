using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
	
	public enum State {settingPath, waiting, walking, readyToMove};
	State state = State.settingPath;


	[HideInInspector]
	public Texture2D portrait;
	public Color color = Color.black;

	public Texture2D[] possiblePortraits;
	public Texture2D[] possibleBaseTextures;
	public Texture2D[] possibleClothingTextures;
	public Color[] possibleColors;

	Building[] objects;

	public MeshRenderer[] baseRenderer;
	public MeshRenderer[] recolorableClothes;

	private NavMeshAgent _agent;
	private LineRenderer _lineRenderer;

	public bool selected = false;

	private float nextShiftStart;
	public bool paused = false;
	bool timeIsPaused;

	static int peopleCreated = 0;

	public int mood = 0;
	public Status icon;
	public enum Status {lonely, tired, bored, fulfilled, rested, excited, noStatus};

	public Sprite lonely;
	public Sprite tired;
	public Sprite bored;
	public Sprite fulfilled;
	public Sprite rested;
	public Sprite excited;
	private Animation[] anim;

	void Start () {
		timeIsPaused = DayNightController.instance.paused;
		ClearPaths ();

		_agent = GetComponent<NavMeshAgent>();			
		_lineRenderer = GetComponentInChildren<LineRenderer> ();
		_lineRenderer.useWorldSpace = true;

		// Don't always start with the same color etc.
		peopleCreated += UnityEngine.Random.Range (0, 10);

		color = possibleColors [peopleCreated % possibleColors.Length];
		peopleCreated++;

		var clothingTexture = possibleClothingTextures [peopleCreated % possibleClothingTextures.Length];
		var baseTexture = possibleBaseTextures [peopleCreated % possibleBaseTextures.Length];
		portrait = possiblePortraits [peopleCreated % possiblePortraits.Length];

		foreach (var renderer in recolorableClothes) {
			var mat = new Material(renderer.sharedMaterial);
			mat.color = color;
			mat.mainTexture = clothingTexture;
			renderer.material = mat;
		}

		foreach (var renderer in baseRenderer) {
			var mat = new Material(renderer.sharedMaterial);
			mat.mainTexture = baseTexture;
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

	public Vector3 bullpenLocation;
	
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

		if (state == State.settingPath) {
			_agent.SetDestination (bullpenLocation);
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

	Sprite iconSprite;
	private void ShowIcon(){
		// Status {lonely, tired, bored, fulfilled, rested, excited, noStatus};
		switch (icon) {
		case Status.lonely:
			iconSprite = lonely;
			break;
		case Status.tired:
			iconSprite = tired;
			break;
		case Status.bored:
			iconSprite = bored;
			break;
		case Status.fulfilled:
			iconSprite = fulfilled;
			break;
		case Status.rested:
			iconSprite = rested;
			break;
		case Status.excited:
			iconSprite = excited;
			break;
		}
		
		this.GetComponentInChildren<SpriteRenderer> ().sprite = iconSprite;
		statusIcon.instance.Popup();
	}

	private void HideIcon(){
		//Animation[] anim = this.GetComponentsInChildren<Animation>();
		statusIcon.instance.Popdown();
	}

	// Fo calculating mood
	float start;
	float end;

	int home;
	int play;
	int work;
	private void CheckMood(){
		// play: lonely, home: tired, work: bored
		// play: fulfilled, home: rested, work: excited
		var i = 0;
		for (i = 0; i < objects.Length; i++) {
			if (objects [i].type == Building.Type.Home) {
				home++;
			}else if (objects [i].type == Building.Type.Play) {
				play++;
			}else if (objects [i].type == Building.Type.Work) {
				work++;
			}
		}
		if (home == 0) {
			icon = Status.tired;
			mood--;
		} else if (play == 0) {
			icon = Status.lonely;
			if (GameObject.FindGameObjectsWithTag("Player").Length > 1){
				mood -= 3;
			}
		} else if (work == 0) {
			icon = Status.bored;
			mood--;
		} else {
			icon = PreviousDestination ().ComputeStatus (mood);
		}
		Debug.Log (icon);
	}

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
			HideIcon ();
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
			mood += PreviousDestination ().ComputeScore (mood);
			CheckMood ();
			PreviousDestination ().RemovePerson (this);
			this.GetComponent<NavMeshAgent> ().radius = .5F;
			this.GetComponent<Collider> ().isTrigger = false;
			ShowIcon ();
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
