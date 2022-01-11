using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Image Image;

    public int Value
    {
        set
        {
            text.text = value.ToString();

        }
    }

    public Color Color
    {
        set
        {
            Image.color = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }


}
