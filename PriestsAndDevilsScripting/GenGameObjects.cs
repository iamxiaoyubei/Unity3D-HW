using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenGameObjects : MonoBehaviour {

	SSDirector director;

	// Use this for initialization
	void Start () {
		Debug.Log("gen start!!");
		director = SSDirector.getInstance();
		director.currentSceneController.setGenGameObjects(this);
		LoadResources();
	}

	// the position to create
	GameObject lShore, rShore, boat_obj;
	Vector3 lShorePos = new Vector3 (0, 0, -12);
	Vector3 rShorePos = new Vector3(0, 0, 12);  
	Vector3 lBoatPos = new Vector3(0, 0, -4);  
	Vector3 rBoatPos = new Vector3(0, 0, 4);

	// the pos of priest and devils in a shore
	Vector3 priestStartPos = new Vector3(0, 2.5f, -11f);  
	Vector3 priestEndPos = new Vector3(0, 2.5f, 8f);  
	Vector3 devilStartPos = new Vector3(0, 2.5f, -16f);  
	Vector3 devilEndPos = new Vector3(0, 2.5f, 13f); 

	Vector3 leftBoatPos = new Vector3(0, 1.2f, -0.55f);  
    Vector3 rightBoatPos = new Vector3(0, 1.2f, 0.55f);  	

	//  GameOjects
	Stack<GameObject> lPriests = new Stack<GameObject> ();
	Stack<GameObject> rPriests = new Stack<GameObject> ();
	Stack<GameObject> lDevils = new Stack<GameObject> ();
	Stack<GameObject> rDevils = new Stack<GameObject> ();

	//  boat
	GameObject[] boat = new GameObject[2];
	int side = 1;               // side records where boat docks  
    public float speed = 5f; 
	float gap = 1.5f;

	public void LoadResources() {
		Debug.Log("Load resources!!");
		//  the instantiation of shore
		lShore = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/shore"),
			lShorePos, Quaternion.identity);

		rShore = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/shore"),
			rShorePos, Quaternion.identity);

		//  the instantiation of boat
		boat_obj = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/boat"),
			lBoatPos, Quaternion.identity) as GameObject;

		for (int i = 0; i < 3; i++) {
			/*
			lDevils.Push(Instantiate<GameObject> (
				Resources.Load<GameObject> ("Prefabs/devil")));

			lPriests.Push(Instantiate<GameObject> (
				Resources.Load<GameObject> ("Prefabs/priest")));
			*/
			GameObject priest = Instantiate(Resources.Load("Prefabs/Priest")) as GameObject;  
            priest.transform.position = getCharacterPosition(priestStartPos, i);  
            priest.tag = "Priest";  
            lPriests.Push(priest);  
            GameObject devil = Instantiate(Resources.Load("Prefabs/Devil")) as GameObject;  
            devil.transform.position = getCharacterPosition(devilStartPos, i);  
            devil.tag = "Devil";  
            lDevils.Push(devil);  
		}

		Debug.Log ("load resource ...\n");
	}

	//  calculate the capacity of boat now
	int boatCapacity() {
		int capacity = 0;
		for (int i = 0; i < 2; i++)
			if (boat [i] == null)
				capacity++;
		return capacity;
	}

	// some actions be done
	void goToTheBoat(GameObject obj) {
		if (boatCapacity() > 0) {
			obj.transform.parent = boat_obj.transform;
			Vector3 target = new Vector3();  
			if (boat[0] == null) {  
				boat[0] = obj;  
				target = boat_obj.transform.position + leftBoatPos;  
				//obj.transform.localPosition = new Vector3(0, 1.2f, -0.3f);  
			} else {  
				boat[1] = obj;  
				target = boat_obj.transform.position + rightBoatPos;  
				//obj.transform.localPosition = new Vector3(0, 1.2f, 0.3f);  
			}  
			ActionManager.GetInstance().ApplyMoveToYZAction(obj, target, speed);  
		}  
	}

	public void lPriestGoBoat() {  
		if (lPriests.Count != 0 && boatCapacity() != 0 && side == 1)  
			goToTheBoat(lPriests.Pop());  
	}  

	public void rPriestGoBoat() {  
		if (rPriests.Count != 0 && boatCapacity() != 0 && side == 2)  
			goToTheBoat(rPriests.Pop());  
	}  

	public void lDevilGoBoat() {  
		if (lDevils.Count != 0 && boatCapacity() != 0 && side == 1)  
			goToTheBoat(lDevils.Pop());  
	}  

	public void rDevilGoBoat() {  
		if (rDevils.Count != 0 && boatCapacity() != 0 && side == 2)  
			goToTheBoat(rDevils.Pop());  
	}  

	public void getOffTheBoat(int bside) {  
		if (boat[bside] != null) {  
			boat[bside].transform.parent = null;  
			Vector3 target = new Vector3(); 
			if (side == 1) {  
				if (boat[bside].tag == "Priest") { 
				lPriests.Push(boat[bside]);  
                    target = getCharacterPosition(priestStartPos, lPriests.Count - 1);   
					//lPriests.Push(boat[side]);  
				}  
				else if (boat[bside].tag == "Devil") {  
					//lDevils.Push(boat[side]);  
					lDevils.Push(boat[bside]);  
                    target = getCharacterPosition(devilStartPos, lDevils.Count - 1);  
				}  
			}  
			else if (side == 2) {  
				if (boat[bside].tag == "Priest") {  
					//rPriests.Push(boat[side]);  
					rPriests.Push(boat[bside]);  
                    target = getCharacterPosition(priestEndPos, rPriests.Count - 1);  
				}  
				else if (boat[bside].tag == "Devil") {  
					//rDevils.Push(boat[side]);  
					rDevils.Push(boat[bside]);  
                    target = getCharacterPosition(devilEndPos, rDevils.Count - 1);  
				}  
			}  
			ActionManager.GetInstance().ApplyMoveToYZAction(boat[bside], target, speed);  
			boat[bside] = null;  
		}  
	}  

	public void lOffBoat() {
		getOffTheBoat(0);
	}

	public void rOffBoat() {
		getOffTheBoat(1);
	}

	public void boatMove() {  
		Debug.Log("boat Moving!!");
		print(boatCapacity());
		if (boatCapacity() <= 1) {  
			Debug.Log("i am here!!");
			if (side == 1) {  
				Debug.Log("Ready to Go!!");
				ActionManager.GetInstance().ApplyMoveToAction(boat_obj, rBoatPos, speed);  
                side = 2;    
			}  
			else if (side == 2) {  
				ActionManager.GetInstance().ApplyMoveToAction(boat_obj, lBoatPos, speed);  
                side = 1;  
			}  
		}  
	}  

	//  to calculate the position of the priests and devils
	void setCharacterPositions(Stack<GameObject> stack, Vector3 pos) {  
		GameObject[] array = stack.ToArray();  
		for (int i = 0; i < stack.Count; ++i) {  
			array[i].transform.position = new Vector3(pos.x, pos.y, pos.z + gap*i);  
		}  
	}  

	Vector3 getCharacterPosition(Vector3 pos, int index) {  
        return new Vector3(pos.x, pos.y, pos.z + gap*index);  
    }  


	//  to check whether this game is succeed or fail
	void check() {  
		int pOnb = 0, dOnb = 0;  
		int priests_l = 0, devils_l = 0, priests_r = 0, devils_r = 0;  

		if (rPriests.Count == 3 && rDevils.Count == 3) {  
			director.currentGameStatus.setMessage("Win!");    
			return;  
		}  

		for (int i = 0; i < 2; ++i) {  
			if (boat[i] != null && boat[i].tag == "Priest") pOnb++;  
			else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;  
		}  
		if (side == 1) {  
			priests_l = lPriests.Count + pOnb;  
			devils_l = lDevils.Count + dOnb;  
			priests_r = rPriests.Count;  
			devils_r = rDevils.Count;  
		}  
		else if (side == 2) {  
			priests_l = lPriests.Count;  
			devils_l = lDevils.Count;  
			priests_r = rPriests.Count + pOnb;  
			devils_r = rDevils.Count + dOnb;  
		}  
		if ((priests_l != 0 && priests_l < devils_l) || (priests_r != 0 && priests_r < devils_r)) {  
			director.currentGameStatus.setMessage("Lose!");  
		}  
	}  

	void Update() {  
		//Debug.Log("Update now!!");
		/*
		setCharacterPositions(lPriests, priestStartPos);  
		setCharacterPositions(rPriests, priestEndPos);  
		setCharacterPositions(lDevils, devilStartPos);  
		setCharacterPositions(rDevils, devilEndPos);  

		if (director.state == State.LTRMOVING) {  
			boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, rBoatPos, speed*Time.deltaTime);  
			if (boat_obj.transform.position == rBoatPos) {  
				director.state = State.RIGHT;  
			}  
		}  
		else if (director.state == State.RTLMOVING) {  
			boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, lBoatPos, speed*Time.deltaTime);  
			if (boat_obj.transform.position == lBoatPos) {  
				director.state = State.LEFT;  
			}  
		}  
		else check();  
		*/
		check(); 
	}  
	
}
