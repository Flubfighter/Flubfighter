using UnityEngine;
using System.Collections;

public class CharacterSpawnTester : MonoBehaviour
{
	public Character characterPrefab;
	public Item itemPrefab;

	ItemSpawnPoint itemSpawnPoint;

	void Start()
	{
		if (!Application.isEditor)
			DestroyImmediate (gameObject);
		Player.One.team = Assets.Teams[0];
		Player.Two.team = Assets.Teams[1];
		Player.Three.team = Assets.Teams[2];
		Player.Four.team = Assets.Teams[3];
	}

	void OnGUI()
	{
		if(GUILayout.Button("Spawn 1"))
		{
			Character character = SpawnPoint.FindSafeSpawnPoint(Player.One.team).Spawn (Player.One, characterPrefab);
		}

		if(GUILayout.Button("Spawn 2"))
		{
			Character character = SpawnPoint.FindSafeSpawnPoint(Player.Two.team).Spawn (Player.Two, characterPrefab);
		}

		if(GUILayout.Button("Spawn 3"))
		{
			Character character = SpawnPoint.FindSafeSpawnPoint(Player.Three.team).Spawn (Player.Three, characterPrefab);
		}

		if(GUILayout.Button("Spawn 4"))
		{
			Character character = SpawnPoint.FindSafeSpawnPoint(Player.Four.team).Spawn (Player.Four, characterPrefab);
		}

		if (GUILayout.Button ("Spawn Item"))
			ItemSpawnPoint.FindOpenSpawnPoint ().Spawn (itemPrefab);

		if(GUILayout.Button ("Kill 1 and Respawn"))
		{
			StartCoroutine(KillAndRespawn(Player.One, 3f));
		}

		if(GUILayout.Button ("Kill 2 and Respawn"))
		{
			StartCoroutine(KillAndRespawn(Player.Two, 3f));
		}

		if(GUILayout.Button ("Kill 3 and Respawn"))
		{
			StartCoroutine(KillAndRespawn(Player.Three, 3f));
		}

		if(GUILayout.Button ("Kill 4 and Respawn"))
		{
			StartCoroutine(KillAndRespawn(Player.Four, 3f));
		}

		if(GUILayout.Button("Spawn All Teams"))
		{
			SpawnPoint.FindSafeSpawnPoint(Player.One.team).Spawn (Player.One, characterPrefab);
			SpawnPoint.FindSafeSpawnPoint(Player.Two.team).Spawn (Player.Two, characterPrefab);
			SpawnPoint.FindSafeSpawnPoint(Player.Three.team).Spawn (Player.Three, characterPrefab);
			SpawnPoint.FindSafeSpawnPoint(Player.Four.team).Spawn (Player.Four, characterPrefab);
		}
		
		if(GUILayout.Button ("Kill All"))
		{
			foreach(Character character in FindObjectsOfType<Character>())
				character.Die(DeathType.Generic);
		}
	}

	IEnumerator KillAndRespawn(Player player, float delay)
	{
		player.playableCharacter.Die (DeathType.Generic);
		yield return new WaitForSeconds (delay);
		SpawnPoint.FindSafeSpawnPoint (player.team).Spawn (player, characterPrefab);
	}

}
