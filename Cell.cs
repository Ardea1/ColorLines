using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Color highlightedColor;

    private Renderer renderer;

    private Color originalColor;

    /// <summary>
    /// Метод, чтобы выбранная нами клетка, куда следует переместить шарик,
    /// меняла цвет
    /// </summary>
    public void Highlight(bool highlighted)
    {
        renderer.material.color = highlighted ? highlightedColor : originalColor;
    }

    // Start вызывается перед обновлением первого кадра 
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }
}
