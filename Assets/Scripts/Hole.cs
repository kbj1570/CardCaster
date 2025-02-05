using UnityEngine;

public class Hole : MonoBehaviour
{
    private bool mouseOn;

    
    public void OnMouseUp()
    {
        if(mouseOn)
        {BattleManagerAlt.Inst.ShowTrashCards();}
    }

    public void OnMouseEnter()
    {mouseOn = true;}

    public void OnMouseExit()
    {mouseOn = false;}

}