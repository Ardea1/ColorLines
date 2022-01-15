using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sphere : MonoBehaviour
{
    /// <summary>
    /// Событие об окончании движения сферы
    /// </summary>
    public event System.Action<Sphere> OnMoveComplete;

    /// <summary>
    /// Метод для движения шарика
    /// </summary>
    public void Move(List<Vector3> path)
    {
        transform.DOPath(path.ToArray(), path.Count * 0.15f).OnComplete(() => OnMoveComplete?.Invoke(this));
    }

    /// <summary>
    /// Метод для отображения удаления линии
    /// </summary>
    public void Destroy()
    {
        transform.DOScale(0.1f, 0.25f).OnComplete(() =>
        {
            GameObject.Destroy(gameObject);
        });
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
