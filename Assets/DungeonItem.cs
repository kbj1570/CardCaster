using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonItem : MonoBehaviour, IPointerClickHandler
{
    public int itemNum;

    public void OnPointerClick(PointerEventData eventData)
    {DungeonManager.Inst.SetSelectedItem(itemNum);}
}
