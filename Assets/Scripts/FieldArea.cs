using UnityEngine;

//필드에 마우스를 올렸다는 것을 인식하는 범위를 표현한 공간
public class FieldArea : MonoBehaviour
{
    public EMouseOnArea mouseOnArea;
    void OnMouseEnter()
    {BattleManagerAlt.Inst.SetMouseOnField(mouseOnArea);}

    void OnMouseExit()
    {BattleManagerAlt.Inst.ResetMouseOnField();}
}