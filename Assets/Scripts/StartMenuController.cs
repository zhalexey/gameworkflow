using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenuController : MonoBehaviour {

	public void OnStart() {
		SceneManager.LoadScene (GameController.MAP_SCENE);
	}

	public void OnCredits() {
		SceneManager.LoadScene(GameController.CREDITS_SCENE);
	}
}
