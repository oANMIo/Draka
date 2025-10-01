using UnityEngine;
using System.Collections;

public class imxa : MonoBehaviour
{
    public GameObject[] objects; // массив объектов для активации
    public float activeDuration = 2f; // время, в течение которого объект будет активен

    private Coroutine currentCoroutine = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateObject();
        }
    }

    void ActivateObject()
    {
        // отключаем текущий активный объект, если есть
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // выбираем случайный объект для активации
        if (objects.Length == 0) return;
        int index = Random.Range(0, objects.Length);

        // активируем выбранный объект
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
        objects[index].SetActive(true);

        // запускаем корутину, которая отключит объект через некоторое время
        currentCoroutine = StartCoroutine(DeactivateAfterDelay(objects[index]));
    }

    IEnumerator DeactivateAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(activeDuration);
        obj.SetActive(false);
        currentCoroutine = null;
    }
}