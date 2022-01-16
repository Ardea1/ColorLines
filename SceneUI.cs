using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUI : MonoBehaviour
{ 
    [SerializeField]
    private MainMenu mainMenu;

    [SerializeField]
    private Points points;

    public Points Points
    {
        get
        {
            return points;
        }
        set
        {

        }
    }

    public MainMenu MainMenu
    {
        get
        {
            return mainMenu;
        }
        set
        {

        }
    }
}
