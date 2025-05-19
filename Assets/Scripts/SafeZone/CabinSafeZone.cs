
public class CabinSafeZone : SafeZone
{

	public CabinSafeZone()
	{
		safeZoneName = "버려진 오두막";
		safeZoneNum = 0;

		lores = new();
		commentaries = new();

		Lore lore = new();
		lore.loreName = "묘지기의 낡은 수기";
		lore.loreNum = 0;
		lore.loreText = "";
		lores.Add(lore);

		string commentary = "역겨운 냄새가 코를 찌른다.";
		commentaries.Add(commentary);

		commentary = "사람들이 사라진 집의 창가에는 차가운 기운만이 감돌고 있다.";
		commentaries.Add(commentary);
	}
}
