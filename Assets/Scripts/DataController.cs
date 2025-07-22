using UnityEngine;
using System.Collections.Generic;
using LitJson;
using Newtonsoft.Json;
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

    public SaveData LoadData()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "SaveData.json")))
        {
            SaveData data = JsonMapper.ToObject<SaveData>(File.ReadAllText(Path.Combine(Application.dataPath , "SaveData.json")));
            return data;
        }
        return null;
    }

    public void SaveData(SaveData value)
    {
        JsonData info = JsonMapper.ToJson(value);

        File.WriteAllText(Path.Combine(Application.dataPath , "SaveData.json"), info.ToString());
    }

    public Dictionary<string, int> LoadCardHashMap()
    {
        Dictionary<string, int> cardHashMap = new() {
            { "101", 0 },
			{ "102", 1 },
            { "103", 2 },
			{ "104", 3 },
            { "105", 4 },
			{ "106", 5 },
			{ "107", 6 },
			{ "108", 7 },
			{ "109", 8 },
			{ "110", 9 },
			{ "111", 10 },
			{ "112", 11 },
			{ "113", 12 },
			{ "114", 13 },
			{ "115", 14 },
			{ "116", 15 }
		};
        return cardHashMap;
	}

    public List<CutSceneNode> LoadCutScene(string cutSceneNum)
    {
		TextAsset jsonFile = Resources.Load<TextAsset>("CutScenes/cutscene_" + cutSceneNum +".json");
		if (jsonFile == null)
		{
			Debug.LogError("dialogues.json 파일을 찾을 수 없습니다.");
			return null;
		}

		return JsonConvert.DeserializeObject<List<CutSceneNode>>(jsonFile.text);
	}

    public Dictionary<string, Dialogue> LoadDialogues(string fileName)
    {
		TextAsset jsonFile = Resources.Load<TextAsset>("Dialogues/" + fileName);
		if (jsonFile == null)
		{
			Debug.LogError("dialogues.json 파일을 찾을 수 없습니다.");
			return null;
		}

		return JsonConvert.DeserializeObject<Dictionary<string, Dialogue>>(jsonFile.text);
	}



	// public void SaveCard(CardData value)
	// {
	//     JsonData info = JsonMapper.ToJson(value);

	//     File.WriteAllText(Path.Combine(Application.dataPath , "Card.json"), info.ToString());
	// }

	// public CardData LoadCard()
	// {
	//     if(File.Exists(Path.Combine(Application.dataPath , "Card.json")))
	//     {
	//         CardData data = JsonMapper.ToObject<CardData>(File.ReadAllText(Path.Combine(Application.dataPath , "Card.json")));
	//         return data;
	//     }
	//     return null;
	// }


	public Dictionary<string, int> LoadDeck()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Deck.json")))
        {
            Dictionary<string, int> data = JsonMapper.ToObject<Dictionary<string, int>>(File.ReadAllText(Path.Combine(Application.dataPath , "Deck.json")));
            return data;
        }
        return null;
    }

    public List<string> LoadItemList()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Item.json")))
        {
            List<string> data = JsonMapper.ToObject<List<string>>(File.ReadAllText(Path.Combine(Application.dataPath , "Item.json")));
            return data;
        }
        return null;
    }


    public void SaveCardList(Dictionary<string, int> value)
    {
        JsonData info = JsonMapper.ToJson(value);

        File.WriteAllText(Path.Combine(Application.dataPath , "CardList.json"), info.ToString());
    }

     public List<CardData> LoadCardDatabase()
    {

		List<CardData> cardDatas = new();

        cardDatas.Add(new CrescentLancer()); //101
        cardDatas.Add(new InvisibleCape());//102
        cardDatas.Add(new BlueSlime()); //103
        cardDatas.Add(new RedSlime());//104
        cardDatas.Add(new GreenSlime());//105
        cardDatas.Add(new RandomTeleporter());//106
        cardDatas.Add(new OddedStew());//107
        cardDatas.Add(new PriceOfBlood());//108
        cardDatas.Add(new NoWayToReturn());//109
        cardDatas.Add(new WillOfBerserker());//110
        cardDatas.Add(new MonsterMask());//111

        cardDatas.Add(new PurpleCenser());//112
        cardDatas.Add(new BrownSlime());//113
        cardDatas.Add(new WhiteSlime());//114
        cardDatas.Add(new BlackSlime());//115
        cardDatas.Add(new GraySlime());//116
        
        return cardDatas;
    }

    public List<Item> LoadItemDatabase()
    {
        List<Item> items = new();

        items.Add(new LetterFromKingdom());
        items.Add(new BigRedPotion());
        items.Add(new BrokenCompass());
        items.Add(new GoldenDice());
        items.Add(new GuideLantern());
        items.Add(new OminousCenser());
        items.Add(new RedPotion());
        items.Add(new RustyKnife());
        items.Add(new TrickGlove());
        items.Add(new DirtyPatch());
        items.Add(new OldStick());
        items.Add(new ShardOfStarlight());
        items.Add(new SharpFang());

        return items;
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

    public List<RandomEvent> LoadEncounterList()
    {
        if(File.Exists(Path.Combine(Application.dataPath , "Encounter.json")))
        {
            List<RandomEvent> data = JsonMapper.ToObject<List<RandomEvent>>(File.ReadAllText(Path.Combine(Application.dataPath , "Encounter.json")));
            return data;
        }
        return null;
    }
}