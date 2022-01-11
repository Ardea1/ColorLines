using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    /// <summary>
    /// Событие об окончании движения сферы
    /// </summary>
    public event System.Action<Sphere> OnMoveComplete;

    /// <summary>
    /// Путь сферы
    /// </summary>
    private List<Vector3> path;

    /// <summary>
    /// Текущий индекс
    /// </summary>
    private int index;

    /// <summary>
    /// Метод, показывающий движение шарика
    /// </summary>
    private IEnumerator Move()
    {
        while (index != path.Count)
        {
            transform.position = path[index];

            yield return new WaitForSeconds(0.10f);

            index++;
        }

        OnMoveComplete?.Invoke(this);
    }

    /// <summary>
    /// Метод для движения шарика
    /// </summary>
    public void Move(List<Vector3> path)
    {
        this.path = path;
        index = 0;

        StartCoroutine(Move());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
