/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org/>
*/
using UnityEngine;
using System.Collections;

//============================================================================================
//============================================================================================
public class MainGameObject : MonoBehaviour 
{
	public PadController padC;

	//joystick model transforms
	public Transform leftjoystick_t;
	Vector3 leftjoystickUp;
	Vector3 leftjoystickDown;
	
	public Transform rightjoystick_t;
	Vector3 rightjoystickUp;
	Vector3 rightjoystickDown;

	public Transform ltrigger_t;
	public Transform rtrigger_t;


	//dpad model transforms
	public Transform leftbutton_t;
	Vector3 leftbuttonUp;
	Vector3 leftbuttonDown;

	public Transform rightbutton_t;
	Vector3 rightbuttonUp;
	Vector3 rightbuttonDown;

	public Transform upbutton_t;
	Vector3 upbuttonUp;
	Vector3 upbuttonDown;

	public Transform downbutton_t;
	Vector3 downbuttonUp;
	Vector3 downbuttonDown;

	//action button model transforms
	public Transform abutton_t;
	Vector3 abuttonUp;
	Vector3 abuttonDown;

	public Transform bbutton_t;
	Vector3 bbuttonUp;
	Vector3 bbuttonDown;

	public Transform xbutton_t;
	Vector3 xbuttonUp;
	Vector3 xbuttonDown;

	public Transform ybutton_t;
	Vector3 ybuttonUp;
	Vector3 ybuttonDown;

	//shoulder button  model transforms
	public Transform ltbutton_t;
	Vector3 ltbuttonUp;
	Vector3 ltbuttonDown;

	public Transform rtbutton_t;
	Vector3 rtbuttonUp;
	Vector3 rtbuttonDown;

	//pause button model transforms
	public Transform pbutton_t;  
	Vector3 pbuttonUp;
	Vector3 pbuttonDown;
	


	//============================================================================================
	//============================================================================================
	// Use this for initialization
	void Start () 
	{

		//this code just gets the positions for the various controller components that are rendered
		padC = GameObject.Find("ControllerObject").GetComponent<PadController>();

		leftbutton_t = GameObject.Find("leftbutton").GetComponent<Transform>();
		rightbutton_t = GameObject.Find("rightbutton").GetComponent<Transform>();
		upbutton_t = GameObject.Find("upbutton").GetComponent<Transform>();
		downbutton_t = GameObject.Find("downbutton").GetComponent<Transform>();

		abutton_t = GameObject.Find("abutton").GetComponent<Transform>();
		bbutton_t = GameObject.Find("bbutton").GetComponent<Transform>();
		xbutton_t = GameObject.Find("xbutton").GetComponent<Transform>();
		ybutton_t = GameObject.Find("ybutton").GetComponent<Transform>();

		ltbutton_t = GameObject.Find("lefttop").GetComponent<Transform>();
		rtbutton_t = GameObject.Find("righttop").GetComponent<Transform>();

		pbutton_t = GameObject.Find("pause").GetComponent<Transform>();
	
		//object positions for analog stick models
		leftjoystick_t = GameObject.Find("leftjoystick").GetComponent<Transform>();
		leftjoystickUp = leftjoystick_t.localPosition;  
		leftjoystickDown = leftjoystickUp;
		leftjoystickDown.y -= 0.1f;
		
		rightjoystick_t = GameObject.Find("rightjoystick").GetComponent<Transform>();
		rightjoystickUp = rightjoystick_t.localPosition;  
		rightjoystickDown = rightjoystickUp;
		rightjoystickDown.y -= 0.1f;

		//object positions for analog triggers
		ltrigger_t = GameObject.Find("ltrigger").GetComponent<Transform>();
		rtrigger_t = GameObject.Find("rtrigger").GetComponent<Transform>();


		//object positions for dpad buttons
		leftbuttonUp = leftbutton_t.localPosition;
		leftbuttonDown = leftbuttonUp;
		leftbuttonDown.y -= 0.1f;

		rightbuttonUp = rightbutton_t.localPosition;
		rightbuttonDown = rightbuttonUp;
		rightbuttonDown.y -= 0.1f;

		upbuttonUp = upbutton_t.localPosition;
		upbuttonDown = upbuttonUp;
		upbuttonDown.y -= 0.1f;

		downbuttonUp = downbutton_t.localPosition;
		downbuttonDown = downbuttonUp;
		downbuttonDown.y -= 0.1f;

		//object positions for action buttons
		abuttonUp = abutton_t.localPosition;
		abuttonDown = abuttonUp;
		abuttonDown.y -= 0.1f;

		bbuttonUp = bbutton_t.localPosition;
		bbuttonDown = bbuttonUp;
		bbuttonDown.y -= 0.1f;

		xbuttonUp = xbutton_t.localPosition;
		xbuttonDown = xbuttonUp;
		xbuttonDown.y -= 0.1f;

		ybuttonUp = ybutton_t.localPosition;
		ybuttonDown = ybuttonUp;
		ybuttonDown.y -= 0.1f;

		//object positions for shoulder buttons
		ltbuttonUp = ltbutton_t.localPosition;
		ltbuttonDown = ltbuttonUp;
		ltbuttonDown.z += 0.1f;

		rtbuttonUp = rtbutton_t.localPosition;
		rtbuttonDown = rtbuttonUp;
		rtbuttonDown.z += 0.1f;

		//pause
		pbuttonUp = pbutton_t.localPosition;
		pbuttonDown = pbuttonUp;
		pbuttonDown.y -= 0.1f;
		
		
	}
	
