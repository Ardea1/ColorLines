using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Матрица из клеток.
public enum CellType
{
    Empty,
    Red,
    Blue,
    Green,
    Violet,
    Orange,
    LightBlue,
    Yellow
}

public class DebugGrid : MonoBehaviour
{

    private const int SIZE = 9;

    [Header("Spheres Prefabs")]
    [SerializeField]
    private GameObject redSphere;

    [SerializeField]
    private GameObject blueSphere;

    [SerializeField]
    private GameObject greenSphere;

    [SerializeField]
    private GameObject violetSphere;

    [SerializeField]
    private GameObject orangeSphere;

    [SerializeField]
    private GameObject lightblueSphere;

    [SerializeField]
    private GameObject yellowSphere;

    private CellType[,] cells;


    // Даннй метод нужен, чтобы соотнести координаты с координатами из мира игры
    public Vector3 ToWorldCoords(Vector2Int coord)
    {
        return new Vector3 (coord.y, 0.45f, coord.x);
    }

    private Vector2Int ToLocalCoords (Vector3 coord)
    {
        return new Vector2Int(Mathf.RoundToInt (coord.z), Mathf.RoundToInt(coord.x));
    }

    // Список пустых клеток.
    private List<Vector2Int> GetEmptyCoords ()
    {
        var emptyCoords = new List<Vector2Int> ();

        // Проверяем клетки. Если пустые, то в список пустых клеток.
        for (int i = 0; i  < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (cells [j, i] == CellType.Empty)
                {
                    emptyCoords.Add(new Vector2Int(j, i));
                }
            }
        }

