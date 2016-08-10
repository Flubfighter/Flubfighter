using UnityEngine;
using System.Collections;

public class ThreatenedArea : MonoBehaviour 
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 pos = transform.position;
		pos.z = 0f;
		Gizmos.matrix = Matrix4x4.TRS(pos, transform.rotation, Vector3.Scale(transform.lossyScale, (Vector3)(collider2D as BoxCollider2D).size));
		Gizmos.DrawWireCube(Vector2.zero, Vector2.one);
		Gizmos.color = Color.Lerp(Color.red, Color.clear, 0.75f);
		Gizmos.DrawCube(Vector2.zero, Vector2.one);
	}
}
