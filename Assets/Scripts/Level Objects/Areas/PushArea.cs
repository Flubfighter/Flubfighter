using UnityEngine;
using System.Collections;

public class PushArea : MonoBehaviour
{
	public Transform direction;
	[Tooltip("The speed the area imparts to pushable objects in m/s.")]
	public float speed;
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if(collider.rigidbody2D)
		{
			// F = mdv
			collider.rigidbody2D.AddForce(direction.up * (collider.rigidbody2D.mass * speed), ForceMode2D.Impulse);
		}
	}

	void OnDrawGizmos()
	{
		if(direction)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawRay(transform.position, direction.up);
			GizmosHelper.DrawArrowHead(direction.up + transform.position, direction.up, 45f, 0.25f);
		}
	}
}
