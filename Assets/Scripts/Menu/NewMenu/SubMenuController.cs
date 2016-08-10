using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SubMenuController : MonoBehaviour
{
	[SerializeField] private SubMenu firstMenu;
	
	private List<SubMenu> breadcrumbs = new List<SubMenu>();
	
	private void Start()
	{
//		foreach(SubMenu menu in FindObjectsOfType<SubMenu>())
//			menu.gameObject.SetActive(false);
		Enter (firstMenu);
	}
	
	/// <summary>
	/// Enter the specified menu.
	/// </summary>
	/// <param name="menu">Menu to enter.</param>
	public void Enter(SubMenu menu)
	{
		// Exit the previous in the breadcrumb if there is one
		if(breadcrumbs.Count > 0)
			ExitMenu (breadcrumbs.Last()); // Leave the old breadcrumb in
		
		// Add the new menu to the breadcrumb
		breadcrumbs.Add(menu);
		
		// Enter the new breadcrumb
		EnterMenu(menu);
	}
	
	/// <summary>
	/// Go back to the previous menu.
	/// </summary>
	public void Back()
	{
		// Can't go back if there is nothing to go back to!
		if(breadcrumbs.Count <= 1)
			return;
		
		// Exit and remove the current breadcrumb
		ExitMenu(breadcrumbs.Last());
		breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
		
		// Enter the new current breadcrumb
		SubMenu previous = breadcrumbs.Last ();
		EnterMenu(previous);
		EventSystem.current.SetSelectedGameObject(previous.GetPreviousSelection());
	}
	
	private void ExitMenu(SubMenu menu)
	{
		menu.SendMessage("OnExit", SendMessageOptions.DontRequireReceiver);
	}
	
	private void EnterMenu(SubMenu menu)
	{
		if(!menu.gameObject.activeSelf)
			menu.gameObject.SetActive(true);
		menu.SendMessage("OnEnter", SendMessageOptions.DontRequireReceiver);
		if(menu.StaticEventSystem)
			EventSystem.current.SetSelectedGameObject(menu.GetFirstSelected().gameObject);
	}
}