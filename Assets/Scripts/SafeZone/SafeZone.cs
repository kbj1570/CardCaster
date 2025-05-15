using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone
{
	protected string safeZoneName;
	protected int safeZoneNum;
	protected List<CutSceneNode> cutSceneNodes;
	protected List<Lore> lores;
	public List<CutSceneNode> GetCutSceneNodes()
	{ return cutSceneNodes; }
	public List<Lore> GetLores()
	{ return lores;}
}
