using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueryGameStatus {

	bool isMoving();  
    void setMoving(bool state);  
    string isMessage();  
    void setMessage(string message);  
}
