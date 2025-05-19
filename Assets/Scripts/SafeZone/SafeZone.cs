using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeZone
{
	protected string safeZoneName;
	protected int safeZoneNum;
	protected List<Lore> lores;
	protected List<string> commentaries;
	public List<Lore> GetLores()
	{ return lores;}
	public List<string> GetCommentaries()
	{ return commentaries; }
	
}
