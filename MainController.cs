using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private DebugGrid grid;

    [SerializeField]
    private Selector selector;

    private StepData stepData;

    /// <summary>
    /// Начинаем движение шарика
    /// </summary>
    private void StartStep()
    {
        // Блокировка селектора
        selector.Locked = true;

        // Обновление позиции шарика
        grid.Move(stepData.Sphere.transform.position, stepData.Cell.transform.position);

        // Включаем подсветку клетки
        stepData.Cell.Highlight(true);

        // Подписываемся на завершение события
        stepData.Sphere.OnMoveComplete += Sphere_OnMoveComplete;
        stepData.Sphere.Move(stepData.Path);
    }

    /// <summary>
    /// Завершаем движение шарика
    /// </summary>
    private void CompleteStep()
    {
        // Разблокировка селектора
        selector.Locked = false;

        // Отписываемся от события
        stepData.Sphere.OnMoveComplete -= Sphere_OnMoveComplete;

        // Отключаем подсветку клетки
        stepData.Cell.Highlight(false);

        var destroyed = grid.DestroyLines(stepData.Cell.transform.position);

        grid.Generate(3);
    }

    /// <summary>
    /// В данном методе мы вызываем метод, завершающий движение шарика
    /// </summary>
    private void Sphere_OnMoveComplete(Sphere sender)
    {
        CompleteStep();
    }

    private void Selector_OnSelected(Sphere sphere, Cell cell)
    {
        // Если путь существует, то запускаем анимацию шарика.
        // Находим одинаковые линии, если нашли, то линии уничтожаются

        var spherePos = sphere.transform.position;
        var cellPos = cell.transform.position;

        // Если мы выбрали сферу, а потом нажали на неё же
        if (spherePos.x == cellPos.x && spherePos.z == cellPos.z)
            return;

        var path = grid.GetPath(spherePos, cellPos);
        if (path.Count > 0)
        {
            stepData = new StepData(sphere, cell, path);

            StartStep();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Подписываемся на событие
        selector.OnSelected += Selector_OnSelected;

        grid.Clear();
        grid.Generate(3);
    }



    // Update is called once per frame
    void Update()
    {

    }
}

/// <summary>
/// Класс, для хранения данных шага
/// </summary>
public class StepData
{
    public Sphere Sphere
    {
        get;
        private set;
    }

    public Cell Cell
    {
        get;
        private set;
    }

    public List<Vector3> Path
    {
        get;
        private set;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    public StepData(Sphere sphere, Cell cell, List<Vector3> path)
    {
        Sphere = sphere;
        Cell = cell;
        Path = path;
    }
}
