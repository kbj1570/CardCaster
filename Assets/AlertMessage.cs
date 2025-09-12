using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
	public TMP_Text message;
	public Image textbox;
	private bool isDestroyed = false;

	float fadeDuration = 0.6f; // 나타나거나 사라지는 데 걸리는 시간
	float stayDuration = 2.2f; // 다 나타난 후 유지 시간

	void Start()
	{
	}

	public void SetText(string value)
	{
		message.text = value;
	}

	public IEnumerator FadeInOut()
	{
		float alpha = 0f;
		float t = 0f;

		// Fade In
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			alpha = Mathf.Clamp01(t / fadeDuration);

			SetAlpha(alpha);
			yield return null;
		}

		// 대기
		yield return new WaitForSeconds(stayDuration);

		// Fade Out
		t = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			alpha = 1f - Mathf.Clamp01(t / fadeDuration);

			SetAlpha(alpha);
			yield return null;
		}

		Destroy(gameObject);
	}

	public IEnumerator FadeAway()
	{
		if (isDestroyed) yield break;
		isDestroyed = true;

		float alpha = 1f;
		float t = 0f;

		SetAlpha(alpha);
		// 잠깐 대기
		yield return new WaitForSeconds(stayDuration);

		// Fade Out
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			alpha = 1f - Mathf.Clamp01(t / fadeDuration);

			SetAlpha(alpha);
			yield return null;
		}

		Destroy(gameObject);
	}

	private void SetAlpha(float alpha)
	{
		// Text
		Color msgColor = message.color;
		msgColor.a = alpha;
		message.color = msgColor;

		// Box
		Color boxColor = textbox.color;
		boxColor.a = alpha;
		textbox.color = boxColor;
	}
}
