﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 
/// 	SpinSlotsScript.cs
/// 	Experiment to get tiling tiles working a basic 
/// 	fundamental in gaming needed for multiple games.
/// 
/// </summary>

public class SpinSlotsScript : MonoBehaviour {

	public Transform TilePrefab;
	public Transform TileFirstRowParent;
	public Transform TileMasterParent;
	public int TileXCount;
	public int TileYCount;
	public int Unit;
	public int SpinCountBeforeStop;
	public bool EnableCloneTileTest;
	public float UnitSpeed;
	public Material[] TileTex;


	//Pre-internal
	Vector3 TileFirstRowParentStart;
	float UnitSpeedStart;
	float[] BottomLinez;
	float[] BottomLineOffsetz;
	float[] EndTileOffsetz;
	float[] UnitSpeedz;
	int[] SpinCountBeforeStopz;

	//Internal
	bool EnableSpinSlotTest;
	bool disableSpinButton;
	int SpinCountOffset;
	int[] ChildCountz;
	int[] SpinCountz;
	Transform FirstRowX;
	Transform[] YRow;
	Transform[] EndTilez;
	float[] RowTopz;
	Vector3[,] TileXYPos;
	int TileRange;

	//get an array to store all the y parents
	//then a series of arrays for the x's
	void Awake(){
		TileXCount = 6;
		TileYCount = 9;

		//Unit
		Unit = 2000; 
		UnitSpeed = 10;					//10 too fast, 5 too slow

		UnitSpeedStart = UnitSpeed;
		SpinCountBeforeStop = 30;

		TileRange = TileTex.Length;
		//TileRange = 2;

		TileFirstRowParent = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;

		YRow = new Transform[TileYCount];
		EndTilez = new Transform[TileYCount];
		EndTileOffsetz = new float[TileYCount];
		BottomLinez = new float[TileYCount];
		BottomLineOffsetz = new float[TileYCount];
		RowTopz = new float[TileYCount];
		SpinCountz = new int[TileYCount];
		ChildCountz = new int[TileYCount];
		SpinCountBeforeStopz = new int[TileYCount];
		UnitSpeedz = new float[TileYCount];
		//TileTex = new Material[6];
		if(EnableCloneTileTest){	CreateTileGridTransform();	}
	}

