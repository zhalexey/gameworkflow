using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static int levelNumber = 1;
	public static int levelMaxAchieved = 1;
	private static bool[] prehistoryViewed = new bool[GameController.MAX_LEVEL];

	public static bool IsNotPrehistoryViewed() {
		return !prehistoryViewed [levelNumber - 1];
	}

	public static void SetPrehistoryViewed() {
		prehistoryViewed [levelNumber - 1] = true;
	}

	public static void ResetLevel() {
		levelNumber = 1;
	}

	public static void NextLevel() {
		levelNumber++;
		levelMaxAchieved++;
	}

	public static bool HasAchievedLevel(int level) {
		return levelNumber == level;
	}

}
