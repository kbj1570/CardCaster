using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class DescriptionWindow : MonoBehaviour
{

    public TMP_Text name;
    public TMP_Text description;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame

    public void SetUp(string name, string description)
    {
        this.name.text = name;
        this.description.text = description;
    }
}
