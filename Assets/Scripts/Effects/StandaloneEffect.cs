using UnityEngine;
using System.Collections;

public class StandaloneEffect : MonoBehaviour 
{
	public float lifespan;
	public bool followsParent;
	private Transform trackedObject;
	private Vector3 localOffset;
	private Quaternion localRotation;
	
//	void Awake()
//	{
//		Destroy (gameObject, lifespan);
//	}
//	
//	public void Track(Transform target)
//	{
//		if(!followsParent)
//			return;
//		trackedObject = target;
//		localOffset = transform.TransformPoint (target.position);
//		localRotation = target.rotation * Quaternion.Inverse(transform.rotation);
//	}
//	
//	void LateUpdate()
//	{
//		if(trackedObject)
//		{
//			transform.position = trackedObject.position;// + transform.InverseTransformPoint (localOffset);
////			transform.rotation = trackedObject.rotation * localRotation;
//		}
//	}
}
