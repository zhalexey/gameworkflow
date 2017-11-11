using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager {

	public static LevelController LevelController
	{
		get{
			return (LevelController)GameObject.Find ("LevelManager").GetComponent ("LevelController");
		}
	}

	public static GameController GameController
	{
		get{
			return (GameController)GameObject.Find ("GameManager").GetComponent ("GameController");
		}
	}

}
