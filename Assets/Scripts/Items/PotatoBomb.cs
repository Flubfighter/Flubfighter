using UnityEngine;
using System.Collections;

public class PotatoBomb : Item
{
	// TODO: Remove me once playtested and Pat says you can remove it!
	public bool oldStyleExplosion=false;

	[Header("Explosion Settings")]
	public float explodeDelay = 5.0f;
	public float explodeForce = 30;
	public float explodeStunTime = 2.0f;
	public float explosionRadius = 5f;
    public float plateformDamage;
	public float explosionUppiness = 1f;

	[Header("Explosion Angle")]
	[Tooltip("This is the lowest angle that a flub will explode outwards at. Must be between 0 and 90.")]
	public float maximumAngleAboveHorizontal = 10.0f;
	[Header("Prefab")]
	public Transform explosionPrefab;

	public override void OnBecameUsed (Character target)
	{
		base.OnBecameUsed (target);
		AttachTo (target);
		Invoke ("Explode", explodeDelay);
	}

	public void AttachTo(Character character)
	{
		transform.parent = character.transform;
		transform.localPosition = Vector3.zero;
		character.HitByItem (this);
	}

	private void Explode()
	{
//		Debug.Log ("BOOM!");
		foreach(Collider2D hit in Physics2D.OverlapCircleAll (transform.position, explosionRadius))
		{
			Explodable explodable = hit.GetComponent<Explodable>();
            DestructablePlatform hitPlateform = hit.GetComponent<DestructablePlatform>();
			Character hitCharacter = hit.GetComponent<Character>();
			Rigidbody2D hitRigidbody = hit.GetComponent<Rigidbody2D>();
			if(hitCharacter)
			{
				Vector2 direction = Vector2.zero;
				
				if(oldStyleExplosion)
				{
					direction = GetRandomExplodeDirection();
					direction += (Vector2)(transform.position - hitCharacter.transform.position).normalized;
					direction /= 2f;
				}
				else
				{
					direction = hitCharacter.transform.position - transform.position;	// Get direction to player
					float distanceSquared = direction.sqrMagnitude;						// Get the distance and hold on to it
					direction.Normalize();												// Normalize
					direction += Vector2.up * explosionUppiness;						// Add some up
					direction.Normalize ();												// Normalize again
					direction *= 0.5f + (1f - distanceSquared / (explosionRadius * explosionRadius)) / 2f; // Multiply by inverse distance (closer = larger), scaled to range 0.5-1
					// lol math
				}
				
				hitCharacter.BecomeExploded(direction * explodeForce, explodeStunTime);
				hitCharacter.GetThreatened(itemOwner.Player);
//				Debug.Log ("Hit " + hitCharacter + " for force " + (direction * explodeForce));
			}
			else if(hitRigidbody)
			{
				hitRigidbody.AddExplosionForce(400f * hitRigidbody.mass, transform.position, 20f);
			}
            if (hitPlateform)
            {
                hitPlateform.health -= plateformDamage;
            }
			if(explodable)
			{
				explodable.Explode();
			}
		}
		if(explosionPrefab)
			Instantiate (explosionPrefab, transform.position, explosionPrefab.rotation);
		Destroy (gameObject);
	}

	private Vector2 GetRandomExplodeDirection()
	{
		float randomAngle = Random.Range (maximumAngleAboveHorizontal, 180 - maximumAngleAboveHorizontal);
		Vector2 randomVector = new Vector2 ((float)Mathf.Cos (Mathf.Deg2Rad * randomAngle), 
		                                    (float)Mathf.Sin (Mathf.Deg2Rad * randomAngle));
		//print (randomVector);
		return randomVector.normalized;
	}
}