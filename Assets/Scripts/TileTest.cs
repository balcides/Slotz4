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
	public Transform TileMasterParent;
	public int TileXCount;
	public int TileYCount;
	Vector3 BottomLine;
	float BottomLineOffset;
	float EndTileOffset;

	public bool EnableTileCloneTest;
	public bool EnableAnimateRowTest;

	public int Unit;
	public int UnitSpeed;

	//internal
	int ChildCount;
	int SpinCount;
	int LastTileCount;
	Transform EndTile;
	Transform FirstRowX;
	Vector3 RowTop;

	void Awake(){
		TileParent = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;
		TileXCount = 6;
		TileYCount = 8;

		//Unit
		Unit = 2000; 
		UnitSpeed = 1;

		ChildCount = 0;
		SpinCount = 1;
	}

	// Use this for initialization
	void Start () {
		if(EnableTileCloneTest){	TileCloneTest();	}

		BottomLine =  new Vector3(TileParent.position.x,TileParent.position.y - (Unit * 2), TileParent.position.z);
		FirstRowX = TileParent; //easier to rememeber
		LastTileCount = FirstRowX.transform.childCount;
		RowTop = FirstRowX.transform.GetChild(LastTileCount - 1).position;
	}
	
	// Update is called once per frame
	void Update () {
		if(EnableAnimateRowTest){	AnimateRowTest();	}
	}

	public void TileCloneTest(){
		//Creates the first row X of tiles
		for (int i = 0; i < TileXCount; i++){
			Transform Clone;
			Clone = Instantiate(TilePrefab, new Vector3(2000, 2000 + (4000 * i), 0), Quaternion.Euler(new Vector3(90, 0, 0)));
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
		//EndTile.gameObject.GetComponent<Renderer>().material.color = Color.red;

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
		}
	}
}
