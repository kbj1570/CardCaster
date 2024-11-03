
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Encounter
{
    public int encounterNum;
    public string encounterName;
    public string encounterDescription;
    public List<string> select;
    public List<string> result;

    public int GetEncounterNum()
    {return encounterNum;}
    public void SetEncounterNum(int value)
    {this.encounterNum = value;}

    public string GetEncounterName()
    {return encounterName;}

    public string GetEncounterDescription()
    {return encounterDescription;}

    public List<string> GetSelect()
    {return select;}

    public List<string> GetResult()
    {return result;}
}