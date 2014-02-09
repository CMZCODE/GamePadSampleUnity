#define DEBUG_CONTROLLER 

//============================================================================================
//============================================================================================
using UnityEngine;
using System.Collections;
using System;
//============================================================================================


	public enum ControllerButtons
	{
		LAS_BUTTON,
		RAS_BUTTON,
		BUTX,
		BUTY,
		BUTA,
		BUTB,
		UP,
		DOWN,
		LEFT,         
		RIGHT,
		LEFTTOP,
		RIGHTTOP,
		PAUSE
	}
	
	public enum ControllerAnalogs
	{
		LEFTX,
		LEFTY,
		RIGHTX,
		RIGHTY,
		LEFTTRIGGER,
		RIGHTTRIGGER
	}
	




//============================================================================================
//============================================================================================
public class ControllerPacket
{
public	float leftStickX;
public	float leftStickY;
public	float rightStickX;
public	float rightStickY;

public	bool leftAsDown;
public	bool rightAsDown;

public	bool butX;
public	bool butY;
public	bool butA;
public	bool butB;
	
public	bool up;
public	bool down;
public	bool left;
public	bool right;

public	bool leftTop;
public	float leftTrigger;
	
public	bool rightTop;
public	float rightTrigger;

public	bool pause;

	
public ControllerPacket Copy()
    {
       return (ControllerPacket)this.MemberwiseClone();
    }
	
	
};
//============================================================================================



//============================================================================================
//============================================================================================
public class PadController : MonoBehaviour {

	static int mMAXCONTROLLERS = 4;
	static string mVersion = "0.01";

	int mActivePadCount;  //the number of currently active game pads detected
	ControllerPacket[] mPadData;
	ControllerPacket[] mPadDataOld; 
	string[] mPadName;


	string[] mPADS=
		{
			"NVIDIA Corporation NVIDIA Controller v01",  //ignoring the version number
			"Controller (Wireless Gamepad F710)",
			"XBOX 360 For Windows",
		    "Afterglow Gamepad for Xbox 360",
			"Broadcom Bluetooth HID" // nyco playpad pro

	};

	

	//============================================================================================
	//returns the number of connected games pads
	//============================================================================================
	public int PadCount()
	{
		return mActivePadCount;
	}


	//============================================================================================
	//can we handle this type of pad?
	//============================================================================================
	public bool KnownPad(int id)
	{
		bool padFound=false;
		foreach (string pad in mPADS)
		{
    		if(mPadName[id].Contains(pad)) padFound=true;
		}
	return padFound;		
	}
	  

	//============================================================================================
	// Use this for initialization
	//============================================================================================
	void Start () 
	{
		mPadData = new ControllerPacket[mMAXCONTROLLERS];
		mPadDataOld = new ControllerPacket[mMAXCONTROLLERS];

		for(int i=0; i<mMAXCONTROLLERS; i++)
		{
			mPadData[i] = new ControllerPacket();
			mPadDataOld[i] = new ControllerPacket();
		}

	}
	//============================================================================================


	//============================================================================================
	// Update is called once per frame
	//============================================================================================
	void Update() 
	{
		
		//copy of previous frames data is used for detecting down and up events for debounced controls
		for(int i=0; i<mMAXCONTROLLERS; i++)
		{
			mPadDataOld[i] = mPadData[i].Copy(); 
		}
		
		mPadName = Input.GetJoystickNames();
		mActivePadCount = mPadName.Length;
		
		
		for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and perform mapping to internal representation
		{
			if(mPadName[i].Contains(mPADS[0]))	 readSHIELDPAD(i);  //shield

			if(mPadName[i].Contains(mPADS[1]))	 readXBOX360(i);    //logitech f710 (maps to xbox360)

			if(mPadName[i].Contains(mPADS[2]))	 readXBOX360(i);     //xbox360 wired

			if(mPadName[i].Contains(mPADS[3]))	 readXBOX360(i);  	//afterglow xbox 360

			if(mPadName[i].Contains(mPADS[4]))	 readSHIELDPAD(i); //nyco maps to same controls as shield  

			filterAnalogs(i); //filter small return values 

		}



	}
	//============================================================================================

	//============================================================================================
	//use this function to filter out small analog values caused by poor self centering
	//============================================================================================
	void filterAnalogs(int id)
	{
		float anaFloor=0.2f;  //this value seems to work pretty well on my skanky xbox controller

		if(Mathf.Abs(mPadData[id].leftStickX)<anaFloor) 
			mPadData[id].leftStickX=0.0f;

		if(Mathf.Abs(mPadData[id].leftStickY)<anaFloor) 
			mPadData[id].leftStickY=0.0f;

		if(Mathf.Abs(mPadData[id].rightStickX)<anaFloor) 
			mPadData[id].rightStickX=0.0f;
		
		if(Mathf.Abs(mPadData[id].rightStickY)<anaFloor) 
			mPadData[id].rightStickY=0.0f;


	}
	//============================================================================================


