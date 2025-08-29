using UnityEngine;

public class Hole : MonoBehaviour
{
    private bool mouseOn;

    
    public void OnMouseUp()
    {}

    public void OnMouseEnter()
    {mouseOn = true;}

    public void OnMouseExit()
    {mouseOn = false;}

}