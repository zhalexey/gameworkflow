using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {


	public void OnBack ()
	{
		SceneManager.LoadScene (GameController.START_MENU_SCENE);
	}

}
