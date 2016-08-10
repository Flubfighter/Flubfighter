using UnityEngine;
using System.Collections;

public class StarShipDiscoAnimation : MonoBehaviour {

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
            Invoke("StartDisco", delay);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.V) && state == State.Testing)
        {
            Debug.Log(index);
            StartDisco();
        }
	}

    void StartDisco()
    {

        if (state == State.Gaming)
        {
            string discoState = string.Format("Disco {0}", Random.Range((int)1, (int)3));
            //animator.Play(discoState);
            animator.SetTrigger(discoState);
            float percentageGameElapsed = (Game.gametype.duration - Game.TimeRemaining) / Game.gametype.duration;
            float percentageOfDelay = curve.Evaluate(percentageGameElapsed);
            delay = percentageOfDelay * Game.gametype.duration;
            Invoke("StartDisco", delay);
        }
        else if (state == State.Testing)
        {
            string discoState = string.Format("Disco {0}", index);
            Debug.Log(discoState);
            //animator.Play(discoState);
            animator.SetTrigger(discoState);
            index++;
            if (index >= 4)
                index = 1;
        }
    }

    void EndDisco()
    {
        animator.Play("Idle");
    }

}
