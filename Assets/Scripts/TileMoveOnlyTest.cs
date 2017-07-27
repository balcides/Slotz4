using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMoveOnlyTest : MonoBehaviour {

	//MOVE1
	public Transform Target;
	public float Speed;

	//MOVE2
	public Transform StartMarker;
	public Transform EndMarker;
	public float Speed2 = 1.0F;
	private float StartTime;
	private float JourneyLength;


	//MOVE3
	public Transform Sunrise;
	public Transform Sunset;
	public float JourneyTime = 1.0F;
	private float StartTime2;

	//MOVE4
	public Transform Target2;
	public float Speed3;
	public Vector3 Direction;

	void Awake(){

	}

	// Use this for initialization
	void Start () {

		//Move2
		StartTime = Time.time;
		JourneyLength = Vector3.Distance(StartMarker.position, EndMarker.position);


		//Move3
		StartTime2 = Time.time;

		//Move4
		Speed3 = Speed3 * Time.deltaTime;
		Direction = Vector3.up;
	}
	
	// Update is called once per frame
	void Update () {
		Move1();
	}

	//Uses Move Towards
	void Move1(){
		float Step = Speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, Target.position, Step);
	}

	//Uses LERP
	void Move2(){
		float DistCovered = (Time.time - StartTime) * Speed2;
		float FracJourney = DistCovered / JourneyLength;
		transform.position = Vector3.Lerp(StartMarker.position, EndMarker.position, FracJourney);
	}

	//Uses SuperLerp(SLERP)
	void Move3(){
		Vector3 Center = (Sunrise.position + Sunset.position) * 0.5F;
		Center -= new Vector3(0, 1, 0);
		Vector3 RiseRelCenter = Sunrise.position - Center;
		Vector3 SetRelCenter = Sunset.position - Center;
		float FracComplete = (Time.time - StartTime2) / JourneyTime;
		transform.position = Vector3.Slerp(RiseRelCenter, SetRelCenter, FracComplete);
		transform.position += Center;
	}

	//Uses Translate
	void Move4(){
		if(transform.position.y >= Target2.position.y){
		}else{
			//else move
			//transform.Translate(Direction * Speed3);
			transform.Translate(Direction * Speed3, Space.World);
		}
	}
}
