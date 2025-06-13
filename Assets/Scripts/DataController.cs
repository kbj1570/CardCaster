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
        // if(File.Exists(Path.Combine(Application.dataPath, "CardDatabase.json")))
        // {
        //     List<CardData> data = JsonMapper.ToObject<List<CardData>>(File.ReadAllText(Path.Combine(Application.dataPath , "CardDatabase.json")));
        //     return data;
        // }

        List<CardData> cardDatas = new();

        cardDatas.Add(new CrescentLancer()); //0
        cardDatas.Add(new ElementalBoost());//1
        cardDatas.Add(new Duplicate()); //2
        cardDatas.Add(new GloriousLight());//3
        cardDatas.Add(new VioletLichLord());//4
        cardDatas.Add(new NoPainNoGain());//5
        cardDatas.Add(new OnlySilence());//6
        cardDatas.Add(new Stew());//7
        cardDatas.Add(new PriceOfBlood());//8
        cardDatas.Add(new CookOfDarkness());//9
        cardDatas.Add(new FlameLizard());//10
        cardDatas.Add(new BrokenDeal());//11
        cardDatas.Add(new HeartOnFire());//12
        cardDatas.Add(new FireCrimson());//13
        cardDatas.Add(new WaterHeize());//14
        cardDatas.Add(new MaskedWorld());//15
        cardDatas.Add(new WindCrest());//16
        cardDatas.Add(new WillOfBerserker());//17
        cardDatas.Add(new AbyssSeeker());//18
        cardDatas.Add(new DespairOfBerserker());//19
        
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