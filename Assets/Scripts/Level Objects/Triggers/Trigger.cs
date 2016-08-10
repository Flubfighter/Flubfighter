using UnityEngine;
using System.Collections;

public abstract class Trigger : MonoBehaviour
{
	[Tooltip("The delay between triggers.")]
	public float cooldown = 0;
	[Tooltip("The target of the trigger. If empty, will target itself")]
	public Transform target;
	[Tooltip("The method to call on the target")]
	public string message;
	private bool isCoolingDown = false;

	protected void SendTriggerMessage()
	{
		if(!isCoolingDown)
		{

			if(!target)
				BroadcastMessage(message, SendMessageOptions.RequireReceiver);
			else
				target.BroadcastMessage(message, SendMessageOptions.RequireReceiver);
			isCoolingDown = true;
			Invoke ("EndCooldown", cooldown);
		}
	}

	void EndCooldown()
	{
		isCoolingDown = false;
	}

	void OnDrawGizmos()
	{
		if(target)
		{
			Vector3 direction = target.transform.position - transform.position;
			Gizmos.color = Color.cyan;
			Gizmos.DrawRay(transform.position, direction);
			GizmosHelper.DrawArrowHead(target.transform.position, direction, 45f, 0.25f);
		}
	}
}