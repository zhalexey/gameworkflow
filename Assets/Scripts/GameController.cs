using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static int MAX_LEVEL = 2;
	private const string LEVEL_SCENE = "Level";
	public const string START_MENU_SCENE = "StartMenu";
	public const string CREDITS_SCENE = "Credits";
	public const string MAP_SCENE = "Map";

	enum State
	{
		WIN,
		LOOSE}

	;

	private State gameState;


	public void OnWin ()
	{
		gameState = State.WIN;
		ScriptManager.LevelController.ShowWinDemo (OnCallback);
	}

	public void OnLoose ()
	{
		gameState = State.LOOSE;
		ScriptManager.LevelController.ShowLooseDemo (OnCallback);
	}

	public void OnCallback ()
	{
		if (State.WIN == gameState) {
			if (PlayerController.HasAchievedLevel(MAX_LEVEL)) {
				PlayerController.ResetLevel ();
				SceneManager.LoadScene (CREDITS_SCENE);
				return;
			}
			PlayerController.NextLevel ();
		}
		SceneManager.LoadScene (MAP_SCENE);
	}

	public static string GetLevelScene ()
	{
		return LEVEL_SCENE + PlayerController.levelNumber;
	}

}
