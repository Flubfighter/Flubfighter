using UnityEngine;
using System.Collections;

public class Interperlerp : MonoBehaviour
{
	public Transform target;
	public float percentPerFrame;

	void Update()
	{
		if(target)
			transform.position = Vector3.Lerp(transform.position, target.position, percentPerFrame);
	}

	public void UpdateTarget(Transform target)
	{
		this.target = target;
	}
}
