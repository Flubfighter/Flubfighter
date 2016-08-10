using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PunchTimeDilation : MonoBehaviour
{
	class TimeDilationValue
	{
		public float timeRemaining;
		public float timeDilation;
		public TimeDilationValue(float timeDilation, float timeRemaining)
		{
			this.timeRemaining = timeRemaining;
			this.timeDilation = timeDilation;
		}
	}

	public float timeDilationSpeed = 1f;
	private List<TimeDilationValue> timeDilationValues = new List<TimeDilationValue>();

	public void AddTimeDilation(float targetTimeScale, float duration)
	{
//		Debug.Log ("Dilating time to " + targetTimeScale + " for a duraton of " + duration);
//		timeDilationValues.Add (new TimeDilationValue (targetTimeScale, duration));
	}

	public static void DilateTime(float targetTimeScale, float duration)
	{
//		PunchTimeDilation obj = FindObjectOfType<PunchTimeDilation> ();
//		if(obj)
//			obj.AddTimeDilation (targetTimeScale, duration);
	}

	void Update()
	{
//		if (Game.paused)
//		{
//			timeDilationValues.Clear();
//			return;
//		}
//		float timeDilation = 1f;
//		for (int index = timeDilationValues.Count - 1; index >= 0; index--)
//		{
//			timeDilation = Mathf.Min(timeDilationValues[index].timeDilation, timeDilation);
//			timeDilationValues[index].timeRemaining -= Time.unscaledDeltaTime;
//			if(timeDilationValues[index].timeRemaining <= 0f)
//				timeDilationValues.RemoveAt(index);
//		}
//		Time.timeScale = Mathf.Lerp (Time.timeScale, timeDilation, Time.unscaledDeltaTime * timeDilationSpeed);
	}
}
