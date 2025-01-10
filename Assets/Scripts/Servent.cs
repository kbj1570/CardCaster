using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Servent: MonoBehaviour
{
    private ESequence sequence;
    private string serventName;
    private string serventNum;
    private string serventOriginForce;
    private string serventAbility;

    public int serventForce;

    public GameObject border;
    public GameObject infoWindow;
    public TMP_Text serventForceText;
    public Button activationButton;


    public void Attack()
    {}

    public void Defend()
    {}

    public void Summon(CardData cardData)
    {}


    public void OnMouseUp()
    {
        StartCoroutine(BattleManagerAlt.Inst.ShowServentInfo(this));
    }

    public void OnMouseDown()
    {

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

public enum ESequence
{Idle, Targeting, Attacking, Blocking}