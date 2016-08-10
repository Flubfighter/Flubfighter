using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public float activationTimer;
    public Portal portalPair;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, portalPair.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		Character character = other.GetComponent<Character>();
		if(character)
		{
			if(!character.CanTeleport)
			{
				// TODO: Push the character away?
				return;
			}
		}
		Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();
		if(rigidbody)
		{
			Vector2 inVelocity = rigidbody.velocity;
			Vector2 outVelocity = portalPair.transform.right * Mathf.Max(5f, inVelocity.magnitude);
			rigidbody.velocity = outVelocity;
			other.transform.position = portalPair.transform.position;
			other.transform.SendMessage("Teleport", SendMessageOptions.DontRequireReceiver);
		}
    }
}
