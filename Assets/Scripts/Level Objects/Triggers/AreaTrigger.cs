using UnityEngine;
using System.Collections;

public class AreaTrigger : Trigger
{
	[Tooltip("If set to true, will trigger when the area is entered.")]
	public bool triggerOnEnter;
	[Tooltip("If set to true, will trigger when the area is exited.")]
	public bool triggerOnExit;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(triggerOnEnter)
			SendTriggerMessage ();
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(triggerOnExit)
			SendTriggerMessage ();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "AreaTriggerGizmo", true);
	}
}
