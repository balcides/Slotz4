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
	int ChildCount;
	int[] ChildCountz;
	int SpinCount;
	int[] SpinCountz;
	int LastTileCount;
	Transform EndTile;
	Transform EndTile2;
	Transform EndTile3;
	Transform[] EndTilez;
	Transform FirstRowX;
	Transform[] YRow;
	float RowTop; 
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
		TileYCount = 8;

		//Unit
		Unit = 2000; 
		UnitSpeed = 10;					//10 too fast, 5 too slow
		UnitSpeedStart = UnitSpeed;

		ChildCount = 0;
		SpinCount = 1;

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
		RowTop = TileFirstRowParent.transform.GetChild(LastTileCount - 1).position.y;
		for (int i = 0; i < TileYCount; i++){ RowTopz[i] = TileFirstRowParent.transform.GetChild(LastTileCount - 1).position.y; } 		// fill bottom lines
		TileFirstRowParentStart = TileFirstRowParent.position;
		TileXPositions = new Vector3[LastTileCount];
		TileXYPos = new Vector3[TileXCount,TileYCount];
		disableSpinButton = false;
		SetYRowTransform();
		//TempRow = YRow[0];
		//GetTilePositions(TempRow);
		GetTileXYPositions();
		for (int i = 0; i < TileYCount; i++){ SpinCountz[i] = 1; } 
		for (int i = 0; i < TileYCount; i++){ ChildCountz[i] = 0; } 
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableSpinSlotTest){	
			//SpinSlotTest( TempRow );
			SpinSlotTest(0);
			SpinSlotTest(1);
			SpinSlotTest(2);
			SpinSlotTest(3);
			SpinSlotTest(4);
			SpinSlotTest(5);
			SpinSlotTest(6);
			SpinSlotTest(7);

			//SpinSlotTest(YRow[0], YRow[1], YRow[2]);
			//for(int i =0; i < TileYCount; i++){
			//	SpinSlotTest(YRow[i]);
			//}
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
		

	public void SpinSlotTest(int i = 0){
		//take the FIRST ROW X
		//float XRowPos = SlotRow.position.x;

		//wonder if better to use the loop in this or outside of this?
		//for(int i = 0; i < YRow.Length; i++){
		//for(int j = 0; j < TileXCount; j++){
			//MOVE IT down by 1 UNIT (whatever that is (Target is one unit down from current pos)
			float Step = UnitSpeed * Time.deltaTime * (Unit * 2);
			//SlotRow.position = Vector3.MoveTowards(SlotRow.position,new Vector3(SlotRow.position.x ,BottomLine, SlotRow.position.z), Step);
			//SlotRow2.position = Vector3.MoveTowards(SlotRow2.position,new Vector3(SlotRow2.position.x ,BottomLine, SlotRow2.position.z), Step);
			//SlotRow3.position = Vector3.MoveTowards(SlotRow3.position,new Vector3(SlotRow3.position.x ,BottomLine, SlotRow3.position.z), Step);
			//slotRow[i].position = Vector3.MoveTowards(slotRow[i].position,new Vector3(slotRow[i].position.x ,BottomLinez[i], slotRow[i].position.z), Step);
			YRow[i].position = Vector3.MoveTowards(YRow[i].position,new Vector3(YRow[i].position.x ,BottomLinez[i],YRow[i].position.z), Step);

			//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
			//EndTile = SlotRow.transform.GetChild(ChildCount);
			//EndTile2 = SlotRow2.transform.GetChild(ChildCount);
			//EndTile3 = SlotRow3.transform.GetChild(ChildCount);
			//EndTilez[i] = YRow[i].transform.GetChild(ChildCount);
			EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);

			//RowTop - top of the row is the last tile position y plus 1 unit, get length of row
			//BottomLineOffset = BottomLine * 0.5f; 			//bottom line is tile parent position
			//BottomLineOffset = BottomLinez[i] * 0.5f;
			BottomLineOffsetz[i] = BottomLinez[i] * 0.5f;

			//EndTileOffset = EndTile.position.y * (SpinCount);
			//EndTileOffset2 = EndTile.position.y * (SpinCount);
			//EndTileOffset3 = EndTile.position.y * (SpinCount);
			//EndTileOffset = endTile[i].position.y * (SpinCount);
			//EndTileOffsetz[i] = EndTilez[i].position.y * (SpinCount);
			EndTileOffsetz[i] = EndTilez[i].position.y * (SpinCountz[i]);

			print("index= " + i + " EndTileOffsetz: " + EndTileOffsetz[i]+ "=== BottomLineOffsetz: " + BottomLineOffsetz[i]);

	
			//When the tiles move down 1 unit
			//if(EndTileOffset == BottomLineOffset || EndTileOffset2 == BottomLineOffset || EndTileOffset3 == BottomLineOffset){
			if(EndTileOffsetz[i] == BottomLineOffsetz[i]){
				print("********* Tile end reached *******");
				print("current index = " + i);
				//EndTile.position = new Vector3(EndTile.position.x, RowTop, EndTile.position.z);
				//EndTile2.position = new Vector3(EndTile2.position.x, RowTop, EndTile2.position.z);
				//EndTile3.position = new Vector3(EndTile3.position.x, RowTop, EndTile3.position.z);
				//endTile[i].position = new Vector3(endTile[i].position.x, RowTop, endTile[i].position.z);
				EndTilez[i].position = new Vector3(EndTilez[i].position.x, RowTopz[i], EndTilez[i].position.z);
				//print("EndTilez pos" + i + " " + EndTilez[i].position);
				//print("RowTopz " + i + " " + RowTopz[i]);

				SpinCount++;
				SpinCountz[i]++;

				//Reset spin
				//if(ChildCount < (LastTileCount - 1)){ 	ChildCount++; }
				//else{	ChildCount = 0;  }
				if(ChildCountz[i] < (LastTileCount - 1)){ 	ChildCountz[i]++; }
				else{	ChildCountz[i] = 0;  }

				//EndTile = SlotRow.transform.GetChild(ChildCount);
				//EndTile2 = SlotRow2.transform.GetChild(ChildCount);
				//EndTile3 = SlotRow3.transform.GetChild(ChildCount);
				//EndTilez[i] = YRow[i].transform.GetChild(ChildCount);
				EndTilez[i] = YRow[i].transform.GetChild(ChildCountz[i]);

				//print("EndTilez" + i + " " + EndTilez[i]);
				//print("YRow" + i + " " + YRow[i]);

				//BottomLine = SlotRow.position.y - (Unit * 2);
				//BottomLine = TileFirstRowParentStart.y - (Unit * 2);
				//BottomLine = slotRow[i].position.y - (Unit * 2); //working one
				BottomLinez[i] = YRow[i].position.y - (Unit * 2);

				//if(SpinCount >= SpinCountBeforeStop){
				if(SpinCountz[i] >= SpinCountBeforeStop){
					
					//spin stops, reset
					if( UnitSpeed < 2 ){   
						UnitSpeed = 0;		 
						EnableSpinSlotTest = false;
						disableSpinButton = false;	}
					else{ 
						// spin keeps on spinnin'
						UnitSpeed = UnitSpeed - (UnitSpeed * 0.25f);	}
				}
				else{ //nothin 
				}
				print("********* Tile end reached *******");
			}
		//}
	}


	//sets enabled the anime row test and resets the spin
	public void RunSlotSpin(){
		if (disableSpinButton){}
		else{
			print("SPIN BUTTON PRESSED========================================");
			//ResetVarsBeforeSpin( YRow[0] );
			//ResetVarsBeforeSpin( YRow[1] );
			//ResetVarsBeforeSpin( YRow[2] );
			ResetVarsBeforeSpin(YRow);

			//for(int i =0; i < TileYCount; i++){
			//	ResetVarsBeforeSpin( YRow[i] );
			//}

		}
	}


	//resets vars before spin
	void ResetVarsBeforeSpin(Transform[] slotRow){
		for(int i = 0; i < TileYCount; i++){
			EnableSpinSlotTest = true;
			UnitSpeed = UnitSpeedStart;
			SpinCount = 1;
			SpinCountz[i] = 1; 
			ChildCount = 0;
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
		//Vector3 tileY;
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