	//============================================================================================
	//read the xbox 360 for windows controller
	//============================================================================================
	void readXBOX360(int id)
	{
		//http://docs.unity3d.com/Documentation/Manual/Input.html
		//		joystick Buttons (from any joystick): "joystick button 0", "joystick button 1", "joystick button 2", ...
		//			Joystick Buttons (from a specific joystick): "joystick 1 button 0", "joystick 1 button 1", "joystick 2 button 0", ...
		//          note they have to be named this in the input manager else it doesn't work
		//          Also we can't use "buttons from any joystick data" because we need to remap the various controllers before combining them!!

		string idString = (id+1).ToString(); //internally joysticks start at 1 not 0
				
		mPadData[id].butX 	= Input.GetButton("joystick "+idString+" button 2")? true: false; 
		mPadData[id].butY 	= Input.GetButton("joystick "+idString+" button 3")? true: false;
		mPadData[id].butA 	= Input.GetButton("joystick "+idString+" button 0")? true: false; 
		mPadData[id].butB 	= Input.GetButton("joystick "+idString+" button 1")? true: false;
		     
		mPadData[id].leftTop = Input.GetButton("joystick "+idString+" button 4")? true: false;
		mPadData[id].rightTop = Input.GetButton("joystick "+idString+" button 5")? true: false;

		mPadData[id].leftAsDown = Input.GetButton("joystick "+idString+" button 8")? true: false;
		mPadData[id].rightAsDown = Input.GetButton("joystick "+idString+" button 9")? true: false;

				
		mPadData[id].leftStickX = Input.GetAxis(idString+"_X axis");
		mPadData[id].leftStickY = Input.GetAxis(idString+"_Y axis");

		mPadData[id].rightStickX = Input.GetAxis(idString+"_4th axis");
		mPadData[id].rightStickY = Input.GetAxis(idString+"_5th axis");

		float dPadX = Input.GetAxis(idString+"_6th axis");
		mPadData[id].left 	= (dPadX<0)? true: false;
		mPadData[id].right 	= (dPadX>0)? true: false;
		
		float dPadY = Input.GetAxis(idString+"_7th axis");
		mPadData[id].up 	= (dPadY>0)? true: false;
		mPadData[id].down 	= (dPadY<0)? true: false;

		mPadData[id].leftTrigger=Input.GetAxis(idString+"_9th axis");
		mPadData[id].rightTrigger=Input.GetAxis(idString+"_10th axis");

		mPadData[id].pause = Input.GetButton("joystick "+idString+" button 7")? true: false; 

		    

	}
	//============================================================================================




	//============================================================================================
	//read the shield pad
	//============================================================================================
	void readSHIELDPAD(int id)
	{

		string idString = (id+1).ToString(); //internally joysticks start at 1 not 0
		
		
		//http://docs.unity3d.com/Documentation/Manual/Input.html
		//		joystick Buttons (from any joystick): "joystick button 0", "joystick button 1", "joystick button 2", ...
		//			Joystick Buttons (from a specific joystick): "joystick 1 button 0", "joystick 1 button 1", "joystick 2 button 0", ...
		//          note they have to be named this in the input manager else it doesn't work
		//          Also we can't use "buttons from any joystick data" because we need to remap the various controllers before combining them!!
		
		mPadData[id].butX 	= Input.GetButton("joystick "+idString+" button 2")? true: false; 
		mPadData[id].butY 	= Input.GetButton("joystick "+idString+" button 3")? true: false;
		mPadData[id].butA 	= Input.GetButton("joystick "+idString+" button 0")? true: false; 
		mPadData[id].butB 	= Input.GetButton("joystick "+idString+" button 1")? true: false;
		
		mPadData[id].leftTop = Input.GetButton("joystick "+idString+" button 4")? true: false;
		mPadData[id].rightTop = Input.GetButton("joystick "+idString+" button 5")? true: false;
		
		mPadData[id].leftAsDown = Input.GetButton("joystick "+idString+" button 8")? true: false;
		mPadData[id].rightAsDown = Input.GetButton("joystick "+idString+" button 9")? true: false;
		
		
		mPadData[id].leftStickX = Input.GetAxis(idString+"_X axis");
		mPadData[id].leftStickY = Input.GetAxis(idString+"_Y axis");
		
		mPadData[id].rightStickX = Input.GetAxis(idString+"_3rd axis");
		mPadData[id].rightStickY = Input.GetAxis(idString+"_4th axis");
		
		float dPadX = Input.GetAxis(idString+"_5th axis");
		mPadData[id].left 	= (dPadX<0)? true: false;
		mPadData[id].right 	= (dPadX>0)? true: false;
		
		float dPadY = Input.GetAxis(idString+"_6th axis");
		mPadData[id].up 	= (dPadY<0)? true: false;
		mPadData[id].down 	= (dPadY>0)? true: false;
		
		mPadData[id].leftTrigger=Input.GetAxis(idString+"_7th axis");
		mPadData[id].rightTrigger=Input.GetAxis(idString+"_8th axis");

		mPadData[id].pause = Input.GetButton("joystick "+idString+" button 10")? true: false; 

				
	}
	//============================================================================================




