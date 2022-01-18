using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс для отображения Счёта игрока
/// </summary>
public class Points : MonoBehaviour
{
    // Количество очков
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
        TextPlayerPoints.text = "Счёт:";
    }
}
