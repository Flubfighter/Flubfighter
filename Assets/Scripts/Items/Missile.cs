using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public float missileLife;
	[Header("Missle Settings")]
	public float missileSpeed;
	public float attackRadius;
	[Header("Explosion Settings")]
	public float explodeRadius;
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
	[HideInInspector]public MissileBeacon beacon;
	private Vector3 missileTarget;



	void Start()
	{
		missileTarget = beacon.transform.position;
        Destroy(gameObject, missileLife);
	}

	void Update () 
	{
        missileLife -= 1.0f * Time.deltaTime;
        if (missileTarget != null && beacon != null)
        {
            float distance = Vector3.Distance(transform.position, missileTarget);
            if (distance > attackRadius)
            {
                missileTarget = beacon.transform.position; // Update target
            }
            else if (distance < explodeRadius)
            {
                Destroy(beacon.gameObject); // TODO: When do you destroy the beacon? When the missile picks the final location or when the missile hits?
                Explode();
            }
            MoveTowardsTarget(missileTarget); // Actually move, regardless of target
        }
        else
            Destroy(gameObject);
	}

	private void Explode()
	{
		foreach(Collider2D hit in Physics2D.OverlapCircleAll (transform.position, explosionRadius))
		{
			Explodable explodable = hit.GetComponent<Explodable>();
            DestructablePlatform hitPlateform = hit.GetComponent<DestructablePlatform>();
            Character hitCharacter = hit.GetComponent<Character>();
			Rigidbody2D hitRigidbody = hit.GetComponent<Rigidbody2D>();
			if(hitCharacter)
			{
//				Vector2 direction = GetRandomExplodeDirection();
//				direction += (Vector2)(transform.position - hitCharacter.transform.position).normalized;
//				direction /= 2f;
//				hitCharacter.BecomeExploded(direction * explodeForce, explodeStunTime);
//				Debug.Log ("Hit " + hitCharacter + " for force " + (direction * explodeForce));
				Vector2 direction = hitCharacter.transform.position - transform.position;	// Get direction to player
				float distanceSquared = direction.sqrMagnitude;						// Get the distance and hold on to it
				direction.Normalize();												// Normalize
				direction += Vector2.up * explosionUppiness;						// Add some up
				direction.Normalize ();												// Normalize again
				direction *= 0.5f + (1f - distanceSquared / (explosionRadius * explosionRadius)) / 2f; // Multiply by inverse distance (closer = larger), scaled to range 0.5-1
				hitCharacter.BecomeExploded(direction * explodeForce, explodeStunTime);
				hitCharacter.GetThreatened(beacon.GetItemOwner().Player);
//				Debug.Log ("Hit " + hitCharacter + " for force " + (direction * explodeForce));
			}
			else if(hitRigidbody)
			{
//				Debug.Log("Exploding " + hitRigidbody.name);
//				hitRigidbody.AddExplosionForce(explodeForce * hitRigidbody.mass, transform.position, explosionRadius);
				hitRigidbody.AddExplosionForce(400f * hitRigidbody.mass, transform.position, 20f);
			}
            if (hitPlateform)
            {
                hitPlateform.health -= plateformDamage;
			}
//			if(explodable)
//			{
//				explodable.Explode();
//			}
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

	private void MoveTowardsTarget(Vector3 targetPosition)
	{
		transform.LookAt (targetPosition);
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, missileSpeed*Time.deltaTime);
	}
}
