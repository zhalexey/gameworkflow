using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour {

	public void OnSkip() {
		ScriptManager.LevelController.SkipDemo ();
	}
}
