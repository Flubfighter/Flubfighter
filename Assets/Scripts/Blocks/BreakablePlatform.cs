using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakablePlatform : MonoBehaviour
{
	public enum BreakMode
	{
		WaitThenBreak,
		AfterContinuousContact
	}
	[Tooltip("Does the object respawn?")]
	public bool respawns = true;
	[Tooltip("How long the object waits until respawning")]
	public float respawnDelay;
	[Tooltip("The extra room around the collider that needs to be clear of players for the object to respawn")]
	public float checkPadding = 0.1f;
	[Tooltip("The break mode. WaitThenBreak will break a set time after a platform is stepped on, while AfterContinuousContact requires a player be standing on the object for a set amount of time before it breaks.")]
	public BreakMode breakMode;
	[Tooltip("The amount of time a player needs to be standing on the platform to make it break OR the amount of time the platform waits until breaking after receiving contact.")]
	public float breakTime = 1f;
	private float health;

	private bool isBroken;

	void Awake()
	{
		health = breakTime;
	}

	private Vector2 BottomLeft
	{
		get
		{
			return collider2D.bounds.min - Vector3.one * checkPadding;
		}
	}
	
	private Vector2 TopRight
	{
		get
		{
			return collider2D.bounds.max + Vector3.one * checkPadding;
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		Character character = collision.gameObject.GetComponent<Character> ();
		if(character)
		{
			if(breakMode == BreakMode.AfterContinuousContact)
			{
				health -= breakTime * Time.deltaTime;
				if(health < 0f)
					Break ();
			}
			else
			{
				Invoke ("Break", breakTime);
			}
		}
	}

	void Break()
	{
		if(!isBroken)
		{
			isBroken = true;
			if(respawns)
				StartCoroutine("Repair");
			Broken ();
		}
	}

	IEnumerator Repair()
	{
		yield return new WaitForSeconds (respawnDelay);
		while(!RepairAreaIsClear())
			yield return null;
		isBroken = false;
		Repaired ();
	}

	bool RepairAreaIsClear()
	{
		Collider2D[] collidersInArea = Physics2D.OverlapAreaAll (BottomLeft, TopRight);
		foreach(Collider2D otherCollider in collidersInArea)
		{
			if(otherCollider.GetComponent<Character>())
				return false;
		}
		return true;
	}

	void Broken()
	{
		renderer.enabled = false;
		collider2D.enabled = false;
	}

	void Repaired ()
	{
		renderer.enabled = true;
		collider2D.enabled = true;
		health = breakTime;
	}
}
