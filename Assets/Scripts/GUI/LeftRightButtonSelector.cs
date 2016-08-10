using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public class LeftRightButtonSelector : LeftRightSelector, IPointerClickHandler, ISubmitHandler
{
	[SerializeField]
	private UnityEvent onClick;
	
	private void Press()
	{
		if (!IsActive() || !IsInteractable())
			return;
		onClick.Invoke();
	}
	
	private IEnumerator OnFinishSubmit()
	{
		var time = colors.fadeDuration;
		yield return new WaitForSeconds (time);
		DoStateTransition (currentSelectionState, false);
	}
	
	#region ISubmitHandler implementation
	
	public void OnSubmit (BaseEventData eventData)
	{
		Press();
		if (!IsActive() || !IsInteractable())
			return;
		DoStateTransition(Selectable.SelectionState.Pressed, false);
		StartCoroutine(OnFinishSubmit());
	}
	
	#endregion
	
	#region IPointerClickHandler implementation
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		Press ();
	}
	
	#endregion
}

