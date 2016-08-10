using UnityEngine;
using System.Collections;

public class RandomItem : Item
{
	public Item[] items;

	public override void GotPunched (Character target)
	{
		if (CurrentState == State.WaitingToBePickedUp)
		{
			int randomInt = Random.Range (0, items.Length);
			Item itemToBeSpawned = Instantiate (items [randomInt], transform.position, items [randomInt].transform.rotation) as Item;
//			Debug.Log ("BOB");
			itemToBeSpawned.GetPickedUp (target);
			Destroy (gameObject);
		}
	}

}

