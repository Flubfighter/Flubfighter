using UnityEngine;
using System.Collections;

public class PlayParticleAnimationEvent : MonoBehaviour 
{
	public ParticleSystem particleSystem;

	public void AnimationEventPlayParticle()
	{
		Debug.Log ("Played");
		if (particleSystem)
			particleSystem.Play ();
	}

	public void AnimationEventStopParticle()
	{
		Debug.Log ("Stopped");
		if (particleSystem)
			particleSystem.Stop ();
	}
}
