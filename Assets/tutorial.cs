using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class Tutorial : MonoBehaviour {

	[System.Serializable]
	public class Page {
		public string name;
		public Texture2D card;
		public GameObject zoomToObject;
		public bool pausesWhileZooming = true;
		public float delay = 0;

		[System.NonSerialized]
		public bool used;
	}

	public Page[] pages;
	public static Tutorial instance = null;

	Blur _blur;
	Stratecam _cam;

	void Start () {
		_blur = Camera.main.GetComponent<Blur> ();
		_cam = Camera.main.GetComponent<Stratecam> ();
	}

	void Awake (){
		instance = this;
	}

	float height = 290;
	float width = 529;

	public bool Active() {
		return _currentPage != null;
	}

	Page _currentPage = null;
	Queue<Page> _queue = new Queue<Page>();
	public void ShowPage(Page page) {
		_queue.Enqueue (page);
		if (_currentPage == null) {
			Advance ();
		}
	}

	public void ShowPage(string pageName) {
		foreach (var p in pages) {
			if (p.name == pageName) {
				if (!p.used) {
					ShowPage (p);
				} else {
					Debug.Log ("Tutorial page " + pageName + " already used.");
				}
				return;
			}
		}
		Debug.LogError ("Tutorial page " + pageName + " not found.");
	}

	public bool UsedPage(string pageName) {
		foreach (var p in pages) {
			if (p.name == pageName) {
				return p.used;
			}
		}
		Debug.LogError ("Tutorial page " + pageName + " not found.");
		return false;
	}

	void Advance () {
		_currentPage = _queue.Dequeue ();
		DayNightController.instance.pauseNoPreview = true;
		DisableInput ();
		_currentPage.used = true;
		StartCoroutine (FollowAndDelay ());
	}

	void Dismiss() {
		_currentPage = null;
		DayNightController.instance.pauseNoPreview = false;
		EnableInput ();
		_cam.objectToFollow = null;
		_cam.maxZoomDistance = 40;
		_cam.minZoomDistance = 10;
		_displaying = false;
		StopAllCoroutines ();
	}

	void OnGUI() {
		if (_displaying){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), _currentPage.card);
		}
		_blur.enabled = _displaying;
	}

	void DisableInput() {
		_cam.useKeyboardInput = false;
		_cam.useMouseInput = false;
	}

	void EnableInput () {
		_cam.useKeyboardInput = true;
		_cam.useMouseInput = true;
	}


	bool hasIntroducedPerson = false;

	void Update () {

		if (!hasIntroducedPerson && GameObject.FindWithTag ("Player")) {
			pages [0].zoomToObject = GameObject.FindWithTag ("Player");
			pages [0].delay = 8;
			ShowPage ("people");
			hasIntroducedPerson = true;
		}

		if (_displaying && (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonUp(0))) {
			Dismiss ();
		}

		if (DayNightController.instance.daysElapsed > 0 && UsedPage("circles")) {


			Tutorial.instance.ShowPage("abouthouses");

			if (DayNightController.instance.TimeOfDayActual() > 0.25f) {
				Tutorial.instance.ShowPage("aboutfood");
			}

			if (DayNightController.instance.TimeOfDayActual() > 0.9f) {
				Tutorial.instance.ShowPage("about work");

			}

		}

	}

	bool _displaying = false;
	IEnumerator FollowAndDelay() {
		if (!_currentPage.pausesWhileZooming) {
			DayNightController.instance.pauseNoPreview = false;
		}
		DisableInput ();
		if (_currentPage.zoomToObject != null) {
			_cam.maxZoomDistance = 11;
			_cam.minZoomDistance = 10;
			_cam.objectToFollow = _currentPage.zoomToObject;
		}
		yield return new WaitForSeconds (_currentPage.delay);
		DayNightController.instance.pauseNoPreview = true;
		_displaying = true;
	}
		
}
