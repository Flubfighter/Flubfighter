using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour//, IPunchable
{
	/*[Tooltip("Does the object respawn?")]
	public bool respawns = true;
	[Tooltip("How long the object waits until respawning")]
	public float respawnDelay;
	[Tooltip("The extra room around the collider that needs to be clear of players for the object to respawn")]
	public float checkPadding = 0.1f;

	protected bool isBroken;

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

	public void GetPunched(Character puncher, Vector2 punchForce)
	{
		Break ();
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

	void Broken()
	{
		renderer.enabled = false;
		collider2D.enabled = false;
	}

	IEnumerator Repair()
	{
		yield return new WaitForSeconds (respawnDelay);
		while(!RepairAreaIsClear())
			yield return null;
		isBroken = false;
		Repaired ();
	}

	void Repaired()
	{
		renderer.enabled = true;
		collider2D.enabled = true;
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
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "BreakableWallGizmo", true);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(BottomLeft + (TopRight - BottomLeft) / 2f, TopRight - BottomLeft);
	}*/
}
