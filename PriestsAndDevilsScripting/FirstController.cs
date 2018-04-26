using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { LEFT, LTRMOVING, RTLMOVING, RIGHT, WIN, LOSE };  

public class FirstController : MonoBehaviour, ISceneController, IUserAction, IQueryGameStatus {

	SSDirector director;
	private GenGameObjects genGameObjects;

	void Awake() {
		director = SSDirector.getInstance ();
		director.currentSceneController = this;
		director.currentGameStatus = this;
		//director.currentSceneController.LoadResources ();
	}

	public void setGenGameObjects(GenGameObjects obj) {
		if (null == genGameObjects) {
			genGameObjects = obj;
		}
	}

	public void Resume() {
	}

	public void Pause() {
	}

	public void GameOver() {
		//	SSDirector.getInstance ().NextScene ();
	}

	public void LoadResources() {
		genGameObjects.LoadResources();
	}

	public void lPriestGoBoat () {
		genGameObjects.lPriestGoBoat();
	}

	public void rPriestGoBoat () {
		genGameObjects.rPriestGoBoat();
	}

	public void lDevilGoBoat () {
		genGameObjects.lDevilGoBoat();
	}

	public void rDevilGoBoat () {
		genGameObjects.rDevilGoBoat();
	}

	public void lOffBoat () {
		genGameObjects.lOffBoat();
	}

	public void rOffBoat () {
		genGameObjects.rOffBoat();
	}

	public void boatMove () {
		genGameObjects.boatMove();
	}

	public void restart() {
		Application.LoadLevel(Application.loadedLevelName);  
		director.moving = false;  
        director.message = ""; 
	}

	// IQueryGameStatus  
    public bool isMoving() { return director.moving; }  
    public void setMoving(bool state) { director.moving = state; }  
    public string isMessage() { return director.message; }  
    public void setMessage(string message) { director.message = message; }  
}

