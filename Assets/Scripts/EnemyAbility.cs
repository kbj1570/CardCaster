using System.Collections.Generic;

public class EnemyAbility
{
    protected string abilityNum;
    protected string abilityName;
    protected string abilityDescription;
    protected List<PreRequisite> preRequisites;
    public string GetNum()
    {return abilityNum;}
    
    public string GetName()
    {return abilityName;}

    public string GetDescription()
    {return abilityDescription;}

    public List<PreRequisite> GetPreRequisites()
    {return preRequisites;}
}