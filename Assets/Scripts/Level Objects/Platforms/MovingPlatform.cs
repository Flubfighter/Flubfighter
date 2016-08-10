using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{
	
	void Update()
	{
		Debug.DrawRay(transform.position, rigidbody2D.velocity * 5, Color.red);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.GetComponent<MoveWithPlatform> ())
		{
			other.transform.SetParent (transform);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		MoveWithPlatform obj = other.gameObject.GetComponent<MoveWithPlatform>();
		if (obj && other.transform.parent == transform)
		{
			other.transform.SetParent(null);
			obj.SetNormal(Vector3.up);
		}
	}

	void OnCollisionStay2D(Collision2D other)
    {
		MoveWithPlatform obj = other.gameObject.GetComponent<MoveWithPlatform>();
        if (obj)
        {
			obj.SetNormal(transform.up);
			if (obj.rigidbody2D && transform.rigidbody2D)
			{
				obj.rigidbody2D.AddForce(rigidbody2D.velocity, ForceMode2D.Impulse);
			}
        }
    }


	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "MovingPlatformGizmo", true);
	}

}