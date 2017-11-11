using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenuController : MonoBehaviour {

	public void OnStart() {
		SceneManager.LoadScene(GameController.GetLevelScene());
	}

	public void OnCredits() {
		SceneManager.LoadScene(GameController.CREDITS_SCENE);
	}
}
