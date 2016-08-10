using UnityEngine;
using System.Collections;

public interface ICloudPlatformPassable
{
	// Get the height the cloud platform should use
	Vector2 GetCloudPlatformDetectionPoint();

	bool IgnoreCloudPlatform();

	Collider2D[] GetCloudPlatformColliders();
}
