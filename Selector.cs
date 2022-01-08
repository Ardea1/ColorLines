using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Класс, отвечающий за выбор шарика, рэйкаст
public class Selector : MonoBehaviour
{

    public event System.Action<Sphere, Cell> OnSelected;


    [SerializeField]
    private Camera camera;

    // Выделенный шарик
    private Sphere selected;

    // Для временного блокирования возможности игрока выбрать шарик и др.
    public bool Locked { get; set; }


    // Raycast - это некоторый луч, испускаемый из некоторого объекта
    // в некотором направлении некоторой длины (либо бесконечный)
    // для определения коллизий (столкновений) с объектами. После
    // испускания луча мы получаем объект (либо массив объектов если
    // используем Physics.RaycastAll), с которыми он столкнулся и далее
    // можем определить попали ли в нужный нам объект. Часто используется
    // в стрельбе. 

    // С помощью T мы можем указать какой именно компонент необходимо получить (sell, sphere и др.)
    private T Raycast<T> () where T : Component
    {
        //Сюда запишется информация о пересечении луча, если оно будет
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
            return hit.transform.GetComponent<T>();

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Locked)
            return;

        if (Input.GetMouseButtonUp (0))
        {
            var sphere = Raycast<Sphere>();

            if (sphere != null)
            {
                selected = sphere;
            }

            else
            {
                // Если шар выделен
                if (selected != null)
                {
                    var cell = Raycast<Cell>();

                    if (cell != null)
                        OnSelected?.Invoke(selected, cell);

                    selected = null;
                }
            }
        }
    }
}
