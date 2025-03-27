using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    public TMP_Text message;
    public Image textbox;

    public float fadeDuration = 0.7f; // 사라지는 데 걸리는 시간
    private float fadeSpeed;

    void Start()
    {
        
        fadeSpeed = textbox.color.a / fadeDuration;
        
        StartCoroutine(FadeAway());
    }
    
    public void SetText(string value)
    {message.text = value;}

    IEnumerator FadeAway()
    {
        float alpha = textbox.color.a;
        Color color = textbox.color;
        yield return new WaitForSeconds(0.5f);
        
        while (alpha > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            message.alpha = alpha;
            color.a = alpha;
            textbox.color = color;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
