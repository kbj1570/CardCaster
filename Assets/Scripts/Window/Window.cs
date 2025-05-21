using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Window : MonoBehaviour
{

	bool isOpened;
	void Start()
	{
		ScaleZero();
		
	}

	public void OnOff()
	{
		if(isOpened)
		{
			DG.Tweening.Sequence sequence = DOTween.Sequence()
			.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc));
			isOpened = false;
		}
		else
		{
			DG.Tweening.Sequence sequence = DOTween.Sequence()
			.Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.OutCirc);
			isOpened = true;
		}
	}

	protected void ScaleOne() => transform.localScale = Vector3.one;
	protected void ScaleZero() => transform.localScale = Vector3.zero;
}
