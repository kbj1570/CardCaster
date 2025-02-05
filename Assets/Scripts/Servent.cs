using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class Servent: MonoBehaviour
{
    private ESequence sequence;
    private string serventName;
    private string serventOriginForce;
    private string serventAbility;

    public int serventForce;

    public GameObject border;
    public GameObject infoWindow;
    public TMP_Text serventForceText;
    public Button activationButton;

    public int serventNum;

    private bool mouseOn;


    public void Attack()
    {}

    public void Defend()
    {}

    public void Summon(CardData cardData)
    {}


    public void OnMouseUp()
    {
        if(mouseOn)
        {StartCoroutine(BattleManagerAlt.Inst.ShowServentInfo(this));}
    }

    public void OnMouseEnter()
    {mouseOn = true;}

    public void OnMouseExit()
    {mouseOn = false;}

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
    // public IEnumerator FadeOut()
    // {

    // }
    // void OnMouseDrag()
    // {
    //     Debug.Log("Dragging");
    // }
    public int GetServentNum()
    {return serventNum;}


}

public enum ESequence
{Idle, Targeting, Attacking, Blocking}