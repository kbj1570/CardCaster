using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    
    public void GoToDungeon()
    {
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
