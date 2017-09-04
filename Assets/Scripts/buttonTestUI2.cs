using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM = UnityEngine.SceneManagement;

public class buttonTestUI2 : MonoBehaviour {

	//Assets
	public TextMesh testUI2;
	public string[] buttonMessage = {"", "Button Pressed"};

	// Use this for initialization
	void Start () {
		print(buttonMessage[1]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	public void OnMouseDown() {
		
		switch (transform.name){
			case "button2":
				SM.SceneManager.LoadScene("UIOneDemo", SM.LoadSceneMode.Single);
				break;
			case "button3":
				SM.SceneManager.LoadScene("UITestMenuDemo", SM.LoadSceneMode.Single);
				break;
			default:
				print("test");
				testUI2.GetComponent<TextMesh>().text = buttonMessage[1];
				break;
		}
	}

	public void OnMouseUp(){
			testUI2.GetComponent<TextMesh>().text = buttonMessage[0];
	}
}
