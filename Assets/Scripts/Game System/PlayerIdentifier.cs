using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerIdentifier : MonoBehaviour 
{
	[SerializeField] private Image baseImage;
	[SerializeField] private Image highlightImage;
	[SerializeField] private Text label;

	public void SetColor(Color baseColor, Color highlightColor)
	{
		baseImage.color = baseColor;
		highlightImage.color = highlightColor;
	}

	public void SetIndex(int playerIndex)
	{
		label.text = playerIndex.ToString();
	}

	public IEnumerator Show(float fadeOutDelay, float fadeOutDuration)
	{
		baseImage.color = SetAlpha(baseImage.color, 1f);
		highlightImage.color = SetAlpha(highlightImage.color, 1f);
		label.color = SetAlpha(label.color, 1f);

		gameObject.SetActive(true);
		yield return new WaitForSeconds(fadeOutDelay);
		float t = 0f;
		while(t <= fadeOutDuration)
		{
			float a = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
			baseImage.color = SetAlpha(baseImage.color, a);
			highlightImage.color = SetAlpha(highlightImage.color, a);
			label.color = SetAlpha(label.color, a);

			t += Time.deltaTime;
			yield return null;
		}
		gameObject.SetActive(false);
	}

	Color SetAlpha(Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}
}
