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

	public bool EnableTileCloneTest;
	public bool EnableAnimateRowTest;

	void Awake(){
		TileParent = new GameObject("Example GO").transform;
		TileMasterParent = new GameObject("Example GO Master").transform;
		TileXCount = 6;
		TileYCount = 8;
	}

	// Use this for initialization
	void Start () {
		if(EnableTileCloneTest){	TileCloneTest();	}
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
		//take the FIRST ROW and MOVE IT down by 1 UNIT (whatever that is)
		Transform FirstRow = TileParent;			//FirstRow

		//Unit
		int Unit = 4000;
		int UnitSpeed = 10;

		//MoveDown
		FirstRow.Translate(0, (Time.deltaTime * (-2000) * UnitSpeed), 0);

		//Target is one unit down from current pos
		Vector3 Target = new Vector3(FirstRow.position.x,FirstRow.position.y - Unit, FirstRow.position.z);
		float Step = UnitSpeed * Time.deltaTime;
		FirstRow.position = Vector3.MoveTowards(FirstRow.position, Target, Step);

		//and then take the TILE AT THE END and MOVE IT to the TOP OF THE ROW
		//EndTile
		//MoveIt
		//RowTop
	}

	public void AnimateTileTest(){
		
	}
}
