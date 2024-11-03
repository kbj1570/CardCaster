using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Hole : MonoBehaviour
{
    void OnMouseOver()
    {
        FieldManager.Inst.HoleMouseOver();
    }

    void OnMouseExit()
    {
        FieldManager.Inst.HoleMouseExit();
    }

    void OnMouseDown()
    {
        FieldManager.Inst.HoleMouseDown();
    }
    void OnMouseUp()
    {
        FieldManager.Inst.HoleMouseUp();
    }

}