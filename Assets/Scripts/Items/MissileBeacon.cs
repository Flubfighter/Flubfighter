using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileBeacon : Item 
{	
	public Missile missilePrefab;
	private bool hasSpawnedMissile;
    private GameObject missleSpawnPosition;
    private GameObject[] missleSpawnPositionList;
	private Missile spawnedMissile;
	
	private void Start()
	{
        //missleSpawnPosition = GameObject.FindGameObjectWithTag ("MissleSpawnPoint");
        missleSpawnPositionList = GameObject.FindGameObjectsWithTag("MissleSpawnPoint");
		hasSpawnedMissile = false;
	}

	public override void OnBecameThrown (CharacterItems.ThrowData throwData)
	{
		base.OnBecameThrown (throwData);
	}

	public override void OnBecameUsed (Character target)
	{
		base.OnBecameUsed (target);
		AttachTo (target);
		if (!hasSpawnedMissile)
			SpawnMissile ();
	}

	public override void OnBecamePlaced (Vector3 placePosition)
	{
		base.OnBecamePlaced (placePosition);
		if (!hasSpawnedMissile)
			SpawnMissile ();
	}
	
	public void AttachTo(Character character)
	{
		transform.parent = character.transform;
		transform.localPosition = Vector3.zero;
		character.Beacon = this;
		character.GotTaggedByMissileBeacon ();
	}

	private void SpawnMissile()
	{
        missleSpawnPosition = missleSpawnPositionList[Random.Range(0,missleSpawnPositionList.Length)];
		spawnedMissile = Instantiate (missilePrefab, missleSpawnPosition.transform.position, Quaternion.identity) as Missile;
		spawnedMissile.beacon = this;
		hasSpawnedMissile = true;
		spawnedMissile.beacon.itemOwner = itemOwner;
	}
	
}
