using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class  DataController : MonoBehaviour
{

    public static DataController Inst{get; private set;}
    void Awake() => Inst = this;

    public void SaveDeck(Dictionary<string, int> value)
    {
        JsonData info = JsonMapper.ToJson(value);

        File.WriteAllText(Path.Combine(Application.dataPath , "Deck.json"), info.ToString());
    }

   

    public void SaveCard(CardData value)
    {
        JsonData info = JsonMapper.ToJson(value);

        File.WriteAllText(Path.Combine(Application.dataPath , "Card.json"), info.ToString());
    }

    public CardData LoadCard()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Card.json")))
        {
            CardData data = JsonMapper.ToObject<CardData>(File.ReadAllText(Path.Combine(Application.dataPath , "Card.json")));
            return data;
        }
        return null;
    }

    public Dictionary<string, int> LoadDeck()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Deck.json")))
        {
            Dictionary<string, int> data = JsonMapper.ToObject<Dictionary<string, int>>(File.ReadAllText(Path.Combine(Application.dataPath , "Deck.json")));
            return data;
        }
        return null;
    }

    public void SaveCardList(Dictionary<string, int> value)
    {
        JsonData info = JsonMapper.ToJson(value);

        File.WriteAllText(Path.Combine(Application.dataPath , "CardList.json"), info.ToString());
        Debug.Log("Yes");
    }

     public List<CardData> LoadCardDatabase()
    {
        if(File.Exists(Path.Combine(Application.dataPath, "CardDatabase.json")))
        {
            List<CardData> data = JsonMapper.ToObject<List<CardData>>(File.ReadAllText(Path.Combine(Application.dataPath , "CardDatabase.json")));
            return data;
        }
        return null;
    }

    public Dictionary<string, int> LoadCardList()
    {
        if(File.Exists(Path.Combine(Application.dataPath, "CardList.json")))
        {
            Dictionary<string, int> data = JsonMapper.ToObject<Dictionary<string, int>>(File.ReadAllText(Path.Combine(Application.dataPath , "CardList.json")));
            return data;
        }
        return null;
    }

    public List<Encounter> LoadEncounterList()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Encounter.json")))
        {
            List<Encounter> data = JsonMapper.ToObject<List<Encounter>>(File.ReadAllText(Path.Combine(Application.dataPath , "Encounter.json")));
            return data;
        }
        return null;
    }
}