public class MysteriousFog : EnemyAbility
{
    public MysteriousFog()
    {
        abilityNum = "0";
        abilityName = "미지의 안개";
        abilityDescription = "상대 전원에게 1 대미지를 준다";

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.None;
        preRequisites.Add(preRequisite);
    }
}