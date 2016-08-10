using UnityEngine;
using System.Collections;

using JointType = UnityEngine.SpringJoint2D;

public class Log : MonoBehaviour 
{
	[SerializeField] private GameObject[] objectsToDestroyWhenVinesDestroyed;
	[Header("Vines")]
	[SerializeField] private Vine leftVine;
	[SerializeField] private Vine rightVine;
	[Header("Attachment Points")]
	[SerializeField] private Transform leftAttachmentPoint;
	[SerializeField] private Transform rightAttachmentPoint;

	private JointType leftJoint;
	private JointType rightJoint;

//	void Update()
//	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			Debug.Log ("Boom!");
//		
//			Plane p = new Plane(Vector3.forward, Vector3.zero);
//			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
//			float distance;
//			p.Raycast(r, out distance);
//			Vector3 point = r.origin + r.direction * distance;
//			Debug.DrawRay (point, Vector3.up, Color.red, 2f);
//			Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
//			rb2D.AddExplosionForce(400f * rb2D.mass, point, 10f);
//		}
//	}

	private void Start()
	{
		leftVine.Link(leftAttachmentPoint);
		rightVine.Link(rightAttachmentPoint);

		leftJoint = gameObject.AddComponent<JointType>();
		leftJoint.anchor = transform.InverseTransformPoint(leftAttachmentPoint.position);
		leftJoint.connectedAnchor = leftVine.transform.position;
		leftJoint.distance = Vector3.Distance(leftAttachmentPoint.position, leftVine.transform.position);
		leftJoint.enabled = false;
		leftJoint.enabled = true;
		leftJoint.dampingRatio = 0.5f;

		rightJoint = gameObject.AddComponent<JointType>();
		rightJoint.anchor = transform.InverseTransformPoint(rightAttachmentPoint.position);
		rightJoint.connectedAnchor = rightVine.transform.position;
		rightJoint.distance = Vector3.Distance(rightAttachmentPoint.position, rightVine.transform.position);
		rightJoint.enabled = false;
		rightJoint.enabled = true;
		leftJoint.dampingRatio = 0.5f;
	}

	public void UnlinkLeft()
	{
		leftVine.Unlink();
		Destroy (leftJoint);
		Destroy (leftAttachmentPoint.gameObject);
		if(!rightAttachmentPoint)
		{
			Destroy (this);
		}
	}

	public void UnlinkRight()
	{
		rightVine.Unlink();
		Destroy (rightJoint);
		Destroy (rightAttachmentPoint.gameObject);
		if(!leftAttachmentPoint)
		{
			Destroy (this);
		}
	}

	public void ToggleCollision(float duration)
	{
		StopAllCoroutines();
		StartCoroutine(ToggleCollisionCoroutine(duration));
	}

	private IEnumerator ToggleCollisionCoroutine(float duration)
	{
		foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(duration);
		foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = true;
		}
	}

	private void OnDestroy()
	{
		foreach(GameObject obj in objectsToDestroyWhenVinesDestroyed)
		{
			Destroy (obj);
		}
	}

	private void OnDrawGizmos()
	{
		if(leftAttachmentPoint)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(leftAttachmentPoint.position, 0.2f);
			if(leftVine)
			{
				Gizmos.DrawLine(leftAttachmentPoint.position, leftVine.transform.position);
			}
		}
		if(rightAttachmentPoint)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(rightAttachmentPoint.position, 0.2f);
			if(rightVine)
			{
				Gizmos.DrawLine(rightAttachmentPoint.position, rightVine.transform.position);
			}
		}
	}
}
