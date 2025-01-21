using UnityEngine;
using UnityEngine.EventSystems;

//필드에 마우스를 올렸다는 것을 인식하는 범위를 표현한 공간
public class FieldArea : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public EMouseOnArea mouseOnArea;
    void OnMouseEnter()
    {BattleManagerAlt.Inst.SetMouseOnField(mouseOnArea);}

    void OnMouseExit()
    {BattleManagerAlt.Inst.ResetMouseOnField();}

    void OnMouseDrag()
    {
        Debug.Log("클릭? 드래그?");
        BattleManagerAlt.Inst.DrawAttackLine(this.transform.position, true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {Debug.Log("드래그 시작");}

    public void OnEndDrag(PointerEventData eventData)
    {}
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작");
    }
}