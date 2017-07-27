using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SM = UnityEngine.SceneManagement;

public class buttonTestUI1 : MonoBehaviour {

	public RectTransform TextUI1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void UpdateTestText(string enter = ""){
		print("this");
		TextUI1.GetComponent<Text>().text = enter;
	}

	public void LoadSceneTwo(){
		SM.SceneManager.LoadScene("UITwo", SM.LoadSceneMode.Single);
	}

	public void LoadSceneName(string sceneName){
		SM.SceneManager.LoadScene(sceneName, SM.LoadSceneMode.Single);
	}
		
}