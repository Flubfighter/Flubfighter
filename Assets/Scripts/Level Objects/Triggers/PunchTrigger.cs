using UnityEngine;
using System.Collections;

public class PunchTrigger : Trigger
{
	[Tooltip("The minimum amount of force required for the button to be pressed.")]
	public float minimumRequiredPunchForce = 0.0f;

	public void GotPunched(Character puncher)
	{
		if(puncher.GetPunchForce().magnitude > minimumRequiredPunchForce)
			SendTriggerMessage ();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "PunchTriggerGizmo", true);
	}
}
