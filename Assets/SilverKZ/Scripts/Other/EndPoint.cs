using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    //[SerializeField] private int _indexNextScene;

    private void Awake()
    {
        if (_fadeImage == null)
        {
            _fadeImage = GetComponent<Image>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeOut()
    {
        Color c = _fadeImage.color;

        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0, 1, t / _fadeDuration);
            _fadeImage.color = c;
            yield return null;
        }

        c.a = 1;
        _fadeImage.color = c;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene == 7)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
}
