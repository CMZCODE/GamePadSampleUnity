using UnityEngine;
using System.Collections;

/*
public class MainGameObjectScript : MonoBehaviour 
{
	public PadController padC;
	public Transform leftbutton_t;
	float leftbuttonY;

	
	//============================================================================================
	void Start () 
	{
		padC = GameObject.Find("ControllerObject").GetComponent<PadController>();
		leftbutton_t = GameObject.Find("leftbutton").GetComponent<Transform>();
		leftbuttonY = leftbutton_t.localPosition.y;

	}
	//============================================================================================

	//============================================================================================
	// Update is called once per frame
	void Update () 
	{
		float t = leftbuttonY;
		if(padC.isDown(0,ControllerButtons.LEFT))
		{
			t+=0.05;
		}
		leftbutton_t.localPosition.y = t;


	}
	//============================================================================================

}

*/