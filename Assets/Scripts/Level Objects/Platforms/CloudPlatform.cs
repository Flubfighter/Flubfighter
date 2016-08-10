using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudPlatform : MonoBehaviour 
{
	public static HashSet<ICloudPlatformPassable> checkList = new HashSet<ICloudPlatformPassable> ();

	public Transform normal;
	private Plane collisionPlane;
	private Collider2D[] colliders;

	void Awake()
	{
		colliders = GetComponentsInChildren<Collider2D>();
	}

	void Update()
	{
		collisionPlane = new Plane(normal.up, normal.position);
		foreach(ICloudPlatformPassable obj in checkList)
		{
			float distance;
			collisionPlane.Raycast(new Ray(obj.GetCloudPlatformDetectionPoint(), Vector3.up), out distance);
			bool ignore = distance > 0f || obj.IgnoreCloudPlatform();
			foreach(Collider2D myCollider in colliders)
			{
				foreach(Collider2D theirCollider in obj.GetCloudPlatformColliders())
				{
					Physics2D.IgnoreCollision(myCollider, theirCollider, ignore);
				}
			}
		}
	}
}
