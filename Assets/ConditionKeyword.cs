
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;

public class ConditionKeyword : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject conditionDetailWindow;
    public void OnPointerEnter(PointerEventData eventData)
    {conditionDetailWindow.SetActive(true);}
    public void OnPointerExit(PointerEventData eventData)
    {conditionDetailWindow.SetActive(false);}


}
