using UnityEngine;
using System.Collections;

public class tutorial : MonoBehaviour {

	GameObject camera;
	private Stratecam stratecam;

	public Texture2D introPerson;
	private bool hasIntroducedPerson;
	private bool showPersonGUI;


	public Texture2D introPath;
	private bool showPathGUI;

	public Texture2D introPath2;
	private bool showPathGUI2;

	public Texture2D introHome;
	private bool showHomeGUI;

	public Texture2D introHome2;
	private bool showHomeGUI2;

	public Texture2D introFood;
	private bool showFoodGUI;

	public Texture2D introWork;
	private bool showWorkGUI;

	public Texture2D introStar;
	private bool showStarGUI;

	public Texture2D introUpgrade;
	private bool showUpgradeGUI;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}


	void OnGUI() {
		if (showPersonGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introPerson);
		}
		if (showPathGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introPath);
		}
		if (showPathGUI2){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introPath2);
		}
		if (showHomeGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introHome);
		}
		if (showHomeGUI2){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introHome2);
		}
		if (showFoodGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introFood);
		}
		if (showWorkGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introWork);
		}
		if (showStarGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introStar);
		}
		if (showUpgradeGUI){
			GUI.DrawTexture(new Rect(10, 10, 529, 290), introUpgrade);
		}
	}



	// Update is called once per frame
	void Update () {

		if (GameObject.FindWithTag ("Player") && !hasIntroducedPerson) {
			StartCoroutine(IntroducePerson());
			hasIntroducedPerson = true;
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
		StartCoroutine(IntroducePath());
	}

	IEnumerator IntroducePath() {
		DayNightController.instance.paused = true;
		showPathGUI = true;
		yield return new WaitForSeconds (5);
		showPathGUI = false;

		showPathGUI2 = true;
		yield return new WaitForSeconds (5);
		showPathGUI2 = false;
		StartCoroutine (IntroduceHouse ());
	}

	IEnumerator IntroduceHouse() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Home (1)");
		yield return new WaitForSeconds (2);
		showHomeGUI = true;
		yield return new WaitForSeconds (5);
		showHomeGUI = false;
		showHomeGUI2 = true;
		yield return new WaitForSeconds (5);
		showHomeGUI2 = false;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		StartCoroutine(IntroduceFood());
	}

	IEnumerator IntroduceFood() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Noodle Shop");
		yield return new WaitForSeconds (2);
		showFoodGUI = true;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		yield return new WaitForSeconds (5);
		showFoodGUI = false;
		StartCoroutine(IntroduceWork());
	}

	IEnumerator IntroduceWork() {
		DayNightController.instance.paused = true;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 11;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		camera.GetComponent<Stratecam> ().objectToFollow = GameObject.Find("Star Factory");
		yield return new WaitForSeconds (2);
		showWorkGUI = true;
		camera.GetComponent<Stratecam> ().objectToFollow = null;
		camera.GetComponent<Stratecam> ().maxZoomDistance = 40;
		camera.GetComponent<Stratecam> ().minZoomDistance = 10;
		yield return new WaitForSeconds (5);
		showWorkGUI = false;
		StartCoroutine(IntroduceStar());
	}

	IEnumerator IntroduceStar() {
		DayNightController.instance.paused = true;
		showStarGUI = true;
		yield return new WaitForSeconds (5);
		showStarGUI = false;
		StartCoroutine (IntroduceUpgrade ());
	}

	IEnumerator IntroduceUpgrade() {
		DayNightController.instance.paused = true;
		showUpgradeGUI = true;
		yield return new WaitForSeconds (5);
		showUpgradeGUI = false;
		DayNightController.instance.paused = true;
	}
}
