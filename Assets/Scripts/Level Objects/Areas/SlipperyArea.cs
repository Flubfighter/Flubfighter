using UnityEngine;
using System.Collections;

public class SlipperyArea : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D collider)
	{
		Character character = collider.GetComponent<Character> ();
		if(character)
		{
			character.BecomeSlippery();
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		Character character = collider.GetComponent<Character> ();
		if(character)
		{
			character.BecomeNotSlippery();
		}
	}
}
