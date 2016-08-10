using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
	public float minimumDisplayTime = 2f;
	public string[] addedScenes;
	GameObject[] loadingSceneObjects;

	void Awake()
	{
		DontDestroyOnLoad (this);
		loadingSceneObjects = FindObjectsOfType<GameObject> ();
		StartCoroutine (HelpfulHints ());
		StartCoroutine (LoadLevel ());
	}

	IEnumerator HelpfulHints()
	{
		yield return null;
	}

	IEnumerator LoadLevel()
	{
		yield return new WaitForSeconds(minimumDisplayTime);

		// Load next level
		Application.LoadLevel (Game.map.sceneName);
		foreach(string sceneName in addedScenes)
			Application.LoadLevelAdditive(sceneName);
		yield return new WaitForFixedUpdate (); // Wait for scene to change

		// Start the game up
		Game.StartRound ();

		// Clean up
		foreach(GameObject gameObj in loadingSceneObjects)
		{
			Destroy(gameObj);
		}
	}
}
