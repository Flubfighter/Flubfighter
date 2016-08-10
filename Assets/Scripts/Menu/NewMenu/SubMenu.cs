using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool staticEventSystem = true;
	[SerializeField] private Selectable firstSelected;
	[SerializeField] private Selectable firstSelectedFallback;
	
	private GameObject previousSelection;

	public bool StaticEventSystem { get { return staticEventSystem; } }
	
	public Selectable GetFirstSelected()
	{
		return firstSelected ?? firstSelectedFallback;
	}
	
	public GameObject GetPreviousSelection()
	{
		return previousSelection;
	}
	
	private void OnExit()
	{
		if(staticEventSystem)
			previousSelection = EventSystem.current.currentSelectedGameObject;
	}
}