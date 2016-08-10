using UnityEngine;
using System.Collections;

public class InputTester : MonoBehaviour 
{
	InputState state;

	void Awake()
	{
		state = new InputState(1);
	}

	void Update () 
	{
		if(state.GetButtonDown("Jump"))
		{
			Debug.Log ("Jumped!");
		}

		if(state.GetButtonUp("Punch"))
		{
			Debug.Log ("Released Punch Charge!");
		}

		rigidbody.AddForce(new Vector3(state.GetAxis("Horizontal"), state.GetAxis("Vertical"), 0f), ForceMode.VelocityChange);
	}
}