	//============================================================================================
	//============================================================================================
	// Update is called once per frame
	void Update () 
	{

		float xR;
		float zR;
		Quaternion q;

		int connectedPadCount =padC.PadCount(); //how many pads do we have currently attached?

		//if any of the pads that are attached are of a type we can't handle, ignore all pads (gui should tell user to remove unknown pad)
		bool knownPadAttached = false;  
		for(int i=0; i<connectedPadCount; i++)
		{
			knownPadAttached|=padC.KnownPad(i);
		}

		//there is at least one pad connected and all the controllers are of known types then we can process the pad input data
		if((connectedPadCount>0) && (knownPadAttached)) 
		{
			//this code just updates the positions for the various controller components that are rendered
			//left analog stick rotation
			//xR = padC.getAnalog(0, ControllerAnalogs.LEFTX); //if you wanna use an enumerated controller just use the version with an id in it
			xR = padC.getAnalog(ControllerAnalogs.LEFTX);
			zR = padC.getAnalog(ControllerAnalogs.LEFTY);

			q = Quaternion.Euler(zR*45.0f,0,xR*45);
   			leftjoystick_t.localRotation=q;

			//left analog stick button
			if(padC.isDown(ControllerButtons.LAS_BUTTON))
				leftjoystick_t.localPosition=leftjoystickDown;
			else
				leftjoystick_t.localPosition=leftjoystickUp;


			//right analog stick rotation
			xR = padC.getAnalog(ControllerAnalogs.RIGHTX);
			zR = padC.getAnalog(ControllerAnalogs.RIGHTY);
			q = Quaternion.Euler(zR*45.0f,0,xR*45.0f);
			rightjoystick_t.localRotation=q;
		
			//right analog stick button
			if(padC.isDown(ControllerButtons.RAS_BUTTON))
				rightjoystick_t.localPosition=rightjoystickDown;
			else
				rightjoystick_t.localPosition=rightjoystickUp;


			//analog triggers
			float lT = padC.getAnalog(ControllerAnalogs.LEFTTRIGGER);
			q = Quaternion.Euler(lT*-45.0f,0.0f,0.0f);
			ltrigger_t.localRotation=q;

			float rT = padC.getAnalog(ControllerAnalogs.RIGHTTRIGGER);
			q = Quaternion.Euler(rT*-45.0f,0.0f,0.0f);
			rtrigger_t.localRotation=q;

			//dpad
			if(padC.isDown(ControllerButtons.LEFT))
				leftbutton_t.localPosition=leftbuttonDown;
			else
				leftbutton_t.localPosition=leftbuttonUp;

			if(padC.isDown(ControllerButtons.RIGHT))
				rightbutton_t.localPosition=rightbuttonDown;
			else
				rightbutton_t.localPosition=rightbuttonUp;

			if(padC.isDown(ControllerButtons.UP))
				upbutton_t.localPosition=upbuttonDown;
			else
				upbutton_t.localPosition=upbuttonUp;

			if(padC.isDown(ControllerButtons.DOWN))
				downbutton_t.localPosition=downbuttonDown;
			else
				downbutton_t.localPosition=downbuttonUp;

			//action buttons
			if(padC.isDown(ControllerButtons.BUTA))
				abutton_t.localPosition=abuttonDown;
			else
				abutton_t.localPosition=abuttonUp;

			if(padC.isDown(ControllerButtons.BUTB))
				bbutton_t.localPosition=bbuttonDown;
			else
				bbutton_t.localPosition=bbuttonUp;

			if(padC.isDown(ControllerButtons.BUTX))
				xbutton_t.localPosition=xbuttonDown;
			else
				xbutton_t.localPosition=xbuttonUp;

			if(padC.isDown(ControllerButtons.BUTY))
				ybutton_t.localPosition=ybuttonDown;
			else
				ybutton_t.localPosition=ybuttonUp;

			//shoulder buttons
			if(padC.isDown(ControllerButtons.LEFTTOP))
				ltbutton_t.localPosition=ltbuttonDown;
			else
				ltbutton_t.localPosition=ltbuttonUp;

			if(padC.isDown(ControllerButtons.RIGHTTOP))
				rtbutton_t.localPosition=rtbuttonDown;
			else
				rtbutton_t.localPosition=rtbuttonUp;

			if(padC.isDown(ControllerButtons.PAUSE))
				pbutton_t.localPosition=pbuttonDown;
			else
				pbutton_t.localPosition=pbuttonUp;
			

		}
	}
}
//============================================================================================