	//============================================================================================
	//============================================================================================
	string DebugPadData(int id)
	{
		string txtLine1= " ";

		txtLine1 += "X:"+ (isDown(id,ControllerButtons.BUTX) ? "1": "0");
		txtLine1 += " Y:"+ (isDown(id,ControllerButtons.BUTY) ? "1": "0");
		txtLine1 += " A:"+ (isDown(id,ControllerButtons.BUTA) ? "1": "0");
		txtLine1 += " B:"+ (isDown(id,ControllerButtons.BUTB) ? "1": "0");
		txtLine1 += " L: "+ (isDown(id,ControllerButtons.LEFT) ? "1": "0");
		txtLine1 += " R:"+ (isDown(id,ControllerButtons.RIGHT) ? "1": "0");
		txtLine1 += " U:"+ (isDown(id,ControllerButtons.UP) ? "1": "0");
		txtLine1 += " D:"+ (isDown(id,ControllerButtons.DOWN) ? "1": "0");
		txtLine1 += " LT:"+ (isDown(id,ControllerButtons.LEFTTOP) ? "1": "0");
		txtLine1 += " RT:"+ (isDown(id,ControllerButtons.RIGHTTOP) ? "1": "0");
		txtLine1 += " LAS:"+ (isDown(id,ControllerButtons.LAS_BUTTON) ? "1": "0");
		txtLine1 += " RAS:"+ (isDown(id,ControllerButtons.RAS_BUTTON) ? "1": "0");
		txtLine1 += " Pause:"+ (isDown(id,ControllerButtons.PAUSE) ? "1": "0");
		txtLine1+= "\n";
		txtLine1 += "LX:"+getAnalog(id,ControllerAnalogs.LEFTX).ToString();
		txtLine1 += " LY:"+getAnalog(id,ControllerAnalogs.LEFTY).ToString();
		txtLine1 += " RX:"+getAnalog(id,ControllerAnalogs.RIGHTX).ToString();
		txtLine1 += " RY:"+getAnalog(id,ControllerAnalogs.RIGHTY).ToString();
		txtLine1 += " LTRIG:"+getAnalog(id,ControllerAnalogs.LEFTTRIGGER).ToString();
		txtLine1 += " RTRIG:"+getAnalog(id,ControllerAnalogs.RIGHTTRIGGER).ToString();
		txtLine1 += "\n\n";
		return txtLine1;
	}
	//============================================================================================


