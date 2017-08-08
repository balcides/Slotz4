using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public int Cash;
	public int Bet;	
	public RectTransform TextUICash;
	public RectTransform TextUIBet;

	// Use this for pre-initialization
	void Awake () {
		Cash = 10000;
		Bet = 100;

		TextUICash.GetComponent<Text>().text = Cash.ToString();
		TextUIBet.GetComponent<Text>().text = Bet.ToString();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextUICash.GetComponent<Text>().text = Cash.ToString();
		TextUIBet.GetComponent<Text>().text = Bet.ToString();
		
	}

	//Withdrawal on spin
	public void PayForSpin(){
		Cash = Cash - Bet;
		if(Cash <= 0){ Cash = 0;}
	}
}


// At start show cash
// Button pressed
// Money taken out
// if theres money, roll, no