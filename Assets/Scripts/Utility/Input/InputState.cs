using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using System.IO;
using System.Xml.Serialization;

[System.Serializable]
public class InputState
{
	public int Index { get; private set; }
	public float VibrationLeft { get; private set; }
	public float VibrationRight { get; private set; }

	public InputState(int index)
	{
		Index = index;
	}

	public float GetAxis(string axisName)
	{
		return Input.GetAxis(axisName + Index);
	}

	public bool GetButton(string buttonName)
	{
		return Input.GetButton(buttonName + Index);
	}

	public bool GetButtonDown(string buttonName)
	{
		return Input.GetButtonDown (buttonName + Index);
	}

	public bool GetButtonUp(string buttonName)
	{
		return Input.GetButtonUp (buttonName + Index);
	}

	public void SetVibrationLeft(float left)
	{
		SetVibration(left, VibrationRight);
	}

	public void SetVibrationRight(float right)
	{
		SetVibration(VibrationLeft, right);
	}

	public void SetVibration(float left, float right)
	{
		VibrationLeft = Mathf.Clamp01(left);
		VibrationRight = Mathf.Clamp01(right);
		GamePad.SetVibration((PlayerIndex) (Index - 1), VibrationLeft, VibrationRight);
	}
}
