using UnityEngine;
using System.Collections;

public class GravityArea : MonoBehaviour
{
	[Tooltip("The gravityScale the area imparts to pushable objects. 0.0 means gravity is ignored, 2.0 means double gravity.")]
	public float gravityScale;
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if(collider.rigidbody2D)
		{
			collider.rigidbody2D.gravityScale = gravityScale;
		}
//		IPushable pushable = collider.gameObject.GetInterface<IPushable>();
//		if(pushable != null)
//		{
//			pushable.ChangeGravityScale(gravityScale);
//		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.rigidbody2D)
		{
			collider.rigidbody2D.gravityScale = 1f;
		}
//		IPushable pushable = collider.gameObject.GetInterface<IPushable>();
//		if(pushable != null)
//		{
//			pushable.ChangeGravityScale(1f);
//		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.DrawWireCube(Vector2.zero, Vector2.one);
//		Gizmos.color = Color.clear;
//		Gizmos.DrawCube(Vector2.zero, Vector2.one);
	}
}

