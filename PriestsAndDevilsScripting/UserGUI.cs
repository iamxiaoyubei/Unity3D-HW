using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

	private IUserAction action;
	SSDirector director;
	IQueryGameStatus state;  

	public string gameName = "Priests and Devils";  
	public string gameRule = "Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many ways. Keep all priests alive! Good luck!             Sphere -- Priest    Cube -- Devil";  

	void Start() {
		Debug.Log("Start");
		director = SSDirector.getInstance ();
		action = SSDirector.getInstance().currentSceneController as IUserAction;
		state = SSDirector.getInstance().currentGameStatus as IQueryGameStatus; 
	}

	float width, height; 

	float calcuX(float scale) {  
		return (Screen.width - width) / scale;  
	}  

	float calcuY(float scale) {  
		return (Screen.height - height) / scale;  
	}  

	void OnGUI() {
		width = Screen.width / 12;
		height = Screen.height / 12;

		string message = state.isMessage();  
 
		if (message != "") {  
			if (GUI.Button(new Rect(calcuX(2f), calcuY(6f), width, height), message)) {  
				action.restart();  
			}  
		}   
		else {  
			if (GUI.RepeatButton(new Rect(10, 10, 120, 20), gameName)) {  
				GUI.TextArea(new Rect(10, 40, Screen.width - 20, Screen.height/2), gameRule);  
			}  
			else if (!state.isMoving()) {  
				if (GUI.Button(new Rect(calcuX(2f), calcuY(6f), width, height), "Go")) {  
					action.boatMove();  
				}  
				if (GUI.Button(new Rect(calcuX(10.5f), calcuY(4f), width, height), "On")) {  
					action.lDevilGoBoat();  
				}  
				if (GUI.Button(new Rect(calcuX(4.3f), calcuY(4f), width, height), "On")) {  
					action.lPriestGoBoat();  
				}  
				if (GUI.Button(new Rect(calcuX(1.1f), calcuY(4f), width, height), "On")) {  
					action.rDevilGoBoat();  
				}  
				if (GUI.Button(new Rect(calcuX(1.3f), calcuY(4f), width, height), "On")) {  
					action.rPriestGoBoat();  
				}  
				if (GUI.Button(new Rect(calcuX(2.5f), calcuY(1.3f), width, height), "Off")) {  
					action.lOffBoat();  
				}  
				if (GUI.Button(new Rect(calcuX(1.7f), calcuY(1.3f), width, height), "Off")) {  
					action.rOffBoat();  
				}  
			}  
		}  
	}  
}  