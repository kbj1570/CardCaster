
public class CabinSafeZone : SafeZone
{

	public CabinSafeZone()
	{
		safeZoneName = "버려진 오두막";
		safeZoneNum = 0;

		lores = new();
		Lore lore = new();
		lore.loreName = "묘지기의 수기";
		lore.loreNum = 0;
		lores.Add(lore);

	}
}
