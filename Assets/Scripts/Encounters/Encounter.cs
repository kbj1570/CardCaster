using System.Collections.Generic;

public class Encounter
{
    protected string encounterName;//돌발상황 이름
    protected string encounterNum;//돌발상황 고유번호
    protected List<string> encounterText; //돌발상황 내용
    protected List<string> encounterSelect;

    protected string firstSelection;
    protected string secondSelection;
    protected string thirdSelection;

    protected List<string> firstResult;
    protected List<string> secondResult;
    protected List<string> thirdResult;
    protected Dictionary<Item, int> encounterRequire;
    
    public string GetName()
    {return encounterName;}

    public string GetNum()
    {return encounterNum;}

    public List<string> GetText()
    {return encounterText;}

    public List<string> GetSelect()
    {return encounterSelect;}

    public List<string> GetFirstResult()
    {return firstResult;}
    public List<string> GetSecondResult()
    {return secondResult;}
    public List<string> GetThirdResult()
    {return thirdResult;}
}