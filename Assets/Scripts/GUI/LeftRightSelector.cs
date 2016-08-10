using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public class LeftRightSelector : Selectable
{
	[SerializeField] 
	private UnityEvent onLeft;
	[SerializeField]
	private UnityEvent onRight;

	private bool _leftRightInteractable = true;
	public bool LeftRightInteractable
	{
		get
		{
			return _leftRightInteractable;
		}
		set
		{
			if(value != _leftRightInteractable)
			{
				foreach(Selectable s in GetComponentsInChildren<Selectable>())
				{
					if(s != this)
						s.interactable = value;
				}
				_leftRightInteractable = value;
			}
		}
	}

	[HideInInspector] public EventSystem eventSystem;

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

	public override void OnMove(AxisEventData eventData)
	{
		switch (eventData.moveDir)
		{
			case MoveDirection.Left:
				if(_leftRightInteractable)
					onLeft.Invoke();
				break;
			case MoveDirection.Right:
				if(_leftRightInteractable)
					onRight.Invoke();
				break;
			default:
				base.OnMove(eventData);
				break;
		}
	}

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
}