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
	public Transform TileMasterParent;
	public int TileXCount;
	public int TileYCount;
	public int Unit;
	public int SpinCountBeforeStop;
	public bool EnableCloneTileTest;
	public float UnitSpeed;
	public float UnitSpeedStart;

	Vector3 TileParentStart;
	Vector3 BottomLine;
	float BottomLineOffset;
	float EndTileOffset;

	//internal
	int ChildCount;
	int SpinCount;
	int LastTileCount;
	Transform EndTile;
	Transform FirstRowX;
	Transform[] XRow;
	Transform[] YRow;
	Vector3 RowTop; 
	Vector3[] TileXPositions;
	Vector3[] TileYPositions;
	bool EnableSpinSlotTest;
	bool disableSpinButton;

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

		XRow = new Transform[TileXCount];
		YRow = new Transform[TileYCount];

		XRow[0] = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;
	}

	// Use this for initialization
	void Start () {
		if(EnableCloneTileTest){	CreateTileGridTransform();	}
		//Order Matters	
		BottomLine =  new Vector3(XRow[0].position.x,XRow[0].position.y - (Unit * 2), XRow[0].position.z);
		LastTileCount = XRow[0].transform.childCount;
		RowTop = XRow[0].transform.GetChild(LastTileCount - 1).position;
		TileParentStart = XRow[0].position;
		TileXPositions = new Vector3[LastTileCount];
		TileYPositions = new Vector3[TileYCount];
		GetTilePositions();
		disableSpinButton = false;
		//GetTileYPosition();
		//SetTileYPosition();
		SetYRowTransform();
		//GetYRowTransform();
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableSpinSlotTest){	
			//SpinSlotTest(XRow[0]);
			SpinSlotTest(YRow[0]);
		}
	}

	//Creates Tile Transform Grid
	public void CreateTileGridTransform(){
		Transform XClone;
		Transform YClone;

		XRow[0].parent = TileMasterParent;

		//Creates the first row X of tiles
		for (int i = 0; i < TileXCount; i++){
			XClone = Instantiate(TilePrefab, new Vector3(2000, 2000 + (4000 * i), 100), Quaternion.Euler(new Vector3(90, 0, 0)));
			XClone.name = XClone.name + i;
			XClone.parent = XRow[0]; //tiles parented to first row only, itll get duped during Y
		}

		//dupe to Y Axis
		for (int i = 1; i < TileYCount; i++){
			YClone = Instantiate(XRow[0], new Vector3((4000 * i), 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
			YClone.name = YClone.name + i;
			YClone.parent = TileMasterParent;
		}
	}
		
	public void SpinSlotTest(Transform SlotRow){
		//take the FIRST ROW X
		//SlotRow = XRow[0];
		BottomLine.x = SlotRow.position.x;

		//MOVE IT down by 1 UNIT (whatever that is (Target is one unit down from current pos)
		float Step = UnitSpeed * Time.deltaTime * (Unit * 2);
		SlotRow.position = Vector3.MoveTowards(SlotRow.position, BottomLine, Step);
		//print(SlotRow.position);
		//print(BottomLine);

		//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
		EndTile = SlotRow.transform.GetChild(ChildCount);

		//RowTop - top of the row is the last tile position y plus 1 unit, get length of row
		BottomLineOffset = BottomLine.y * 0.5f; 			//bottom line is tile parent position
		EndTileOffset = EndTile.position.y * (SpinCount);

		//When the tiles move down 1 unit
		if(EndTileOffset == BottomLineOffset){
			//EndTile.position = RowTop;
			EndTile.position = new Vector3(EndTile.position.x, RowTop.y, EndTile.position.z);
			SpinCount++;

			//Reset spin
			if(ChildCount < (LastTileCount - 1)){ 	ChildCount++; }
			else{	ChildCount = 0;  }
			
			EndTile = SlotRow.transform.GetChild(ChildCount);
			BottomLine = new Vector3( SlotRow.position.x,  SlotRow.position.y - (Unit * 2),  SlotRow.position.z);

			if(SpinCount >= SpinCountBeforeStop){
				
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
		}
	}

	//sets enabled the anime row test and resets the spin
	public void RunSlotSpin(){
		if (disableSpinButton){}
		else{
			print("SPIN BUTTON PRESSED========================================");
			ResetVarsBeforeSpin(YRow[0]);
		}
	}

	//resets vars before spin
	void ResetVarsBeforeSpin(Transform SlotRow){
		EnableSpinSlotTest = true;
		UnitSpeed = UnitSpeedStart;
		SpinCount = 1;
		ChildCount = 0;
		XRow[0].position = TileParentStart;
		//SlotRow.transform.position = new Vector3(SlotRow.transform.position.x, TileParentStart.y, SlotRow.transform.position.x);
		SetTilePositions();
		RowTop = SlotRow.transform.GetChild(LastTileCount - 1).position;
		BottomLine = new Vector3(SlotRow.position.x,SlotRow.position.y - (Unit * 2), SlotRow.position.z);
		disableSpinButton = true;
	}

	//saves the default positions of tiles on start
	void GetTilePositions(){
		for(int i = 0;  i < (LastTileCount); i++ ){ 
			Vector3 TileX = XRow[0].transform.GetChild(i).position;
			TileXPositions[i] = new Vector3(TileX.x, TileX.y, TileX.z); 
			}
	}
		
	//sets the tile positions back to default
	void SetTilePositions(){
		for(int i = 0;  i < (LastTileCount); i++ ){  XRow[0].transform.GetChild(i).position = TileXPositions[i]; }
	}

	//saves the default positions of tile parents
	void SetTileYPosition(){
		for(int i = 0;  i < (TileYCount); i++ ){ 
			Vector3 TileY = TileMasterParent.transform.GetChild(i).position;
			TileYPositions[i] = new Vector3(TileY.x, TileY.y, TileY.z); 
		}
	}

	//sets tile positions Y to default
	void GetTileYPosition(){
		for(int i = 0;  i < (TileYCount); i++ ){ 
			print(TileYPositions[i]);
			//TileMasterParent.transform.GetChild(i).position = TileXPositions[i]; 
		}
	}

	//get the master parent and Y rows for children
	void SetYRowTransform(){
		for(int i = 0;  i < (TileYCount); i++ ){  
			YRow[i] = TileMasterParent.transform.GetChild(i);
			YRow[i].name = "TileRow" + i;
		}
	}

	void GetYRowTransform(){
		for(int i = 0;  i < (TileYCount); i++ ){  print(YRow[i]);  }
	}

}