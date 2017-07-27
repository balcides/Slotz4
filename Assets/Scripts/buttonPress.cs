using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM = UnityEngine.SceneManagement;

public class buttonPress : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//handles all the button inputs for the game menu
	void OnMouseDown() {
		switch (transform.name){
			
			case "game1":
				print("test");
				SM.SceneManager.LoadScene("Game",SM.LoadSceneMode.Single);
				break;

			case "game2":
				print("test");
				SM.SceneManager.LoadScene("Game2",SM.LoadSceneMode.Single);
				break;

			case "home":
				print("test");
				SM.SceneManager.LoadScene("MainMenu",SM.LoadSceneMode.Single);
				break;
		}
	}
}
