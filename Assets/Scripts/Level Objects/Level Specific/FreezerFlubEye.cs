using UnityEngine;
using System.Collections;

public class FreezerFlubEye : MonoBehaviour 
{
	public float trackSpeed = 10f;
	public float minLookTime = 3f; // How long to wait before finding something new to look at
	public float maxLookTime = 6f;
	public Rect lookArea;
	Vector3 lookTarget = Vector3.zero;

	void Awake()
	{
		FindSomethingToLookAt ();
	}

	void FindSomethingToLookAt()
	{
		lookTarget.x = Random.Range (lookArea.x, lookArea.x + lookArea.width);
		lookTarget.y = Random.Range (lookArea.y, lookArea.y + lookArea.height);
		Invoke ("FindSomethingToLookAt", Random.Range (minLookTime, maxLookTime));
	}

	void Update()
	{
		Character[] characters = FindObjectsOfType<Character> ();
		if(characters.Length > 0)
		{
			Vector3 sum = characters[0].transform.position;
			for (int index = 1; index < characters.Length; index++) 
			{
				sum += characters[index].transform.position;
			}
			sum /= characters.Length;
			Quaternion targetRotation = Quaternion.LookRotation(sum - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * trackSpeed);
		}
		else
		{
			Debug.DrawLine(transform.position, lookTarget);
			Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * trackSpeed);
		}
	}
}
