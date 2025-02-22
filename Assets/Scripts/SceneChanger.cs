using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Inst{get; private set;}
    void Awake() => Inst = this;

    public void LoadBattle()
    {SceneManager.LoadScene("Battle");}

    public void LoadSideKick()
    {SceneManager.LoadScene("SideKick");}

    public void LoadCamp()
    {SceneManager.LoadScene("Camp");}

    public void LoadDeckCustom()
    {SceneManager.LoadScene("DeckCustom");}
}
