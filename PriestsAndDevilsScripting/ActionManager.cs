using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IU3dActionCompleted {  
   void OnActionCompleted (U3dAction action);  
}  

public class ActionManager : System.Object {
	private static ActionManager _instance;

	public static ActionManager GetInstance(){  
	    if (_instance == null) {  
	        _instance = new ActionManager();  
	    }  
	    return _instance;  
	}  

	// ApplyMoveToAction  
	public U3dAction ApplyMoveToAction(GameObject obj, Vector3 target, float speed, IU3dActionCompleted completed){  
	    MoveToAction ac = obj.AddComponent <MoveToAction> ();  
	    ac.setting (target, speed, completed);  
	    return ac;  
	}  
	  
	public U3dAction ApplyMoveToAction(GameObject obj, Vector3 target, float speed) {  
	    return ApplyMoveToAction (obj, target, speed, null);  
	}  

	// ApplyMoveToYZAction  
	public U3dAction ApplyMoveToYZAction(GameObject obj, Vector3 target, float speed, IU3dActionCompleted completed){  
	    MoveToYZAction ac = obj.AddComponent <MoveToYZAction> ();  
	    ac.setting (obj, target, speed, completed);  
	    return ac;  
	}  
	  
	public U3dAction ApplyMoveToYZAction(GameObject obj, Vector3 target, float speed) {  
	    return ApplyMoveToYZAction (obj, target, speed, null);  
	}  
}

public class U3dActionException : System.Exception {}  
      
public class U3dAction : MonoBehaviour {  
    public void Free() {  
        Destroy(this);  
    }  
}  
  
public class U3dActionAuto : U3dAction {}  
  
public class U3dActionMan : U3dAction {}  
  
public class MoveToAction :  U3dActionAuto {  
    public Vector3 target;  
    public float speed;  
      
    private IU3dActionCompleted monitor = null;  
      
    public void setting(Vector3 target, float speed, IU3dActionCompleted monitor){  
        this.target = target;  
        this.speed = speed;  
        this.monitor = monitor;  
        SSDirector.getInstance().currentGameStatus.setMoving(true);  
    }  
      
    void Update () {  
        float step = speed * Time.deltaTime;  
        transform.position = Vector3.MoveTowards(transform.position, target, step);  

        // Auto Destroy After Completed  
        if (transform.position == target) {   
            SSDirector.getInstance().currentGameStatus.setMoving(false);  
            if (monitor != null) {  
                monitor.OnActionCompleted(this);  
            }  
            Destroy(this);  
        }  
    }  
}  

/* MoveToYZAction is a combination of two MoveToAction(s) 
 * It moves on a single shaft(Y or Z) each time 
 */  
public class MoveToYZAction : U3dActionAuto, IU3dActionCompleted {  
    public GameObject obj;  
    public Vector3 target;  
    public float speed;  

    private IU3dActionCompleted monitor = null;  

    public void setting(GameObject obj, Vector3 target, float speed, IU3dActionCompleted monitor) {  
        this.obj = obj;  
        this.target = target;  
        this.speed = speed;  
        this.monitor = monitor;  
        SSDirector.getInstance().currentGameStatus.setMoving(true);  

        /* If obj is higher than target, move to target.z first, then move to target.y 
         * If obj is lower than target, move to target.y first, then move to target.z 
         * The turn is implemented through callback function 
         */  
        if (target.y < obj.transform.position.y) {  
            Vector3 targetZ = new Vector3(target.x, obj.transform.position.y, target.z);  
            ActionManager.GetInstance().ApplyMoveToAction(obj, targetZ, speed, this);  
        } else {  
            Vector3 targetY = new Vector3(target.x, target.y, obj.transform.position.z);  
            ActionManager.GetInstance().ApplyMoveToAction(obj, targetY, speed, this);  
        }  
    }  

    // Implement the turn  
    public void OnActionCompleted (U3dAction action) {  
        // Note not calling this callback again!  
        ActionManager.GetInstance().ApplyMoveToAction(obj, target, speed, null);  
    }  

    void Update() {  
        // Auto Destroy After Completed  
        if (obj.transform.position == target) {   
            SSDirector.getInstance().currentGameStatus.setMoving(false);  
            if (monitor != null) {  
                monitor.OnActionCompleted(this);  
            }  
            Destroy(this);  
        }  
    }  
}  
