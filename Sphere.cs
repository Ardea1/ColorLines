using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins.Core.PathCore;

public class Sphere : MonoBehaviour
{
    /// <summary>
    /// Событие об окончании движения сферы
    /// </summary>
    public event System.Action<Sphere> OnMoveComplete;

    private TweenerCore<Vector3, Path, PathOptions> moveTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> scaleTween;

    /// <summary>
    /// Метод для движения шарика
    /// </summary>
    public void Move(List<Vector3> path)
    {
        moveTween = transform.DOPath(path.ToArray(), path.Count * 0.1f).OnComplete(() => OnMoveComplete?.Invoke(this));
    }

    /// <summary>
    /// Метод для отображения удаления линии
    /// </summary>
    public void Destroy()
    {
        scaleTween = transform.DOScale(0.1f, 0.25f).OnComplete(() =>
        {
            GameObject.Destroy(gameObject);
        });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {
        moveTween?.Kill();
        scaleTween.Kill();
    }
}
