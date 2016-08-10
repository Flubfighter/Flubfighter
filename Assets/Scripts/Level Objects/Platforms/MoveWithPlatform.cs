using UnityEngine;
using System.Collections;

public class MoveWithPlatform : MonoBehaviour
{

	protected Vector3 normal;
	protected Vector2 parentVelocity;

	protected virtual void Awake()
	{
		SetNormal(Vector3.up);
	}

	public void SetNormal(Vector3 normal)
	{
		this.normal = normal;
	}

}

