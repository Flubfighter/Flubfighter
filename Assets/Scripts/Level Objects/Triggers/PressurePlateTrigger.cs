using UnityEngine;
using System.Collections;

public class PressurePlateTrigger : Trigger
{
	[Tooltip("If set to true, will trigger when the pressure plate is first stepped on.")]
	public bool triggerOnEnter = false;
	[Tooltip("if set to false, will trigger when the pressure plate is no longer stepped on.")]
	public bool triggerOnExit = false;

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(triggerOnEnter)
			SendTriggerMessage ();
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(triggerOnExit)
			SendTriggerMessage ();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "PressurePlateGizmo", true);
	}
}
