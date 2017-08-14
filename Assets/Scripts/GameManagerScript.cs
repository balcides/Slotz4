using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public static int Cash;
	public static int Bet;	
	public static int Wins;
	public static int RowOneCount;	

	public RectTransform TextUICash;
	public RectTransform TextUIBet;
	public RectTransform TextUIWins;
	public RectTransform TextUIMatchCount;

	// Use this for pre-initialization
	void Awake () {
		Cash = 10000;
		Bet = 100;
		Wins = 0;
		RowOneCount = 0;

		TextUICash.GetComponent<Text>().text = Cash.ToString();
		TextUIBet.GetComponent<Text>().text = Bet.ToString();
		TextUIWins.GetComponent<Text>().text = Wins.ToString();
		TextUIMatchCount.GetComponent<Text>().text = RowOneCount.ToString();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TextUICash.GetComponent<Text>().text = Cash.ToString();
		TextUIBet.GetComponent<Text>().text = Bet.ToString();
		TextUIWins.GetComponent<Text>().text = Wins.ToString();
		TextUIMatchCount.GetComponent<Text>().text = RowOneCount.ToString();
		
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