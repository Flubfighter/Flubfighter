using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class TestVine : MonoBehaviour 
{
	public Transform target;
	private LineRenderer lr;

	void Awake()
	{
		lr = GetComponent<LineRenderer> ();
		lr.SetVertexCount (2);
	}

	void Update()
	{
		if (!target)
			return;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, target.position);
	}

	void OnDrawGizmos()
	{
		if (!lr)
			Awake ();
		if (!target)
			return;
		Update ();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (transform.position, 0.25f);
		Gizmos.DrawLine (transform.position, target.position);
		Gizmos.DrawWireSphere (target.position, 0.25f);
	}
}
