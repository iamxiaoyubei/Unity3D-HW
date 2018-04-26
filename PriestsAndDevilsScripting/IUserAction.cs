using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {
	void GameOver();
	void lPriestGoBoat ();
	void rPriestGoBoat ();
	void lDevilGoBoat ();
	void rDevilGoBoat ();
	void lOffBoat ();
	void rOffBoat ();
	void boatMove ();
	void restart ();
}
