using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
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

    public SpriteRenderer spriteRenderer;

    public Color fadeColor;

    public int serventNum;

    private bool mouseOn;

    public Texture2D texture2D;




    private Material monsterMaterial;
    bool isDissolving = true;
    bool isDying = false;
    float fade = 0f;
    void Start()
    {
        // 몬스터의 Material 가져오기
        monsterMaterial = spriteRenderer.GetComponent<SpriteRenderer>().material;
        monsterMaterial.SetTexture("_MainTex", texture2D);
        monsterMaterial.SetFloat("_Fade", fade);
        monsterMaterial.SetColor("_Color", fadeColor);
    }

    void Update()
    {
        if(isDissolving)
        {
            fade += Time.deltaTime * 1.3f;

            if(fade >= 1f)
            {
                fade = 1f;
                // isDissolving = false;
                // BattleManagerAlt.Inst.ActionDone();
            }
            monsterMaterial.SetFloat("_Fade", fade);
        }

        if(isDying)
        {
            fade -= Time.deltaTime * 1.3f;

            if(fade <= 0f)
            {
                fade = 0f;
                isDying = false;
                Destroy(this.gameObject);
            }
            monsterMaterial.SetFloat("_Fade", fade);
        }
    }

    public void Dead()
    {
        isDying = true;
    }


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