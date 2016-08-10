using UnityEngine;
using System.Collections;

public class TeslaKioAnimation : MonoBehaviour {
	
	public AnimationCurve curve;
	
	public enum State
	{
		Testing,
		Gaming,
	}
	public State state;
	
	private float delay;
	private int index;
	private Animator animator;
	
	// Use this for initialization
	void Start () 
	{
		index = 1;
		animator = GetComponent<Animator>();
		if (state == State.Gaming)
			Invoke("StartTesla", delay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.V) && state == State.Testing)
		{
			Debug.Log(index);
			StartTesla();
		}
	}
	
	void StartTesla()
	{
		
		if (state == State.Gaming)
		{
			string teslaState = string.Format("Tesla {0}", Random.Range((int)1, (int)9));
			//animator.Play(discoState);
			animator.SetTrigger(teslaState);
			float percentageGameElapsed = (Game.gametype.duration - Game.TimeRemaining) / Game.gametype.duration;
			float percentageOfDelay = curve.Evaluate(percentageGameElapsed);
			delay = percentageOfDelay * Game.gametype.duration;
			Invoke("StartTesla", delay);
		}
		else if (state == State.Testing)
		{
			string teslaState = string.Format("Tesla {0}", index);
			Debug.Log(teslaState);
			//animator.Play(discoState);
			animator.SetTrigger(teslaState);
			index++;
			if (index >= 10)
				index = 1;
		}
	}
	
	void EndDisco()
	{
		animator.Play("Swim Underwater");
	}
	
}
