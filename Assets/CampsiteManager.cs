using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampsiteManager : MonoBehaviour
{
    public void GoToDungeon()
    {SceneManager.LoadScene("Dungeon");}

    public void GoToShop()
    {SceneManager.LoadScene("Shop");}

    public void GoToCamp()
    {SceneManager.LoadScene("Camp");}

    public void GoToCollector()
    {SceneManager.LoadScene("OldCabin");}
}
