using UnityEngine;
using System.Collections;

public class TrouserSnakeAnimation : MonoBehaviour 
{
	[SerializeField] private Animator[] trouserSnakes;
	[SerializeField] private AnimationCurve curve;

    private int index = 0;
    private float delay;
	
	void OnEnable()
	{
		StartCoroutine (SnakeCoroutine ());
	}
	
	void OnDisable()
	{
		StopAllCoroutines();
	}

	private float GetDelay()
	{
		if(Game.gametype)
		{
			return curve.Evaluate((Game.gametype.duration - Game.TimeRemaining) / Game.gametype.duration) * Game.gametype.duration;
		}
		else
		{
			return curve.Evaluate(Time.timeSinceLevelLoad / 300f) * 300f;
		}
	}

	IEnumerator SnakeCoroutine()
	{
		while(true)
		{
			float delay = GetDelay();
			Debug.Log ("Sending Snake out in " + delay + " seconds");
			yield return new WaitForSeconds (delay);
			trouserSnakes [Random.Range (0, trouserSnakes.Length)].SetTrigger ("Attack");
		}
	}
}
