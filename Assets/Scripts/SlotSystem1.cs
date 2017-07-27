using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSystem1 : MonoBehaviour {

	//GUI
	int[] Bet = new int[] {1, 3, 5, 9};
	int CurrentIndex = 0;
	int PlayerCash = 100000;

	//Assets
	public RectTransform TextUI1;
	public RectTransform PlayerCashValueTxt;

	// Use this for initialization
	void Start () {
		
		TextUI1.GetComponent<Text>().text = Bet[CurrentIndex].ToString();
		PlayerCashValueTxt.GetComponent<Text>().text = PlayerCash.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlusSetBetValue( int Increment = -1 ){
		CurrentIndex += Increment;
		if(CurrentIndex >= Bet.Length){ CurrentIndex = Bet.Length - 1; }
		if(CurrentIndex <= 0){ CurrentIndex = 0; }

		TextUI1.GetComponent<Text>().text = Bet[CurrentIndex].ToString();
	}

	//click on button, cycle to next bet value up to max
	//click on button, cycle to next bet value up to min
}
