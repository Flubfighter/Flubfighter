using UnityEngine;
using System.Collections;

public class PreserveScale : MonoBehaviour 
{
	public Vector3 desiredScale = Vector3.one;

    private Item item;

    void Start()
    {
        
    }

	void LateUpdate()
	{
		Vector3 newScale = desiredScale;
		Transform parent = transform.parent;
		while(parent != null)
		{
			newScale.x /= parent.localScale.x;
			newScale.y /= parent.localScale.y;
			newScale.z /= parent.localScale.z;
			parent = parent.parent;
		}
		transform.localScale = newScale;
	}
}
