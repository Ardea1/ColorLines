using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Text TextPlayerPoints;

    private int value;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            text.text = this.value.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Value = 0;
        TextPlayerPoints.text = "Очки игрока:";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
