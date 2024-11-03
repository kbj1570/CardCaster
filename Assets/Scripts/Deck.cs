using System;
using System.Collections.Generic;

public class Deck
{
    public Dictionary<CardData, int> deckList;
    public string deckName;

    public Deck()
    {deckList = new();}

    public void AddCard(CardData value)
    {
        if(!deckList.ContainsKey(value))
        {deckList.Add(value, 1);}
        else
        {deckList[value]++;}
    }

    public void DeleteCard(CardData value)
    {
        deckList[value]--;

        if(deckList[value] == 0)
        {deckList.Remove(value);}
    }

    public Dictionary<CardData, int> GetDeckList()
    {return deckList;}

    public Dictionary<string, int> ConvertToDeckData(Dictionary<CardData, int> value)
    {
        Dictionary<string, int> data = new();

        foreach(KeyValuePair<CardData, int> pair in value)
        {data.Add(pair.Key.GetCardName(), pair.Value);}

        return data;
    }

}