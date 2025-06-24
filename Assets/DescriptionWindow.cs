using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class DescriptionWindow : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text description;


    void Start()
    {
        this.gameObject.SetActive(false);
    }


    public void SetUp(string name, string description)
    {
        this.name.text = name;
        this.description.text = description;
    }
}
