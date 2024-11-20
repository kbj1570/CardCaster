using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Servent: MonoBehaviour, IPointerClickHandler
{
    private string serventName;
    private string serventNum;
    private string serventOriginForce;
    private string serventAbility;

    public GameObject border;

    public GameObject infoWindow;
    public Button activationButton;


    public void Attack()
    {}

    public void Defend()
    {}

    public void Summon()
    {}

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleManagerAlt.Inst.ShowServentInfo(this);
    }

    public void ShowInfo()
    {
        infoWindow.GetComponent<ServentInfoWindow>().OnOff(true);
        border.SetActive(true);
        activationButton.gameObject.SetActive(true);
    }
    public void CloseInfo()
    {
        infoWindow.GetComponent<ServentInfoWindow>().OnOff(false);
        border.SetActive(false);
        activationButton.gameObject.SetActive(false);
    }



}