using UnityEngine;
using System.Collections;

public class TimerTrigger : Trigger
{
	[Tooltip("The initial delay before the timer starts triggering.")]
	public float initialDelay = 0.5f;
	[Tooltip("The delay between triggers.")]
	public float repeatingDelay = 0.5f;
	[Tooltip("How many times to trigger before stopping. Set to 0 to repeat forever.")]
	public int repeats = 0; // 0 = no repeats
	private int repeatsLeft;

	void Awake()
	{
		repeatsLeft = repeats;
		InvokeRepeating ("SendTriggerMessage", initialDelay, repeatingDelay);
	}

	void Trigger()
	{
		if(repeats > 0 && --repeatsLeft <= 0)
			CancelInvoke("SendTriggerMessage");
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "TimerGizmo", true);
	}
}