	//============================================================================================
	//============================================================================================
	string DebugPadUnknown(int id)
	{
		string idString = (id+1).ToString(); //internally joysticks start at 1 not 0
		string txtLine1= " ";
		
		txtLine1 += " but0:" +(Input.GetButton("joystick "+idString+" button 0") ? "1": "0");
		txtLine1 += " but1:" +(Input.GetButton("joystick "+idString+" button 1") ? "1": "0");
		txtLine1 += " but2:" +(Input.GetButton("joystick "+idString+" button 2") ? "1": "0");
		txtLine1 += " but3:" +(Input.GetButton("joystick "+idString+" button 3") ? "1": "0");

		txtLine1 += " but4:" +(Input.GetButton("joystick "+idString+" button 4") ? "1": "0");
		txtLine1 += " but5:" +(Input.GetButton("joystick "+idString+" button 5") ? "1": "0");
		txtLine1 += " but6:" +(Input.GetButton("joystick "+idString+" button 6") ? "1": "0");
		txtLine1 += " but7:" +(Input.GetButton("joystick "+idString+" button 7") ? "1": "0");

		txtLine1 += " but8:" +(Input.GetButton("joystick "+idString+" button 8") ? "1": "0");
		txtLine1 += " but9:" +(Input.GetButton("joystick "+idString+" button 9") ? "1": "0");
		txtLine1 += " but10:" +(Input.GetButton("joystick "+idString+" button 10") ? "1": "0");
		txtLine1 += " but11:" +(Input.GetButton("joystick "+idString+" button 11") ? "1": "0");

		txtLine1 += " but12:" +(Input.GetButton("joystick "+idString+" button 12") ? "1": "0");
		txtLine1 += " but13:" +(Input.GetButton("joystick "+idString+" button 13") ? "1": "0");
		txtLine1 += " but14:" +(Input.GetButton("joystick "+idString+" button 14") ? "1": "0");
		txtLine1 += " but15:" +(Input.GetButton("joystick "+idString+" button 15") ? "1": "0");

		txtLine1 += " but16:" +(Input.GetButton("joystick "+idString+" button 16") ? "1": "0");
		txtLine1 += " but17:" +(Input.GetButton("joystick "+idString+" button 17") ? "1": "0");
		txtLine1 += " but18:" +(Input.GetButton("joystick "+idString+" button 18") ? "1": "0");
		txtLine1 += " but19:" +(Input.GetButton("joystick "+idString+" button 19") ? "1": "0");
		txtLine1 += "\n";
		
		txtLine1 += " X axis:" +Input.GetAxis(idString+"_X axis");  
		txtLine1 += " Y axis:" +Input.GetAxis(idString+"_Y axis");
		txtLine1 += " 3rd axis:" +Input.GetAxis(idString+"_3rd axis");
		txtLine1 += " 4th axis:" +Input.GetAxis(idString+"_4th axis");
		txtLine1 += " 5th axis:" +Input.GetAxis(idString+"_5th axis");
		txtLine1 += " 6th axis:" +Input.GetAxis(idString+"_6th axis");
		txtLine1 += " 7th axis:" +Input.GetAxis(idString+"_7th axis");
		txtLine1 += " 8th axis:" +Input.GetAxis(idString+"_8th axis");
		txtLine1 += " 9th axis:" +Input.GetAxis(idString+"_9th axis");
		txtLine1 += "10th axis:" +Input.GetAxis(idString+"_10th axis");
		txtLine1 += "\n\n";

		return txtLine1;
	}
	//============================================================================================
      



	//============================================================================================
	//============================================================================================
	 void OnGUI() 
	{

#if DEBUG_CONTROLLER
		var textArea = new Rect(10,10,Screen.width, Screen.height);
		string txtLine1;

		txtLine1= "PadController Version:"+mVersion;
		txtLine1+= "    Controllers Attached :"+mActivePadCount+"\n";
		txtLine1+= "============================================\n";

		for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and output data
		{
			txtLine1+= i.ToString()+" "+mPadName[i]+"\n";
			if(KnownPad(i)) // we know this type of pad so display the formatted data
				txtLine1+= DebugPadData(i);
			else
			{
				txtLine1+= "+++DONT HAVE A HANDLER FOR THIS GAMEPAD+++\n";
				txtLine1+= DebugPadUnknown(i); //don't recognize this pad so output packet data
				
			}
		}

		//prints out the unparsed raw joystick data
		//for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and output data
		//{
		//	txtLine1+= i.ToString()+" "+mPadName[i]+"\n";
		//	txtLine1+= DebugPadUnknown(i); 
		//}


		GUI.Label(textArea,txtLine1);
#endif

	}
	

	//============================================================================================
	//detects if a button is down, this is just a helper function to make code cleaner, pulls data from the absracted controller
	//============================================================================================
	public bool isDown(int id,ControllerButtons b)
	{
		switch(b)
		{
		case ControllerButtons.LAS_BUTTON:
			return(mPadData[id].leftAsDown);

		case ControllerButtons.RAS_BUTTON:
			return(mPadData[id].rightAsDown);
			
		case ControllerButtons.BUTX:
			return(mPadData[id].butX);
			     
		case ControllerButtons.BUTY:
			return(mPadData[id].butY);
			
		case ControllerButtons.BUTA:
			return(mPadData[id].butA);
			
		case ControllerButtons.BUTB:
			return(mPadData[id].butB);
			
		case ControllerButtons.UP:
			return(mPadData[id].up);
			
		case ControllerButtons.DOWN:
			return(mPadData[id].down);
			
		case ControllerButtons.LEFT:
			return(mPadData[id].left);

		case ControllerButtons.RIGHT:
			return(mPadData[id].right);
		
		case ControllerButtons.LEFTTOP:
			return(mPadData[id].leftTop);
		
		case ControllerButtons.RIGHTTOP:
			return(mPadData[id].rightTop);

		case ControllerButtons.PAUSE:
			return(mPadData[id].pause);

		default:  
			break;
		}
	return false;
	}
	
