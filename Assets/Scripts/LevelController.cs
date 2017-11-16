using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
	private const float BETWEEN_LEVEL_DELAY = 1f;

	public List<GameObject> prehistoryPages;
	public List<GameObject> winPages;
	public List<GameObject> loosePages;

	public GameObject gameManager;

	private GameObject pageInstance;


	void Start ()
	{
		StartCoroutine (ShowPrehistoryDemo ());
	}


	IEnumerator ShowPrehistoryDemo ()
	{
		if (PlayerController.IsNotPrehistoryViewed ()) {
			foreach (GameObject page in prehistoryPages) {
				yield return ShowPage (page);
			}
			PlayerController.SetPrehistoryViewed ();
		}
		Instantiate (gameManager);
	}

	public void SkipDemo ()
	{
		Destroy (pageInstance);
		Instantiate (gameManager);
	}

	IEnumerator ShowPage (GameObject page)
	{
		pageInstance = Instantiate (page, new Vector2 (0, 0), Quaternion.identity);
		AddSkipButtonHandler ();
		AudioSource audio = pageInstance.GetComponent<AudioSource> ();
		audio.Play ();
		yield return new WaitForSeconds (audio.clip.length);
		Destroy (pageInstance, BETWEEN_LEVEL_DELAY);
	}


	private void AddSkipButtonHandler() {
		Button btnSkip = pageInstance.GetComponentInChildren<Button> ();
		if (btnSkip != null) {
			btnSkip.onClick.AddListener (() => SkipDemo());
		}
	}

	public delegate void CallbackFunction ();

	public void ShowWinDemo (CallbackFunction callback)
	{
		StartCoroutine (ShowDemo (winPages, callback));
	}

	public void ShowLooseDemo (CallbackFunction callback)
	{
		StartCoroutine (ShowDemo (loosePages, callback));
	}

	IEnumerator ShowDemo (List<GameObject> pages, CallbackFunction callback)
	{
		foreach (GameObject page in pages) {
			yield return ShowPage (page);
			if (callback != null) {
				callback ();
			}
		}
	}
}
