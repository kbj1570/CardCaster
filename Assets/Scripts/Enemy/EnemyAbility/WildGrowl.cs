using System.Collections.Generic;

public class WildGrowl : EnemyAbility
{
    public WildGrowl()
    {
        abilityName = "사나운 울음소리";
        abilityDescription = "상대 소환수 1마리의 포스를 1 감소시킨다.";

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.PlayerServentCountOver;
        preRequisite.count = 0;
        preRequisites.Add(preRequisite);
    }
}