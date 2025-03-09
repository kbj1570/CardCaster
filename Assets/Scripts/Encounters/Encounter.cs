using System;
using System.Collections.Generic;

public class Encounter
{
    protected string encounterName;//돌발상황 이름
    protected string encounterNum;//돌발상황 고유번호
    protected List<string> encounterText; //돌발상황 내용

    protected List<SelectionNode> firstWaypoint;
    protected List<SelectionNode> secondWaypoint;
    protected List<SelectionNode> thirdWaypoint;

    protected SelectionNode firstSelection;
    protected SelectionNode secondSelection;
    protected SelectionNode thirdSelection;
    protected SelectionNode fourthSelection;
    protected SelectionNode fifthSelection;
    
    public string GetName()
    {return encounterName;}
    public string GetNum()
    {return encounterNum;}
    public List<string> GetText()
    {return encounterText;}
    public SelectionNode GetFirstSelection()
    {return firstSelection;}
    public SelectionNode GetSecondSelection()
    {return secondSelection;}
    public SelectionNode GetThirdSelection()
    {return thirdSelection;}
}
public enum ERequireType
{None, EGold, EItem, EHealth, ECard}
