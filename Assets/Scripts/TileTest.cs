using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 	TileScript.cs
/// 	Experiment to get tiling tiles working a basic 
/// 	fundamental in gaming needed for multiple games.
/// 
/// </summary>

public class TileTest : MonoBehaviour {

	public Transform TilePrefab;
	public Transform TileParent;
	Vector3 TileParentStart;
	public Transform TileMasterParent;
	public int TileXCount;
	public int TileYCount;
	Vector3 BottomLine;
	Vector3 BottomLineStart;
	float BottomLineOffset;
	float EndTileOffset;

	public bool EnableTileCloneTest;
	public bool EnableAnimateRowTest;

	public int Unit;
	public float UnitSpeed;

	public int SpinCountBeforeStop;

	//internal
	int ChildCount;
	int SpinCount;
	int LastTileCount;
	Transform EndTile;
	Transform FirstRowX;
	Vector3 RowTop;
	public float UnitSpeedStart;
	Vector3[] TileXPositions;
	bool disableSpinButton;

	void Awake(){
		TileParent = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;
		TileXCount = 6;
		TileYCount = 8;

		//Unit
		Unit = 2000; 
		UnitSpeed = 10;			//10 too fast, 5 too slow
		UnitSpeedStart = UnitSpeed;

		ChildCount = 0;
		SpinCount = 1;
		SpinCountBeforeStop = 50;
	}

	// Use this for initialization
	void Start () {
		if(EnableTileCloneTest){	TileCloneTest();	}
			
		BottomLine =  new Vector3(TileParent.position.x,TileParent.position.y - (Unit * 2), TileParent.position.z);
		BottomLineStart = BottomLine;
		FirstRowX = TileParent; //easier to rememeber
		LastTileCount = FirstRowX.transform.childCount;
		RowTop = FirstRowX.transform.GetChild(LastTileCount - 1).position;
		TileParentStart = TileParent.position;
		TileXPositions = new Vector3[LastTileCount];
		GetTilePositions();
		disableSpinButton = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableAnimateRowTest){	AnimateRowTest();	}
	}

	public void TileCloneTest(){
		//Creates the first row X of tiles
		for (int i = 0; i < TileXCount; i++){
			Transform Clone;
			Clone = Instantiate(TilePrefab, new Vector3(2000, 2000 + (4000 * i), 100), Quaternion.Euler(new Vector3(90, 0, 0)));
			Clone.parent = TileParent;
		}

		//dupe to Y Axis
		for (int i = 1; i < TileYCount; i++){
			Transform CloneY;
			CloneY = Instantiate(TileParent, new Vector3((4000 * i), 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
			CloneY.parent = TileMasterParent;
			TileParent.parent = TileMasterParent;
		}
	}
		
	public void AnimateRowTest(){
		//take the FIRST ROW X
		FirstRowX = TileParent;

		//MOVE IT down by 1 UNIT (whatever that is (Target is one unit down from current pos)
		float Step = UnitSpeed * Time.deltaTime * (Unit * 2);
		FirstRowX.position = Vector3.MoveTowards(FirstRowX.position, BottomLine, Step);

		//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
		EndTile = FirstRowX.transform.GetChild(ChildCount);

		//RowTop - top of the row is the last tile position y plus 1 unit, get length of row
		BottomLineOffset = BottomLine.y * 0.5f; //bottom line is tile parent position
		EndTileOffset = EndTile.position.y * (SpinCount);

		//When the tiles move down 1 unit
		if(EndTileOffset == BottomLineOffset){
			EndTile.position = RowTop;
			SpinCount++;

			//Reset
			if(ChildCount < (LastTileCount - 1)){ 	ChildCount++; }
			else{	ChildCount = 0;  }
			
			EndTile = FirstRowX.transform.GetChild(ChildCount);
			BottomLine = new Vector3(BottomLine.x, BottomLine.y - (Unit * 2), BottomLine.z);

			if(SpinCount >= SpinCountBeforeStop){
				// spin keeps on spinnin'
				if( UnitSpeed < 2 ){   
					UnitSpeed = 0;		 
					EnableAnimateRowTest = false;
					disableSpinButton = false;}
				else{ 
					UnitSpeed = UnitSpeed - (UnitSpeed * 0.25f);}
			}
			else{ //nothin
			}
		}
	}

	//sets enabled the anime row test and resets the spin
	public void SetEnableAnimateRowTest(){
		if (disableSpinButton){}
		else{
			print("SPIN BUTTON PRESSED========================================");
			EnableAnimateRowTest = true;
			UnitSpeed = UnitSpeedStart;
			SpinCount = 1;
			ChildCount = 0;
			TileParent.position = TileParentStart;
			FirstRowX.position = TileParentStart;
			SetTilePositions();
			RowTop = TileParent.transform.GetChild(LastTileCount - 1).position;
			BottomLine = new Vector3(TileParent.position.x,TileParent.position.y - (Unit * 2), TileParent.position.z);
			disableSpinButton = true;
		}
	}

	//saves the default positions of tiles on start
	public void GetTilePositions(){
		for(int i = 0;  i < (LastTileCount); i++ ){ 
			Vector3 TileX = FirstRowX.transform.GetChild(i).position;
			TileXPositions[i] = new Vector3(TileX.x, TileX.y, TileX.z); 
			}
	}

	//sets the tile positions back to default
	public void SetTilePositions(){
		for(int i = 0;  i < (LastTileCount); i++ ){  FirstRowX.transform.GetChild(i).position = TileXPositions[i]; }
	}
}