using UnityEngine;
using System.Collections;

public class FreezeBomb : Item 
{
	public float freezeDuration = 5.0f;
//	public GameObject freezeBombEffect;

	public override void OnBecameUsed (Character target)
	{
		target.Freeze(freezeDuration);
//		Instantiate (freezeBombEffect, target.transform.position, Quaternion.identity);
		target.GetThreatened(itemOwner.Player);
		Destroy (gameObject);
	}
}