	//============================================================================================
	//detects a momementary button press (e.g debounced), helper pulls data from the abstracted controller data.
	//============================================================================================
	public bool isPressed(int id,ControllerButtons b)
	{
		bool curr,old;
		
		switch(b)
		{
		case ControllerButtons.LAS_BUTTON:
			curr = mPadData[id].leftAsDown;
			old =  !mPadDataOld[id].leftAsDown;
			return(curr&&old);
			
		case ControllerButtons.RAS_BUTTON:
			curr = mPadData[id].rightAsDown;
			old =  !mPadDataOld[id].rightAsDown;
			return(curr&&old);
			
		case ControllerButtons.BUTX:
			curr = mPadData[id].butX;
			old =  !mPadDataOld[id].butX;
			return(curr&&old);
			
		case ControllerButtons.BUTY:
			curr = mPadData[id].butY;
			old =  !mPadDataOld[id].butY;
			return(curr&&old);
			
		case ControllerButtons.BUTA:
			curr = mPadData[id].butA;
			old =  !mPadDataOld[id].butA;
			return(curr&&old);
			
		case ControllerButtons.BUTB:
			curr = mPadData[id].butB;
			old =  !mPadDataOld[id].butB;
			return(curr&&old);
			
		case ControllerButtons.UP:
			curr = mPadData[id].up;
			old =  !mPadDataOld[id].up;
			return((curr&&old)?true:false);
			
		case ControllerButtons.DOWN:
			curr = mPadData[id].down;
			old =  !mPadDataOld[id].down;
			return(curr&&old);
			
		case ControllerButtons.LEFT:
			curr = mPadData[id].left;
			old =  !mPadDataOld[id].left;
			return((curr&&old)?true:false);

		case ControllerButtons.RIGHT:
			curr = mPadData[id].right;
			old =  !mPadDataOld[id].right;
			return(curr&&old);
		
		case ControllerButtons.LEFTTOP:
			curr = mPadData[id].leftTop;
			old =  !mPadDataOld[id].leftTop;
			return(curr&&old);
		
		case ControllerButtons.RIGHTTOP:
			curr = mPadData[id].rightTop;
			old =  !mPadDataOld[id].rightTop;
			return(curr&&old);

		case ControllerButtons.PAUSE:
			curr = mPadData[id].pause;
			old =  !mPadDataOld[id].pause;
			return(curr&&old);

		default:  
			break;
		}
	return false;
	}

	//============================================================================================
	//return an analog channel
	//============================================================================================
	public float getAnalog(int id, ControllerAnalogs c)
	{
		switch(c)
		{
		case ControllerAnalogs.LEFTX:
			return mPadData[id].leftStickX;
		case ControllerAnalogs.LEFTY:
			return mPadData[id].leftStickY;
		case ControllerAnalogs.RIGHTX:
			return mPadData[id].rightStickX;  
		case ControllerAnalogs.RIGHTY:
			return mPadData[id].rightStickY;
		case ControllerAnalogs.LEFTTRIGGER:
			return mPadData[id].leftTrigger;
		case ControllerAnalogs.RIGHTTRIGGER:
			return mPadData[id].rightTrigger;
		default:
				break;
		}
		
		return 0;
	}


	//============================================================================================
	// detects is a button is pressed on any controller, e.g. for a single player game that has 
	// several controllers attached, this lets you treat them all as the same controller
	//============================================================================================
	public bool isDown(ControllerButtons b)
	{
		for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and output data
		{
			if(isDown(i,b)) 
				return true;
		}
		return false;
	}
	
	//============================================================================================
	// detects is a button is down on any controller, e.g. for a single player game that has 
	// several controllers attached, this lets you treat them all as the same controller
	//============================================================================================
	public bool isPressed(ControllerButtons b)
	{
		for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and output data
		{
			if(isPressed(i,b)) 
				return true;
		}
		return false;
	}


	//============================================================================================
	//return an analog channel on any controller by summing them
	//e.g. for a single player game that has 
	// several controllers attached, this lets you treat them all as the same controller
	//============================================================================================
	public float getAnalog(ControllerAnalogs c)
	{
		float val = 0;
		for(int i=0; i<mActivePadCount; i++) //iterate through attached controllers and output data
		{
			val +=getAnalog(i,c); 
		}

		//clamp the summed output to legal values
		if(val<-1.0f) val = -1.0f;
		if(val> 1.0f) val =  1.0f;

		return val;
	}


}
//============================================================================================







