using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	public Image fadeImage;

	public void StartGame()
	{
		StartCoroutine(StartGameCoroutine());
	}

	IEnumerator StartGameCoroutine()
	{
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Campsite");
	}                                                                                                                           

	private IEnumerator FadeOut()
	{
		fadeImage.gameObject.SetActive(true);
		float time = 0;
		Color color = fadeImage.color;

		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(0, 1, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
	}
}
