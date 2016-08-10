using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
	public Transform spawnPoint;
	public ParticleSystem particle;
	private HashSet<Character> inCollisionList = new HashSet<Character>();
	private bool hasSpawnedSomeoneThisFrame = false;

//	void OnDrawGizmos()
//	{
//		Gizmos.color = IsSafe () ? Color.green : Color.red;
//		Gizmos.DrawSphere (transform.position, 1f);
//	}

	public bool IsSafe(Team spawningTeam=null)
	{
		if(hasSpawnedSomeoneThisFrame)
			return false;
		if(spawningTeam == null)
			return inCollisionList.Count == 0;
//		Debug.Log (inCollisionList.Count);
		foreach(Character c in inCollisionList)
		{
			if(c.Player != null)
				if(c.Player.team)
					if(c.Player.team != spawningTeam)
						return false;
		}
		return true;
	}

	public Character Spawn(Player player)
	{
		return Spawn (player, Assets.characterPrefab);
	}

	public Character Spawn(Player player, Character prefab)
	{
		hasSpawnedSomeoneThisFrame = true;
		Character character = Instantiate (prefab, transform.position, transform.rotation) as Character;
		player.SetPlayableCharacter(character);
		character.SetPlayer (player);
		particle.SetStartColorRecursively (player.team.particleColor);
		particle.Play ();
		return character;
	}

	public static SpawnPoint FindSafeSpawnPoint(Team spawningTeam=null)
	{
		SpawnPoint[] allPoints = FindObjectsOfType<SpawnPoint> ();
		SpawnPoint selected = null;
		int sanity = 100;
		do
		{
			 selected = allPoints[Random.Range(0, allPoints.Length)];
		}
		while(!selected.IsSafe(spawningTeam) && --sanity > 0);
		if(sanity > 0)
			return selected;
		else
			throw new System.Exception("Could not find a good spawn point!");
	}

	void Update()
	{
		inCollisionList.RemoveWhere (character => character == null);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character> ())
			inCollisionList.Add (other.gameObject.GetComponent<Character> ());
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character> ())
			inCollisionList.Remove (other.gameObject.GetComponent<Character> ());
	}

	void LateUpdate()
	{
		hasSpawnedSomeoneThisFrame = false;
	}
}
