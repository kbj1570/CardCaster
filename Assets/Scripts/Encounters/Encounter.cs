using System.Collections.Generic;

public class Encounter
{
    protected string encounterName;//돌발상황 이름
    protected string encounterNum;//돌발상황 고유번호
    protected string encounterText; //돌발상황 내용
    protected List<string> encounterSelect;
    protected List<string> encounterResult;
    protected Dictionary<Item, int> encounterRequire;
    
    public string GetName()
    {return encounterName;}

    public string GetNum()
    {return encounterNum;}

    public string GetText()
    {return encounterText;}

    public List<string> GetSelect()
    {return encounterSelect;}

    public List<string> GetResult()
    {return encounterResult;}
}