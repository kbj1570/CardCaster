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

    public int serventNum;

    private bool mouseOn;




    private Material monsterMaterial;
    private float dissolveAmount = -1.0f; // 처음엔 완전히 사라진 상태

    void Start()
    {
        // Vector3 originScale = spriteRenderer.transform.localScale;
        // spriteRenderer.transform.localScale = Vector3.zero; // 처음 크기 0
        // spriteRenderer.color = new Color(1, 1, 1, 0); // 처음엔 투명

        
        // Debug.Log(originScale);

        // Sequence summonSequence = DOTween.Sequence();

        // summonSequence.Append(spriteRenderer.transform.DOScale(originScale * 1.2f, 0.5f).SetEase(Ease.OutBack)) // 부드럽게 커짐
        //               .Join(spriteRenderer.DOFade(1, 0.5f)) // 투명 → 선명
        //               .Append(spriteRenderer.transform.DOScale(originScale, 0.2f)); // 살짝 줄어들며 안정

        // summonSequence.Play();


        
        // 몬스터의 Material 가져오기
        monsterMaterial = spriteRenderer.material;
        monsterMaterial.SetFloat("_DissolveAmount", dissolveAmount);

        // 1초 동안 아래에서 위로 나타나도록 애니메이션 적용
        DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 1.5f, 1.5f)
            .OnUpdate(() => monsterMaterial.SetFloat("_DissolveAmount", dissolveAmount))
            .SetEase(Ease.OutQuad);
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