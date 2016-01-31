using UnityEngine;
using System.Collections;

public class tutorial : MonoBehaviour {

	GameObject camera;
	private Stratecam stratecam;

	public Texture2D introPerson;
	private bool hasIntroducedPerson;
	private bool showPersonGUI;


	public Texture2D introPath;
	private bool hasIntroducedPath;
	private bool showPathGUI;

	public Texture2D introPath2;
	private bool hasIntroducedPath2;
	private bool showPathGUI2;

	public Texture2D introHome;
	private bool hasIntroducedHome;
	private bool showHomeGUI;

	public Texture2D introHome2;
	private bool hasIntroducedHome2;
	private bool showHomeGUI2;

	public Texture2D introFood;
	private bool hasIntroducedFood;
	private bool showFoodGUI;

	public Texture2D introWork;
	private bool hasIntroducedWork;
	private bool showWorkGUI;

	public Texture2D introStar;
	private bool hasIntroducedStar;
	private bool showStarGUI;

	public Texture2D introUpgrade;
	private bool hasIntroducedUpgrade;
	private bool showUpgradeGUI;

	public static tutorial instance = null;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Awake (){
		instance = this;
	}

	float height = 290;
	float width = 529;

	void OnGUI() {
		if (showPersonGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introPerson);
		}
		if (showPathGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introPath);
		}
		if (showPathGUI2){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introPath2);
		}
		if (showHomeGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introHome);
		}
		if (showHomeGUI2){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introHome2);
		}
		if (showFoodGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introFood);
		}
		if (showWorkGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introWork);
		}
		if (showStarGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introStar);
		}
		if (showUpgradeGUI){
			GUI.DrawTexture(new Rect((Screen.width/2) - (width/2), Screen.height/2-(height/2), width, height), introUpgrade);
		}
	}



	// Update is called once per frame
	void Update () {

		if (!hasIntroducedPerson && GameObject.FindWithTag ("Player")) {
			kickOffIntroducePerson ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			showPersonGUI = false;
			showPathGUI = false;
			showPathGUI2 = false;
			showHomeGUI = false;
			showHomeGUI2 = false;
			showFoodGUI = false;
			showWorkGUI = false;
			showStarGUI = false;
			showUpgradeGUI = false;
			StopAllCoroutines();
			camera.GetComponent<Stratecam> ().objectToFollow = null;
			camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
			camera.GetComponent<Stratecam> ().minZoomDistance = 10;
			DayNightController.instance.paused = false;
		}
	}

	public void kickOffIntroducePerson(){
		if (!hasIntroducedPerson) {
			StartCoroutine(IntroducePerson());
			hasIntroducedPerson = true;
		}
	}


	public void kickOffIntroducePath(){
		if (!hasIntroducedPath) {
			StartCoroutine(IntroducePath());
			hasIntroducedPath = true;
		}
	}


	public void kickOffIntroduceHome(){
		if (!hasIntroducedHome) {
			StartCoroutine(IntroduceHome());
			hasIntroducedHome = true;
		}
	}


	public void kickOffIntroduceFood(){
		if (!hasIntroducedFood) {
			StartCoroutine(IntroduceFood());
			hasIntroducedFood = true;
		}
	}


	public void kickOffIntroduceWork(){
		if (!hasIntroducedWork) {
			StartCoroutine(IntroduceWork());
			hasIntroducedWork = true;
		}
	}


	public void kickOffIntroduceStar(){
		if (!hasIntroducedStar) {
			StartCoroutine(IntroduceStar());
			hasIntroducedStar = true;
		}
	}

	public void kickOffIntroduceUpgrade(){
		if (!hasIntroducedUpgrade) {
			StartCoroutine(IntroduceUpgrade());
			hasIntroducedUpgrade = true;
		}
	}


	IEnumerator IntroducePerson() {
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.FindWithTag("Player");
		yield return new WaitForSeconds (5);
		DayNightController.instance.paused = true;
		showPersonGUI = true;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		yield return new WaitForSeconds (5);
		showPersonGUI = false;
		DayNightController.instance.paused = false;
		//StartCoroutine(IntroducePath());
	}

	IEnumerator IntroducePath() {
		DayNightController.instance.paused = true;
		showPathGUI = true;
		yield return new WaitForSeconds (5);
		showPathGUI = false;

		showPathGUI2 = true;
		yield return new WaitForSeconds (5);
		showPathGUI2 = false;
		DayNightController.instance.paused = false;
	}

	IEnumerator IntroduceHome() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Home (1)");
		yield return new WaitForSeconds (1);
		showHomeGUI = true;
		yield return new WaitForSeconds (5);
		showHomeGUI = false;
		showHomeGUI2 = true;
		yield return new WaitForSeconds (5);
		showHomeGUI2 = false;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		DayNightController.instance.paused = false;
		//StartCoroutine(IntroduceFood());
	}

	IEnumerator IntroduceFood() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Noodle Shop");
		yield return new WaitForSeconds (1);
		showFoodGUI = true;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		yield return new WaitForSeconds (5);
		showFoodGUI = false;
		DayNightController.instance.paused = false;
		//StartCoroutine(IntroduceWork());
	}

	IEnumerator IntroduceWork() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Star Factory");
		yield return new WaitForSeconds (1);
		showWorkGUI = true;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		yield return new WaitForSeconds (5);
		showWorkGUI = false;
		DayNightController.instance.paused = false;
		//StartCoroutine(IntroduceStar());
	}

	IEnumerator IntroduceStar() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("StarResource");
		yield return new WaitForSeconds (1);
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		showStarGUI = true;
		yield return new WaitForSeconds (5);
		showStarGUI = false;
		DayNightController.instance.paused = false;
		kickOffIntroduceUpgrade ();
	}

	IEnumerator IntroduceUpgrade() {
		DayNightController.instance.paused = true;
		showUpgradeGUI = true;
		yield return new WaitForSeconds (5);
		showUpgradeGUI = false;
		DayNightController.instance.paused = true;
		DayNightController.instance.paused = false;
	}
}
