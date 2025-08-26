using UnityEngine;
using UnityEngine.EventSystems;

//필드에 마우스를 올렸다는 것을 인식하는 범위를 표현한 공간
public class FieldArea : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Field field;
    public EMouseOnArea mouseOnArea;
    void OnMouseEnter()
    {
    }

    void OnMouseExit()
    {}

    void OnMouseDrag()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {}

    public void OnEndDrag(PointerEventData eventData)
    {
        if(field == null)
        return;
        if(!field.GetFilled())
        return;

        if(mouseOnArea != EMouseOnArea.Hole && mouseOnArea != EMouseOnArea.Enemy && mouseOnArea != EMouseOnArea.Player && BattleManager.Inst.CheckAttackable(mouseOnArea))
        StartCoroutine(BattleManager.Inst.EndAttackLine(mouseOnArea, BattleManager.Inst.CheckAttackable(mouseOnArea)));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(field == null)
        return;
        
        if(!field.GetFilled())
        return;
        
        if(mouseOnArea != EMouseOnArea.Hole && mouseOnArea != EMouseOnArea.Enemy && mouseOnArea != EMouseOnArea.Player)
        BattleManager.Inst.DrawAttackLine(this.transform.position, BattleManager.Inst.CheckAttackable(mouseOnArea));
    }

     public void OnPointerEnter(PointerEventData eventData)
    {
        BattleManager.Inst.SetMouseOnField(mouseOnArea);
    }
     public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.Inst.ResetMouseOnField();
    }
}