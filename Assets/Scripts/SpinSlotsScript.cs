using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	//pre-internal
	Vector3 TileFirstRowParentStart;
	float BottomLine;
	float[] BottomLinez;
	float BottomLineOffset;
	float[] BottomLineOffsetz;
	float EndTileOffset;
	//float EndTileOffset2;
	//float EndTileOffset3;
	float[] EndTileOffsetz;
	float UnitSpeedStart;

	//Internal

	int[] ChildCountz;
	int[] SpinCountz;
	int LastTileCount;
	Transform EndTile;
	Transform EndTile2;
	Transform EndTile3;
	Transform[] EndTilez;
	Transform FirstRowX;
	Transform[] YRow;
	float[] RowTopz;
	Vector3[] TileXPositions;
	Vector3[,] TileXYPos;
	bool EnableSpinSlotTest;
	bool disableSpinButton;
	float CountTest;
	//Transform TempRow;

	//get an array to store all the y parents
	//then a series of arrays for the x's
	void Awake(){
		TileXCount = 6;
		TileYCount = 9;

		//Unit
		Unit = 2000; 
		UnitSpeed = 10;					//10 too fast, 5 too slow
		UnitSpeedStart = UnitSpeed;

		SpinCountBeforeStop = 50;

		YRow = new Transform[TileYCount];

		TileFirstRowParent = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;

		EndTilez = new Transform[TileYCount];
		EndTileOffsetz = new float[TileYCount];
		BottomLinez = new float[TileYCount];
		BottomLineOffsetz = new float[TileYCount];
		RowTopz = new float[TileYCount];
		SpinCountz = new int[TileYCount];
		ChildCountz = new int[TileYCount];
	}

	// Use this for initialization
	void Start () {
		if(EnableCloneTileTest){	CreateTileGridTransform();	}
		//Order Matters	
		BottomLine =  TileFirstRowParent.position.y - (Unit * 2);

		for (int i = 0; i < TileYCount; i++){ BottomLinez[i] = BottomLine; } 		// fill bottom lines

		LastTileCount = TileFirstRowParent.transform.childCount;
		for (int i = 0; i < TileYCount; i++){ RowTopz[i] = TileFirstRowParent.transform.GetChild(LastTileCount - 1).position.y; } 		// fill bottom lines
		TileFirstRowParentStart = TileFirstRowParent.position;
		TileXPositions = new Vector3[LastTileCount];
		TileXYPos = new Vector3[TileXCount,TileYCount];
		disableSpinButton = false;
		SetYRowTransform();
		GetTileXYPositions();
		for (int i = 0; i < TileYCount; i++){ SpinCountz[i] = 1; } 
		for (int i = 0; i < TileYCount; i++){ ChildCountz[i] = 0; } 
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableSpinSlotTest){	
			SpinSlotTest(0);
			SpinSlotTest(1);
			SpinSlotTest(2);
			SpinSlotTest(3);
			SpinSlotTest(4);
			SpinSlotTest(5);
			SpinSlotTest(6);
			SpinSlotTest(7);
			SpinSlotTest(8);
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
		float Step = UnitSpeed * Time.deltaTime * (Unit * 2);
		YRow[i].position = Vector3.MoveTowards(YRow[i].position,new Vector3(YRow[i].position.x ,BottomLinez[i],YRow[i].position.z), Step);

		//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
		EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);

		//RowTop - top of the row is the last tile position y plus 1 unit, get length of row
		BottomLineOffsetz[i] = BottomLinez[i] * 0.5f;			//bottom line is tile parent position
		EndTileOffsetz[i] = EndTilez[i].position.y * (SpinCountz[i]);
		//print("index= " + i + " EndTileOffsetz: " + EndTileOffsetz[i]+ "=== BottomLineOffsetz: " + BottomLineOffsetz[i]);

		//When the tiles move down 1 unit
		if(EndTileOffsetz[i] == BottomLineOffsetz[i]){
			print("********* Tile end reached *******");
			print("current index = " + i);
			EndTilez[i].position = new Vector3(EndTilez[i].position.x, RowTopz[i], EndTilez[i].position.z);

			SpinCountz[i]++;

			//Reset spin
			if(ChildCountz[i] < (LastTileCount - 1)){ 	ChildCountz[i]++; }
			else{	ChildCountz[i] = 0;  }

			EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);
			BottomLinez[i] = YRow[i].position.y - (Unit * 2);

			if(SpinCountz[i] >= SpinCountBeforeStop){
				
				//spin stops, reset
				if( UnitSpeed < 2 ){   
					UnitSpeed = 0;		 
					EnableSpinSlotTest = false;
					disableSpinButton = false;	}
				else{ UnitSpeed = UnitSpeed - (UnitSpeed * 0.25f);	// spin keeps on spinnin' 
				}
			}
			else{}
			print("********* Tile end reached *******");
		}

	}


	//sets enabled the anime row test and resets the spin
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
			SpinCountz[i] = 1; 
			ChildCountz[i] = 0; 
			//SlotRow.position = new Vector3(SlotRow.position.x, TileFirstRowParentStart.y, SlotRow.position.z);
			slotRow[i].position = new Vector3(slotRow[i].position.x, TileFirstRowParentStart.y, slotRow[i].position.z);
			//SetTilePositions(TempRow);
			SetTileXYPositions();
			BottomLine = slotRow[i].position.y - (Unit * 2); //working one
			//BottomLine = TileFirstRowParent.position.y - (Unit * 2);
			BottomLinez[i] = slotRow[i].position.y - (Unit * 2);
			disableSpinButton = true;
		}

	}


	//saves the default positions of tiles on start
	void GetTilePositions(Transform SlotRow){
		//default was TileFirstRowParent for SlotRow
		for(int i = 0;  i < (LastTileCount); i++ ){ 
			Vector3 TileX = SlotRow.transform.GetChild(i).position;
			TileXPositions[i] = new Vector3(TileX.x, TileX.y, TileX.z); 
			}
	}


	//saves the default positions of ALL tiles on start
	void GetTileXYPositions(){
		Transform tileY;
		Vector3 tileX; 

		//default was TileFirstRowParent for SlotRow
		for(int i = 0;  i < (TileYCount); i++ ){ 

			//Transform TMasterParent = TileMasterParent.transform.GetChild(i); //long way
			//Vector3 TileY = new Vector3(TMasterParent.position.x, TMasterParent.position.y, TMasterParent.position.z);//long way
			tileY = TileMasterParent.transform.GetChild(i);

			//how do you get all the slow rows
			for(int j = 0;  j < (TileXCount); j++ ){
				
				tileX = tileY.transform.GetChild(j).position;
				TileXYPos[j,i] = new Vector3(tileX.x, tileX.y, tileX.z); 
			}
		}
	}

		
	//sets the tile positions back to default
	void SetTilePositions(Transform SlotRow ){
		//default was TileFirstRowParent for SlotRow
		for(int i = 0;  i < (LastTileCount); i++ ){  SlotRow.transform.GetChild(i).position = TileXPositions[i]; }
	}


	//sets the tile positions for ALL back to default
	void SetTileXYPositions(){
		Transform yRow;
		Transform tileX; 

		for(int i = 0;  i < (TileYCount); i++ ){ 
			yRow = TileMasterParent.transform.GetChild(i);
			YRow[i].position = new Vector3(YRow[i].position.x, TileFirstRowParentStart.y,YRow[i].position.z);

			for(int j = 0;  j < (TileXCount); j++ ){ 
				//tileX = tileY.transform.GetChild(j).position;
				//tileX.position = TileXYPos[j,i]; 
				tileX = yRow.transform.GetChild(j);
				tileX.position = TileXYPos[j,i];
			}
		}
	}


	//gets and renames the master parent and Y rows for children 
	void SetYRowTransform(){
		for(int i = 0;  i < (TileYCount); i++ ){  
			YRow[i] = TileMasterParent.transform.GetChild(i);
			YRow[i].name = "TileRow" + i;
		}
	}


	void GetYRowTransform(){
		for(int i = 0;  i < (TileYCount); i++ ){  print(YRow[i]);  }
	}

	void IterationTest(float countTest){
		for(int i = 0; i < TileYCount; i++){
			print("testing");
		}
		
	}
}