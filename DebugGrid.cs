using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Матрица из клеток.
/// </summary>
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

    /// <summary>
    /// Даннй метод нужен, чтобы соотнести координаты с координатами из мира игры
    /// </summary>
    public Vector3 ToWorldCoords(Vector2Int coord)
    {
        return new Vector3(coord.y, 0.45f, coord.x);
    }

    private Vector2Int ToLocalCoords(Vector3 coord)
    {
        return new Vector2Int(Mathf.RoundToInt(coord.z), Mathf.RoundToInt(coord.x));
    }

    /// <summary>
    /// Список пустых клеток.
    /// </summary>
    private List<Vector2Int> GetEmptyCoords()
    {
        var emptyCoords = new List<Vector2Int>();

        // Проверяем клетки. Если пустые, то в список пустых клеток.
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (cells[j, i] == CellType.Empty)
                {
                    emptyCoords.Add(new Vector2Int(j, i));
                }
            }
        }

        return emptyCoords;
    }

    /// <summary>
    /// Метод, проверяющий нахождение в границах поля
    /// </summary>
    private bool InBounds(int x, int y)
    {
        // Если данное условие выполняется, то мы находимся внутри поля
        return (x >= 0) && (x < SIZE) && (y >= 0) && (y < SIZE);
    }

    /// <summary>
    /// Метод, возвращающий обратный путь шарика
    /// </summary>
    private List<Vector2Int> GetPath(Vector2Int from, Vector2Int to, int[,] wave)
    {
        // Путь
        List<Vector2Int> path = new List<Vector2Int>();

        // Если волна не дошла до конечной точки
        if (wave[from.x, from.y] == -1)
            return path;

        // Создаём смещения
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

        // Цикл длится, пока мы не дошли до конечной позиции
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
                if (InBounds(x, y) && (wave[x, y] == wave[current.x, current.y] - 1))
                {
                    current = new Vector2Int(x, y);

                    // Переворачиваем путь, чтобы первый шаг был с начальной точки
                    path.Insert(0, current);

                    stop = false;

                    break;
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


    private int[,] Wave(Vector2Int from, Vector2Int to)
    {
        int[,] wave = new int[SIZE, SIZE];

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
                            if (InBounds(x, y) && wave[x, y] == 0)
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

    /// <summary>
    /// Сколько мы собираемся генерировать шариков.
    /// Сначала получаем список пустых клеток. 
    /// В эти клетки рандомно добавляем шарики.
    /// </summary>
    public int Generate(int count)
    {
        // Получаем пустые клетки
        var emptyCoords = GetEmptyCoords();

        // Если клеток нет
        if (emptyCoords.Count == 0)
            return 0;

        // Количество генерируемых шариков
        count = Mathf.Min(count, emptyCoords.Count);

        var cellTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>();
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

    /// <summary>
    /// Метод для перевода полученных координат в те позиции, где они должны находиться в мире
    /// </summary>
    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        // Из мировых координат в локальные
        Vector2Int _from = ToLocalCoords(from);
        Vector2Int _to = ToLocalCoords(to);

        // Распространяем волну от _from к _to
        var wave = Wave(_from, _to);
        // Получаем путь
        var path = GetPath(_to, _from, wave);
        // Полученный путь снова переводим в координаты мира
        var result = path.Select(v => ToWorldCoords(v)).ToList();

        return result;
    }


    /// <summary>
    /// В данном методе мы меняем местами позиции to и from
    /// </summary>
    public void Move(Vector3 from, Vector3 to)
    {
        // Из мировых координат в локальные
        Vector2Int _from = ToLocalCoords(from);
        Vector2Int _to = ToLocalCoords(to);

        // Меняем местами значения из позиции to в позицию from
        cells[_to.x, _to.y] = cells[_from.x, _from.y];
        cells[_from.x, _from.y] = CellType.Empty;
    }

    /// <summary>
    /// Метод, для записи данных о смещениях в цикле в HashSet
    /// </summary>
    private HashSet<Vector2Int> CheckLine(Vector2Int pos, int dx, int dy)
    {
        HashSet<Vector2Int> line = new HashSet<Vector2Int>();

        for (int i = 0; i < SIZE; i++)
        {
            int x = pos.x + i * dx;
            int y = pos.y + i * dy;

            if (InBounds(x, y))
            {
                // Проверяем, если такой же цвет клетки как у той, на которой стоит игрок, то
                if (cells[x, y] == cells[pos.x, pos.y])
                {
                    line.Add(new Vector2Int(x, y));
                }
                // Если другой цвет
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return line;
    }

    /// <summary>
    /// Метод для распознавания линии из 5 шариков
    /// и её уничтожения
    /// </summary>
    public int DestroyLines(Vector3 pos)
    {
        Vector2Int _pos = ToLocalCoords(pos);

        // HashSet для хранения сфер, которые надо удалить
        HashSet<Vector2Int> destroyed = new HashSet<Vector2Int>();

        // HashSet для текущей удаляемой линии
        HashSet<Vector2Int> line = new HashSet<Vector2Int>();

        List<Tuple<Vector2Int, Vector2Int>> dxdy = new List<Tuple<Vector2Int, Vector2Int>>()
        {
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int (1, 0), new Vector2Int (-1, 0)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int (0, 1), new Vector2Int (0, -1)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int (1, 1), new Vector2Int (-1, -1)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int (1, -1), new Vector2Int (-1, 1)),
        };

        for (int i = 0; i < dxdy.Count; i++)
        {

            // Item1  это первый вектор, Item2 - второй
            line.UnionWith(CheckLine(_pos, dxdy[i].Item1.x, dxdy[i].Item1.y));
            line.UnionWith(CheckLine(_pos, dxdy[i].Item2.x, dxdy[i].Item2.y));

            // Проверяем, состоит ли линия из пяти и более одинаковых шаров,
            // объединяем линию и удаляем если состоит
            if (line.Count >= 5)
                destroyed.UnionWith(line);

            line.Clear();
        }
        
        // Получаем все сферы
        List<Sphere> spheres = FindObjectsOfType<Sphere>().ToList();

        foreach (var sphere in spheres)
        {
            Vector2Int spherePos = ToLocalCoords(sphere.transform.position);

            // Если сферы находятся в позиции, в которых они должны быть удалены,
            // то удаляем эти сферы  
            if (destroyed.Contains(spherePos))
            {
                Destroy(sphere.gameObject);

                // Обнуляем клетку там, где мы удалили сферу
                cells[spherePos.x, spherePos.y] = CellType.Empty;


            }
        }

        // Возвращаем количество уничтоженных сфер для подсчёта очков
        return destroyed.Count;
    }

    /// <summary>
    /// Метод для пересоздания поля
    /// </summary>
    public void Clear()
    {
        cells = new CellType[SIZE, SIZE];
    }

    // Start is called before the first frame update
    void Start()
    {

    }


}
