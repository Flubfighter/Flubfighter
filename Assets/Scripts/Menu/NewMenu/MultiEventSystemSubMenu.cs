using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MultiEventSystemSubMenu : MonoBehaviour {

	[SerializeField] private EventSystem[] eventSystems;

	void OnEnter()
	{
		foreach(EventSystem es in eventSystems)
		{
			es.gameObject.SetActive(true);
		}
	}

	void OnExit()
	{
		foreach(EventSystem es in eventSystems)
		{
			es.gameObject.SetActive(false);
		}
	}
}