        return emptyCoords;
    }


    // Метод, проверяющий нахождение в границах поля
    private bool InBounds (int x, int y)
    {
        // Если данное условие выполняется, то мы находимся внутри поля
        return (x >= 0) && (x < SIZE) && (y >= 0) && (y < SIZE);
    }


    // Метод для обратного пути 
    private List<Vector2Int> GetPath (Vector2Int from, Vector2Int to, int[,] wave)
    {
        // Путь
        List<Vector2Int> path = new List<Vector2Int>();

        // Если волна не долшла
        if (wave[from.x, from.y] == -1)
            return path;

        Vector2Int[] dxdy = new Vector2Int[4]
        {
            new Vector2Int (1, 0),
            new Vector2Int (-1, 0),
            new Vector2Int (0, 1),
            new Vector2Int (0, -1)
        };

        // Текущая позиция (клетка)
        Vector2Int current = from;
        path.Add(current);

        while (current != to)
        {
            // Если не можем дойти (препятствия), то выходим из цикла
            bool stop = true;

            for (int k = 0; k < dxdy.Length; k++)
            {
                // Для обхода соседей текущей клетки
                int x = current.x + dxdy[k].x;
                int y = current.y + dxdy[k].y;

                // Проверка: находимся ли на данный момент в границах поля 
                // и явлется ли значение в рассматриваемой клетке на 1 меньше чем в текущей
                if (InBounds (x, y) && (wave[x, y] == wave[current.x, current.y] - 1))
                {
                    current = new Vector2Int (x, y);

                    // Переворачиваем путь, чтобы первый шаг был с начальной точки
                    path.Insert(0, current);

                    stop = false;
                }
            }

            if (stop)
                break;
        }

        // Если пути нет (тупик) 
        if (path.Count > 0)
        {
            if (path[0] != to)
            
                path.Clear();
            
        }

        return path;
    }

    // Наглядное изображение "Волны"
    private int[,] Wave (Vector2Int from, Vector2Int to)
    {
        int[,]  wave = new int[SIZE, SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
                wave[j, i] = cells[j, i] == CellType.Empty ? 0 : -1;
            
        }

        Vector2Int[] dxdy = new Vector2Int[4]
        {
            new Vector2Int (1, 0),
            new Vector2Int (-1, 0),
            new Vector2Int (0, 1),
            new Vector2Int (0, -1)
        };

        int d = 1;

        wave[from.x, from.y] = d;

        // Внутри данного цикла мы будем просматривать матрицу
        while (true)
        {
            bool stop = true;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                { 
                   if (wave[j, i] == d)
                    {
                        for (int k = 0; k < dxdy.Length; k++)
                        {
                            int x = j + dxdy[k].x;
                            int y = i + dxdy[k].y;

                            // Проверяем, находится ли данная координата в границах поля
                            if (InBounds (x, y) && wave [x, y] == 0)
                            {
                                wave[x, y] = d + 1;

                                stop = false;
                            }
                        }
                    }
                }
            }

            d++;

            if (wave[to.x, to.y] != 0)
                break;

            if (stop)
                break;
        }

        return wave;
    }

    // Метод генерации в выбранных hfphf,jnxbrjv координатах
    public void Generate (int x, int y, CellType cellType)
    {
        cells[x, y] = cellType;

        GameObject prefab = null;

        // В зависимости от cellType, назначаем ссылку на prefab
        switch (cellType)
        {
            case CellType.Red:
                prefab = redSphere;
                break;
            case CellType.Blue:
                prefab = blueSphere;
                break;
            case CellType.Green:
                prefab = greenSphere;
                break;
            case CellType.LightBlue:
                prefab = lightblueSphere;
                break;
            case CellType.Orange:
                prefab = orangeSphere;
                break;
            case CellType.Violet:
                prefab = violetSphere;
                break;
            case CellType.Yellow:
                prefab = yellowSphere;
                break;
        }
        Vector3 pos = ToWorldCoords(new Vector2Int(x, y));
        Instantiate(prefab, pos, Quaternion.identity);
    }

    // Сколько мы собираемся генерировать шариков.
    // Сначала получаем список пустых клеток. 
    // В эти клетки рандомно добавляем шарики.
    public int Generate (int count)
    {
        // Получаем пустые клетки
        var emptyCoords = GetEmptyCoords();

        // Если клеток нет
        if (emptyCoords.Count == 0)
            return 0;

        // Количество генерируемых шариков
        count = Mathf.Min(count, emptyCoords.Count);

        var cellTypes = Enum.GetValues(typeof(CellType)).Cast<CellType> ();
        int max = (int)cellTypes.Max() + 1;
        int min = (int)cellTypes.Min() + 1;

        for (int i = 0; i < count; i++)
        {
            // Рандомный тип выбираем здесь.
            CellType cellType = (CellType)UnityEngine.Random.Range(min, max);
            // Выбираем случайную клетку
            int index = UnityEngine.Random.Range(0, emptyCoords.Count);
            // Ставит по координатам матрицы значение cellType
            cells[emptyCoords[index].x, emptyCoords[index].y] = cellType;

            GameObject prefab = null;

            // В зависимости от cellType, назначаем ссылку на prefab
            switch (cellType)
            {
                case CellType.Red:
                    prefab = redSphere;
                    break;
                case CellType.Blue:
                    prefab = blueSphere;
                    break;
                case CellType.Green:
                    prefab = greenSphere;
                    break;
                case CellType.LightBlue:
                    prefab = lightblueSphere;
                    break;
                case CellType.Orange:
                    prefab = orangeSphere;
                    break;
                case CellType.Violet:
                    prefab = violetSphere;
                    break;
                case CellType.Yellow:
                    prefab = yellowSphere;
                    break;
            }

            Vector3 pos = ToWorldCoords(emptyCoords[index]);
            Instantiate(prefab, pos, Quaternion.identity);

            // Удаляем координату, чтобы не выбрать её же заново
            emptyCoords.RemoveAt(index);
        }

            return count;
    }

    // Метод для перевода полученных координат в те позиции, где они должны находиться в мире (Юнити)
    public List<Vector3> GetPath (Vector3 from, Vector3 to)
    {
        // Из мировых координатов в локальные
        Vector2Int _from = ToLocalCoords (from);
        Vector2Int _to = ToLocalCoords(to);

        // Распространяем волну от _from к _to
        var wave = Wave(_from, _to);
        // Получаем путь
        var path = GetPath(_to, _from, wave);
        // Полученный путь снова переводим в координаты мира
        var result = path.Select(v => ToWorldCoords(v)).ToList();

        return result;
    }

    // Метод для пересоздания 
    public void Clear ()
    {
        cells = new CellType[SIZE, SIZE];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
