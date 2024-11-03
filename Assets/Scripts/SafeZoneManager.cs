using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeZoneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSideKick()
    {SceneManager.LoadScene("SideKick");}

    public void LoadCamp()
    {SceneManager.LoadScene("Camp");}

}
