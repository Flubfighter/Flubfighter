using UnityEngine;
using System.Collections;

public class ForceAssetPreload : MonoBehaviour 
{
	void Awake()
	{
		if(!Assets.Loaded)
			StartCoroutine (Assets.Load ());
	}
}
