using UnityEngine;
using System.Collections;

public class ItemSpawnPoint : MonoBehaviour
{
	public Transform spawnPoint;
	public float itemCleanupDuration = 10f;
	private Item spawnedItem = null;
	
	public Item Spawn(Item itemPrefab)
	{
		if (!IsOpen ())
			Debug.LogWarning ("Item Spawn Point (" + name + ") is not open! Spawning an item anyways.");
		spawnedItem = Instantiate (itemPrefab, spawnPoint.position, itemPrefab.transform.rotation) as Item;
		StartCoroutine (CleanUpSpawnedItem (spawnedItem));
		return spawnedItem;
	}

	IEnumerator CleanUpSpawnedItem(Item item)
	{
		yield return new WaitForSeconds (itemCleanupDuration);
		if(item)
		{
			if(item.CurrentState == Item.State.WaitingToBePickedUp)
			{
				Destroy (item.gameObject);
			}
		}
	}

	public bool IsOpen()
	{
		if (!spawnedItem)
			return true;
		return spawnedItem.CurrentState != Item.State.WaitingToBePickedUp;
	}

	public static ItemSpawnPoint FindOpenSpawnPoint()
	{
		ItemSpawnPoint[] allPoints = FindObjectsOfType<ItemSpawnPoint> ();
		ItemSpawnPoint selected = null;
		int sanity = 100;
		do
		{
			selected = allPoints[Random.Range(0, allPoints.Length)];
		}
		while(!selected.IsOpen() && --sanity > 0);
		if(sanity > 0)
			return selected;
		else
			throw new System.Exception("Could not find a good item spawn point!");
	}
}

