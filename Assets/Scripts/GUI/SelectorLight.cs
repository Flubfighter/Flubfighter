using UnityEngine;
using System.Collections;

public class SelectorLight : MonoBehaviour
{
	public float beginDelay = 0f;

	public Vector3 movement = new Vector3(0.01f, 0, 0);
	private bool animate = false;

	private float xBoundLeft, xBoundRight;

	void Start()
	{

		Invoke ("Animate", beginDelay);
	}

	void Update()
	{
//		Debug.DrawLine(transform.root.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[1]) - Vector3.one * 0.1f, transform.root.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[1]) + Vector3.one * 0.1f, Color.red);
//		Debug.DrawLine(new Vector3(xBoundLeft, 200, 170), new Vector3(xBoundLeft, -200, 170));
		if(animate)
			Animate();
	}

	void Animate()
	{
		animate = true;
		xBoundRight = transform.root.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[0]).x;
		xBoundLeft = transform.root.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[1]).x;
//		xBoundRight = FindObjectOfType<Selector>().transform.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[0]).x;
//		xBoundLeft = FindObjectOfType<Selector>().transform.TransformPoint(transform.root.GetComponent<MeshFilter>().mesh.vertices[1]).x;
		transform.localPosition += movement;
		if(transform.position.x > xBoundRight + 0.5f)
		{
			transform.position = new Vector3 (xBoundLeft - 0.5f, transform.position.y, transform.position.z);
		}
	}
}