	// Use this for initialization
	void Start () {
		
		//Order Matters	
		for (int i = 0; i < TileYCount; i++){ BottomLinez[i] =  TileFirstRowParent.position.y - (Unit * 2); } 						// fill bottom lines
		for (int i = 0; i < TileYCount; i++){ RowTopz[i] = TileFirstRowParent.transform.GetChild(TileXCount - 1).position.y; } 		// fill row top lines

		TileFirstRowParentStart = TileFirstRowParent.position;
		TileXYPos = new Vector3[TileXCount,TileYCount];
		disableSpinButton = false;
		GetTileXYPositions();

		for (int i = 0; i < TileYCount; i++){ SpinCountz[i] = 1; } 
		for (int i = 0; i < TileYCount; i++){ ChildCountz[i] = 0; } 
		for (int i = 0; i < TileYCount; i++){ UnitSpeedz[i] = UnitSpeedStart; } 
		for (int i = 0; i < TileYCount; i++){ 
			int divisor = Mathf.RoundToInt(SpinCountBeforeStop/TileYCount);
			SpinCountBeforeStopz[i] = SpinCountBeforeStop + (divisor * i); 
		}
		//int[] rowOfNumz = new int[TileYCount * TileXCount];
		EnableSpinSlotTest = false;
		for (int i = 0; i < TileYCount; i++){ RandomizeTileIcons();}

		//for (int i = 0; i < (rowOfNumz.Length); i++){ 	rowOfNumz[i] = Randomizer(0,10);	} 
		//Debug.Log("rowOfNumz = " +String.Join("", new List<int>(rowOfNumz).ConvertAll(i => i.ToString()).ToArray()));
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableSpinSlotTest){	
			for (int i = 0; i < TileYCount; i++){ 	SpinSlotTest(i);  }	 //runs the actual slots
		}
	}

	//Creates Tile Transform Grid
	public void CreateTileGridTransform(){
		Transform XClone;
		Transform YClone;

		TileFirstRowParent.parent = TileMasterParent;

		//Creates the first row X of tiles
		for (int i = 0; i < TileXCount; i++){
			XClone = Instantiate(TilePrefab, new Vector3(2000, 2000 + (4000 * i), 100), Quaternion.Euler(new Vector3(90, 0, 0)));
			XClone.name = XClone.name + i;
			XClone.parent = TileFirstRowParent; //tiles parented to first row only, itll get duped during Y
		}

		//dupe to Y Axis
		for (int i = 1; i < TileYCount; i++){
			YClone = Instantiate(TileFirstRowParent, new Vector3((4000 * i), 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
			YClone.name = YClone.name + i;
			YClone.parent = TileMasterParent;
		}
	}


	//Core Spin slot mechanic
	public void SpinSlotTest(int i = 0){
		//wonder if better to use the loop in this or outside of this?
		//take ROW Y and MOVE IT down by 1 UNIT (whatever that is (Target is one unit down from current pos)
		float Step = UnitSpeedz[i] * Time.deltaTime * (Unit * 2);
		YRow[i].position = Vector3.MoveTowards(YRow[i].position,new Vector3(YRow[i].position.x ,BottomLinez[i],YRow[i].position.z), Step);

		//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
		EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);

		//RowTop - top of the row is the last tile position y plus 1 unit, get length of row
		BottomLineOffsetz[i] = BottomLinez[i] * 0.5f;			//bottom line is tile parent position
		EndTileOffsetz[i] = EndTilez[i].position.y * (SpinCountz[i]);

		//When the tiles move down 1 unit
		if(EndTileOffsetz[i] == BottomLineOffsetz[i]){
			EndTilez[i].GetComponent<MeshRenderer>().material = TileTex[Randomizer(0, TileRange)];
			EndTilez[i].position = new Vector3(EndTilez[i].position.x, RowTopz[i], EndTilez[i].position.z);

			//Reset spin
			if(ChildCountz[i] < (TileXCount - 1)){ 	ChildCountz[i]++; }
			else{	ChildCountz[i] = 0;  }

			EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);
			BottomLinez[i] = YRow[i].position.y - (Unit * 2);

			if(SpinCountz[i] >= SpinCountBeforeStopz[i]){

				//spin stops, reset
				if( UnitSpeedz[i] < 10 ){   
					UnitSpeedz[i] = 0;	
					if(i == TileYCount - 1){ 
						EnableSpinSlotTest = false; 
						disableSpinButton = false;
					}
				}
				else{ UnitSpeedz[i] = UnitSpeedz[i] - (UnitSpeedz[i] * 0.5f);	// spin keeps on spinnin' 
				}
			}
			else{	}
			SpinCountz[i]++;
		}
	}


	//sets enabled the anime row test and resets the spin, it's what the button calls
	public void RunSlotSpin(){
		if (disableSpinButton){}
		else{
			print("SPIN BUTTON PRESSED========================================");
			ResetVarsBeforeSpin(YRow);
		}
	}


	//resets vars before spin
	void ResetVarsBeforeSpin(Transform[] slotRow){
		for(int i = 0; i < TileYCount; i++){
			EnableSpinSlotTest = true;
			UnitSpeed = UnitSpeedStart;
			UnitSpeedz[i] = UnitSpeedStart;
			SpinCountz[i] = 1; 
			ChildCountz[i] = 0; 
			slotRow[i].position = new Vector3(slotRow[i].position.x, TileFirstRowParentStart.y, slotRow[i].position.z);
			SetTileXYPositions();
			BottomLinez[i] = slotRow[i].position.y - (Unit * 2);
			disableSpinButton = true;
			//RandomizeTileIcons();
		}

	}


	//saves the default positions of ALL tiles on start
	void GetTileXYPositions(){
		Vector3 tileX; 

		//default was TileFirstRowParent for SlotRow
		for(int i = 0;  i < (TileYCount); i++ ){ 
			YRow[i] = TileMasterParent.transform.GetChild(i);
			YRow[i].name = "TileRow" + i;

			//how do you get all the slot rows
			for(int j = 0;  j < (TileXCount); j++ ){
				tileX = YRow[i].transform.GetChild(j).position;
				TileXYPos[j,i] = new Vector3(tileX.x, tileX.y, tileX.z); 
			}
		}
	}


	//sets the tile positions for ALL back to default
	void SetTileXYPositions(){
		Transform tileX; 

		for(int i = 0;  i < (TileYCount); i++ ){ 
			YRow[i].position = new Vector3(YRow[i].position.x, TileFirstRowParentStart.y,YRow[i].position.z);

			for(int j = 0;  j < (TileXCount); j++ ){ 
				tileX = YRow[i].transform.GetChild(j);
				tileX.position = TileXYPos[j,i];
			}
		}
	}

	//sets random textures per tile
	void RandomizeTileIcons(){
		// go through each y
		int randNum;
		for(int i = 0;  i < TileYCount; i++ ){
			for(int j = 0;  j < TileXCount; j++ ){
				randNum = Randomizer(0, TileRange);
				YRow[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = TileTex[randNum];
				//print(YRow[i].transform.GetChild(j).GetComponent<MeshRenderer>().material);
			}
		}

	}

	public int Randomizer(int min, int max){
		System.Random random = new System.Random();
		int chosenSystem = random.Next(0,2);
		int chosenNum;
		if(chosenSystem >= 1){ 	chosenNum = random.Next(min, max); }
		else{	chosenNum = UnityEngine.Random.Range(min, max);  }
		return chosenNum;
	}
		
	//END OF LINE
}



