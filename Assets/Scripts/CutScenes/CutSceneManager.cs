using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

public class CutSceneManager : MonoBehaviour
{
    CutScenes cutScenes;
    public GameObject textBoxObject;
    public TMP_Text textBox;
    public TMP_Text nameBox;
    public AudioSource soundManager;

    public int cutsceneNum;

    public List<Sprite> characters;

    public List<AudioClip> soundEffects;
    public List<AudioClip> backgroundMusic;

    public Image characterOnLeftSide;
    public Image characterOnRightSide;
    public Image fadeImage;
    private StringBuilder currentText = new StringBuilder();

    bool isActionDone;
    float typingSpeed = 0.04f;
    bool isTyping = false;

    void Awake()
    {
        switch(cutsceneNum)
        {
            case 0:
                cutScenes = new Intro();
                break;

            case 1:
                cutScenes = new SmallTalk();
                break;
        }
        StartCoroutine(StartCutScene());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isTyping)
            isActionDone = true;
        }
    }



    public IEnumerator StartCutScene()
    {
        foreach(CutSceneNode cutSceneNode in cutScenes.GetCutSceneNodes())
        {
            isActionDone = false;
            switch(cutSceneNode.cutSceneCommand)
            {
                case ECutSceneCommand.Wait: // ~초 기다리기
                yield return new WaitForSeconds(cutSceneNode.waitTime);
                break;

                case ECutSceneCommand.ShowText:
                isTyping = true;

                textBoxObject.SetActive(true);
                nameBox.text = cutSceneNode.name;
                textBox.text = cutSceneNode.text;

                for (int i = 0; i < cutSceneNode.text.Length; i++)
                {
                    if(cutSceneNode.text[i] == '.' ||
                    cutSceneNode.text[i] == '!' ||
                    cutSceneNode.text[i] == '?')
                    {typingSpeed = 0.17f;}
                    else
                    {typingSpeed = 0.05f;}
                    textBox.text = cutSceneNode.text.Substring(0, i + 1); // 한 글자씩 추가
                    yield return new WaitForSeconds(typingSpeed); // 일정 시간 대기


                    
                }

                isTyping = false;

                yield return new WaitUntil(() => isActionDone);
                break;

                case ECutSceneCommand.HideText:
                textBoxObject.SetActive(false);
                break;

                case ECutSceneCommand.ShowCharacterLeftSide: // 왼쪽에 캐릭터 띄우기
                characterOnLeftSide.sprite = characters[cutSceneNode.valueNum];
            
                if(!characterOnLeftSide.gameObject.activeSelf)
                {
                    characterOnLeftSide.gameObject.SetActive(true);
                    float time = 0;
                    Color color = characterOnLeftSide.color;

                    while (time < 0.7f)
                    {
                        time += Time.deltaTime;
                        color.a = Mathf.Lerp(0, 1, time / 0.7f); // 알파 값을 0 → 1로 변경
                        characterOnLeftSide.color = color;
                        yield return null;
                    }
                }
                break;

                case ECutSceneCommand.HideCharacterLeftSide:
                {
                    float time = 0.7f;
                    Color color = characterOnLeftSide.color;

                    while (time > 0)
                    {
                        time += Time.deltaTime;
                        color.a = Mathf.Lerp(1, 0, time / 0.7f);
                        characterOnLeftSide.color = color;
                        yield return null;
                    }
                    characterOnLeftSide.gameObject.SetActive(false);
                    break;
                }

                case ECutSceneCommand.HighLightCharacterLeftSide:
                characterOnRightSide.color = new Color(0.5f, 0.5f, 0.5f);
                characterOnLeftSide.color = new Color(1f, 1f, 1f);
                break;


                case ECutSceneCommand.ShowCharacterRightSide: // 오른쪽에 캐릭터 띄우기
                characterOnRightSide.sprite = characters[cutSceneNode.valueNum];

                if(!characterOnRightSide.gameObject.activeSelf)
                {
                    characterOnRightSide.gameObject.SetActive(true);
                    float time = 0;
                    Color color = characterOnRightSide.color;

                    while (time < 0.7f)
                    {
                        time += Time.deltaTime;
                        color.a = Mathf.Lerp(0, 1, time / 0.7f); // 알파 값을 0 → 1로 변경
                        characterOnRightSide.color = color;
                        yield return null;
                    }
                }
                // characterOnRightSide.gameObject.SetActive(true);
                
                break;

                case ECutSceneCommand.HideCharacterRightSide: // 오른쪽에 캐릭터 숨기기
                break;

                case ECutSceneCommand.FadeOutScreen: // 화면 어둡게
                {
                    float time = 0;
                    Color color = fadeImage.color;

                    while (time < cutSceneNode.waitTime)
                    {
                        time += Time.deltaTime;
                        color.a = Mathf.Lerp(0, 1, time / cutSceneNode.waitTime); // 알파 값을 0 → 1로 변경
                        fadeImage.color = color;
                        yield return null;
                    }
                    break;

                }
                

                case ECutSceneCommand.FadeInScreen: // 화면 밝게
                {
                    float time = 0;
                    Color color = fadeImage.color;
                    
                    while (time < cutSceneNode.waitTime)
                    {
                        time += Time.deltaTime;
                        color.a = Mathf.Lerp(1, 0, time / cutSceneNode.waitTime); // 알파 값을 1 → 0으로 변경
                        fadeImage.color = color;
                        yield return null;
                    }
                    break;

                }
                case ECutSceneCommand.HighLightCharacterRightSide: // 오른쪽에 띄운 캐릭터를 강조
                characterOnLeftSide.color = new Color(0.5f, 0.5f, 0.5f);
                characterOnRightSide.color = new Color(1f, 1f, 1f);
                break;
            }

        }

        yield return null;
    }

}