using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Explodable : MonoBehaviour 
{
	public UnityEvent onExplode;

	public void Explode()
	{
		onExplode.Invoke();
	}
}
