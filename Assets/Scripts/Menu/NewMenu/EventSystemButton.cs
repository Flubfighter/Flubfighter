using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EventSystemButton : Button
{
	public EventSystem eventSystem;

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		
		// Selection tracking
		if (IsInteractable() && navigation.mode != Navigation.Mode.None)
			eventSystem.SetSelectedGameObject(gameObject, eventData);
		base.OnPointerDown(eventData);
	}
	
	public override void Select()
	{
		if (eventSystem.alreadySelecting)
			return;
		
		eventSystem.SetSelectedGameObject(gameObject);
	}

	protected override void Awake()
	{
		base.Awake();
		eventSystem = EventSystem.current;
		EventSystemProvider p = GetComponent<EventSystemProvider>();
		if(p)
		{
			if(p.eventSystem)
				eventSystem = p.eventSystem;
		}
	}
}
