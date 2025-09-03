
public class BattleDataManager
{
	private int playerHealth;
	private int enemyHealth;
	private int playerGold;
	private int totalSummonedServentCount; // 총 소환된 소환수의 수
}

public enum EEnemyAction { None, Summon, Attack, Ability }
public enum EServentType { None, Player, Enemy }
public enum ECardType { None, Servent, Spell, Field, Enemy }
public enum ESpellType { None, Normal, Field }
public enum ECardRarity { None, Normal, Rare }
public enum EBattleObjectType { None, Card, Servent, Field, Enemy, Player }
public enum EServentAttribute { None, Fire, Water, Earth, Wind, Dark, Light }
public enum EMouseOnArea { None, Player, Enemy, Field_1, Field_2, Field_3, Field_4, Field_5, Field_6, AnyWhere, Hole, Inventory, Storage, Trash }
public enum ECardTargetType { NoneTargeting, Targeting }
public enum EServentCondition { None, Void, Oblivion, Poison, Madness, Testament }
public enum EServentSize { Small, Middle, Big }
public enum EServentState { None, Idle, Guard, Ready, Summon, Attack, Death }
public enum EParryState { Idle, Parry, Succecced, Failed }