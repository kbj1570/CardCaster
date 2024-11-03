using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyButton : MonoBehaviour
{
    public TMP_Text buttonText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string value)
    {this.buttonText.text = value;}
}
