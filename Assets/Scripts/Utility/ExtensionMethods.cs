using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{
	#region Vector3
	public static Vector3 XY(this Vector3 vector)
	{
		return new Vector3(vector.x, vector.y, 0);
	}
	
	public static Vector3 XZ(this Vector3 vector)
	{
		return new Vector3(vector.x, 0, vector.z);
	}
	
	public static Vector3 YZ(this Vector3 vector)
	{
		return new Vector3(0, vector.y, vector.z);
	}
	
	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetX(this Vector3 vector, float newX)
	{
		return new Vector3 (newX, vector.y, vector.z);
	}
	
	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetY(this Vector3 vector, float newY)
	{
		return new Vector3 (vector.x, newY, vector.z);
	}
	
	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetZ(this Vector3 vector, float newZ)
	{
		return new Vector3 (vector.x, vector.y, newZ);
	}
	#endregion

	#region rigidbody2D
	public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
	{
		var dir = (body.transform.position - explosionPosition);
		float wearoff = Mathf.Clamp01(1 - (dir.magnitude / explosionRadius));
//		Debug.Log (explosionForce * wearoff);
		body.AddForceAtPosition(dir.normalized * explosionForce * wearoff, explosionPosition);
	}
	
	public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
	{
		var dir = (body.transform.position - explosionPosition);
		float wearoff = Mathf.Clamp01(1 - (dir.magnitude / explosionRadius));
		Vector3 baseForce = dir.normalized * explosionForce * wearoff;
		body.AddForce(baseForce);
		
		float upliftWearoff = 1 - upliftModifier / explosionRadius;
		Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
		body.AddForce(upliftForce);
	}
	#endregion

	#region Rect
	/// <summary>
	/// Resize the Rect so it contains the point
	/// </summary>
	public static Rect Encapsulate(this Rect rect, Vector2 point)
	{
		Rect copy = new Rect(rect);
		if (point.x < copy.x)
		{
			copy.width += (copy.x - point.x);
			copy.x = point.x;
		}
		if (point.y < copy.y)
		{
			copy.height += (copy.y - point.y);
			copy.y = point.y;
		}
		if (point.x > copy.x + copy.width)
		{
			copy.width += (point.x - (copy.x + copy.width));
		}
		if (point.y > copy.y + copy.height)
		{
			copy.height += (point.y - (copy.y + copy.height));
		}
		return copy;
	}
	#endregion

	#region List
	public static void Shuffle<T>(this List<T> list)
	{
		for (int index = list.Count - 1; index > 0; index--)
		{
			int swapPosition = UnityEngine.Random.Range(0, index + 1);
			T value = list[swapPosition];
			list[swapPosition] = list[index];
			list[index] = value;
		}
	}
	#endregion
	
	#region Color
	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetR(this Color color, float newValue)
	{
		color.r = newValue;
		return color;
	}
	
	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyR(this Color color, float amount)
	{
		color.r += amount;
		return color;
	}
	
	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetG(this Color color, float newValue)
	{
		color.g = newValue;
		return color;
	}
	
	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyG(this Color color, float amount)
	{
		color.g += amount;
		return color;
	}
	
	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetB(this Color color, float newValue)
	{
		color.b = newValue;
		return color;
	}
	
	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyB(this Color color, float amount)
	{
		color.b += amount;
		return color;
	}
	
	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetA(this Color color, float newValue)
	{
		color.a = newValue;
		return color;
	}
	
	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyA(this Color color, float amount)
	{
		color.a += amount;
		return color;
	}
	#endregion
	
	#region Enum Flags
	private static void CheckIsEnum<T>(bool withFlags)
	{
		if (!typeof(T).IsEnum)
			throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
		if (withFlags && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
			throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
	}
	
	public static bool IsFlagSet<T>(this T value, T flag) where T : struct
	{
		CheckIsEnum<T>(true);
		long lValue = Convert.ToInt64(value);
		long lFlag = Convert.ToInt64(flag);
		return (lValue & lFlag) != 0;
	}
	
	public static IEnumerable<T> GetFlags<T>(this T value) where T : struct
	{
		CheckIsEnum<T>(true);
		foreach (T flag in Enum.GetValues(typeof(T)).Cast<T>())
		{
			if (value.IsFlagSet(flag))
				yield return flag;
		}
	}
	
	public static T SetFlags<T>(this T value, T flags, bool on) where T : struct
	{
		CheckIsEnum<T>(true);
		long lValue = Convert.ToInt64(value);
		long lFlag = Convert.ToInt64(flags);
		if (on)
		{
			lValue |= lFlag;
		}
		else
		{
			lValue &= (~lFlag);
		}
		return (T)Enum.ToObject(typeof(T), lValue);
	}
	
	public static T SetFlags<T>(this T value, T flags) where T : struct
	{
		return value.SetFlags(flags, true);
	}
	
	public static T ClearFlags<T>(this T value, T flags) where T : struct
	{
		return value.SetFlags(flags, false);
	}
	
	public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct
	{
		CheckIsEnum<T>(true);
		long lValue = 0;
		foreach (T flag in flags)
		{
			long lFlag = Convert.ToInt64(flag);
			lValue |= lFlag;
		}
		return (T)Enum.ToObject(typeof(T), lValue);
	}
	#endregion
	
	#region GameObject
	/// <summary>
	/// Sets the layer of all Recursively
	/// </summary>
	/// <param name="inObj">In object.</param>
	/// <param name="layer">Layer.</param>
	public static void SetLayer(this GameObject inObj, int layer)
	{
		inObj.layer = layer;
		foreach(Transform child in inObj.transform)
		{
			child.gameObject.SetLayer(layer);
		}
	}
	
	/// <summary>
	/// Gets an interface on the object similarly to GetComponent<T>
	/// </summary>
	public static T GetInterface<T>(this GameObject inObj) where T : class
	{
		if (!typeof(T).IsInterface) {
			Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
			return null;
		}
		
		return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
	}
	
	/// <summary>
	/// Gets the interfaces on the object similarly to GetComponents<T>
	/// </summary>
	public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
	{
		if (!typeof(T).IsInterface) {
			Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
			return Enumerable.Empty<T>();
		}
		
		return inObj.GetComponents<Component>().OfType<T>();
	}

	public static T[] FindInterfacesOfType<T>(this UnityEngine.Object obj)
	{
		return UnityEngine.Object.FindObjectsOfType<Component>().OfType<T>().ToArray();
	}
	#endregion

	#region Particles
	public static void SetStartColorRecursively(this ParticleSystem system, Color color, bool preserveAlpha=true)
	{
		Color newColor = color;
		if (preserveAlpha)
			newColor.a = system.startColor.a;
		system.startColor = newColor;
		foreach(Transform child in system.transform)
		{
			if(child.particleSystem)
				SetStartColorRecursively(child.particleSystem, color, preserveAlpha);
		}
	}
	#endregion
}

#region Collections
public class Tuple<T1, T2>
{
	public T1 first;
	public T2 second;
}
#endregion
