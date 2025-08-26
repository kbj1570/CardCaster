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
        Dictionary<string, int> cardHashMap = new();

        for(int i = 1; i < 200; i++)
        {cardHashMap.Add((i + 100).ToString(), i - 1);}

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
		cardDatas.Add(new BlueSlime()); //102
        cardDatas.Add(new RedSlime());//103
        cardDatas.Add(new GreenSlime());//104
		cardDatas.Add(new OddedStew());//105
		cardDatas.Add(new OddedStew());//106
		cardDatas.Add(new NoWayToReturn());//107
		cardDatas.Add(new WillOfWarrior());//108
        cardDatas.Add(new BrownSlime());//109
        cardDatas.Add(new WhiteSlime());//110
		cardDatas.Add(new BlackSlime());//111
		cardDatas.Add(new ShapeShifter());//112
		cardDatas.Add(new Frillizard());//113
        cardDatas.Add(new Griffin());//114
        cardDatas.Add(new Georgius());//115
        cardDatas.Add(new ForbiddenedSavior());//116
		cardDatas.Add(new KnightOfTheRedFlame());//117
		cardDatas.Add(new KnightOfTheAzure());//118
		cardDatas.Add(new ToddleyWoodley());//119
		cardDatas.Add(new FireBat());//120
		cardDatas.Add(new TheSacredBeast());//121
		cardDatas.Add(new CookOfDarkness());//122
		cardDatas.Add(new Boomsquirrel());//123
		cardDatas.Add(new StrayCat());//124
		cardDatas.Add(new Hypnotist());//125
		cardDatas.Add(new BurningSouls());//126
		cardDatas.Add(new NamelessTraveler());//127
		cardDatas.Add(new GiantLarva());//128
		cardDatas.Add(new SilentFog());//129
		cardDatas.Add(new ForbiddenedSavior());//130
		cardDatas.Add(new GloriousVictory());//131
		cardDatas.Add(new HolyPowerBoost());//132
		cardDatas.Add(new BrokenBless());//133
		cardDatas.Add(new AtTheEdgeOfPledge());//134
		cardDatas.Add(new RunTogether());//135
		return cardDatas;
    }

    public List<CardData> LoadRareCard()
    {
		List<CardData> cardDatas = new();

		cardDatas.Add(new Georgius());//115
		cardDatas.Add(new NamelessTraveler());//127
		cardDatas.Add(new ForbiddenedSavior());//130
		return cardDatas;
	}

    public List<CardData> LoadNormalCard()
	{
		List<CardData> cardDatas = new();
		cardDatas.Add(new BlueSlime()); //102
		cardDatas.Add(new RedSlime());//103
		cardDatas.Add(new GreenSlime());//104
		cardDatas.Add(new OddedStew());//105
		cardDatas.Add(new OddedStew());//106
		cardDatas.Add(new NoWayToReturn());//107
		cardDatas.Add(new WillOfWarrior());//108
		cardDatas.Add(new BrownSlime());//109
		cardDatas.Add(new WhiteSlime());//110
		cardDatas.Add(new BlackSlime());//111
		cardDatas.Add(new ShapeShifter());//112
		cardDatas.Add(new Frillizard());//113
		cardDatas.Add(new Griffin());//114
		cardDatas.Add(new ForbiddenedSavior());//116
		cardDatas.Add(new KnightOfTheRedFlame());//117
		cardDatas.Add(new KnightOfTheAzure());//118
		cardDatas.Add(new ToddleyWoodley());//119
		cardDatas.Add(new FireBat());//120
		cardDatas.Add(new TheSacredBeast());//121
		cardDatas.Add(new CookOfDarkness());//122
		cardDatas.Add(new Boomsquirrel());//123
		cardDatas.Add(new StrayCat());//124
		cardDatas.Add(new Hypnotist());//125
		cardDatas.Add(new BurningSouls());//126
		cardDatas.Add(new GiantLarva());//128
		cardDatas.Add(new SilentFog());//129
		cardDatas.Add(new GloriousVictory());//131
		cardDatas.Add(new HolyPowerBoost());//132
		cardDatas.Add(new BrokenBless());//133
		cardDatas.Add(new AtTheEdgeOfPledge());//134
		cardDatas.Add(new RunTogether());//135

		return cardDatas;
	}

	public List<ItemData> LoadItemDatabase()
    {
        List<ItemData> items = new();

        items.Add(new LetterFromKingdom());
        items.Add(new BigRedPotion());
        items.Add(new BrokenCompass());
        items.Add(new GoldenDice());
        items.Add(new GuideLantern());
        items.Add(new OminousCenser());
        items.Add(new RedPotion());
        items.Add(new RustyKnife());
        items.Add(new TrickGlove());

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