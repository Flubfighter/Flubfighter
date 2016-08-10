using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavigationButton : Button
{
	public override void OnMove(UnityEngine.EventSystems.AxisEventData eventData)
	{
		switch (eventData.moveDir)
		{
			case UnityEngine.EventSystems.MoveDirection.Down:
				if (navigation.selectOnDown)
					if (navigation.selectOnDown.interactable)
						base.OnMove(eventData);
				break;
			case UnityEngine.EventSystems.MoveDirection.Up:
				if(navigation.selectOnUp)
					if (navigation.selectOnUp.interactable)
						base.OnMove(eventData);
				break;
			case UnityEngine.EventSystems.MoveDirection.Left:
				if (navigation.selectOnLeft)
					if (navigation.selectOnLeft.interactable)
						base.OnMove(eventData);
				break;
			case UnityEngine.EventSystems.MoveDirection.Right:
				if (navigation.selectOnRight)
					if (navigation.selectOnRight.interactable)
						base.OnMove(eventData);
				break;
			default:
				base.OnMove(eventData);
				break;
		}
	}
}
