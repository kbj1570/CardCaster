using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DungeonData
{
    public static Dungeon dungeon;
    public static int currentPlayerLocation;
    public static int previousPlayerLocation;
    public static List<Node> map;
    public static List<int> nodeNumList;
    public static Dictionary<int, bool> activeNodes;
    public static Dictionary<int, bool> visitedNodes;
    public static Dictionary<DungeonEnemy, int> dungeonEnemies;
    public static int currentFloor;



    

    public static void Reset()
    {
        dungeon = null;
        map = null;
    }
}

public static class BattleData
{
    public static List<Enemy> enemies;
}

public static class DungeonClearData
{
    public static Enemy enemy;
}

public class CampsiteManager : MonoBehaviour
{
    public Image fadeImage;
    public AudioSource audioSource;
    public AudioClip inventoryOpen;
    public AudioClip inventoryClose;
    public AudioClip mapOpen;


    
    public void GoToDungeon()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    public void OpenCardPack()
    {
        ECardRarity eCardRarity;
        
        int randomNum = Random.Range(0, 10);

        if(randomNum < 6)
        {eCardRarity = ECardRarity.Common;}
        else if(randomNum < 9)
        {eCardRarity = ECardRarity.Uncommon;}
        else
        {eCardRarity = ECardRarity.Rare;}

    }

    public void PlayInventoryOpen()
    {audioSource.PlayOneShot(inventoryOpen);}

    public void PlayInventoryClose()
    {audioSource.PlayOneShot(inventoryClose);}

    public void PlayMapOpen()
    {audioSource.PlayOneShot(mapOpen);}


    private IEnumerator FadeOut()
    {
        float time = 0;
        Color color = fadeImage.color;

        while (time < 0.6f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / 0.6f); // 알파 값을 0 → 1로 변경
            fadeImage.color = color;
            yield return null;
        }

        DungeonData.dungeon = new Graveyard();
        SceneManager.LoadScene("Dungeon");
    }

    public void GoToShop()
    {SceneManager.LoadScene("Shop");}

    public void GoToCamp()
    {SceneManager.LoadScene("Camp");}

    public void GoToCollector()
    {SceneManager.LoadScene("OldCabin");}
}
