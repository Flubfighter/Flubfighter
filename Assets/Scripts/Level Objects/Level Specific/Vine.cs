using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Vine : MonoBehaviour
{
	private Transform target;
	private LineRenderer line;

	private void Awake()
	{
		line = GetComponent<LineRenderer>();
	}

	public void Link(Transform target)
	{
		this.target = target;
	}

	public void Unlink()
	{
		target = null;
		Destroy (gameObject, 3f);
	}

	private void Update()
	{
		if(target)
		{
			// TODO: Draw vine with LR
			line.SetPosition(1, transform.InverseTransformPoint(target.position));
		}
		else
		{
			// TODO: Decay vine
			transform.Translate(Vector3.up * Time.deltaTime * 8f);
		}
	}
}

