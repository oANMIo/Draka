using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private TMPro.TextMeshProUGUI _textPress;
    [SerializeField] private TMPro.TextMeshProUGUI _textLevel;
    [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
    [SerializeField] private string[] _title;
    [SerializeField] private Player _player;

    private void Start()
    {
        if (_fadeImage == null)
        {
            _fadeImage = GetComponent<Image>();
            _title = new string[6];
        }

        _fadeImage.gameObject.SetActive(true); // .enabled = true;
        int scene = SceneManager.GetActiveScene().buildIndex - 1;

        _textLevel.text = "Уровень " + scene.ToString();
        _textTitle.text = _title[scene];

        Time.timeScale = 0;
        _player.gameObject.GetComponent<PlayerAttack>().enabled = false;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _textLevel.enabled = false;
            _textTitle.enabled = false;
            _textPress.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        Color c = _fadeImage.color;

        for (float t = _fadeDuration; t >= 0; t -= Time.unscaledDeltaTime)
        {
           
            c.a = t / _fadeDuration;
            _fadeImage.color = c;
            yield return null;
        }

        c.a = 0;
        _fadeImage.color = c;
        _fadeImage.enabled = false;

        Time.timeScale = 1f;
        _player.gameObject.GetComponent<PlayerAttack>().enabled = true;
        _player.gameObject.GetComponent<PlayerMovement>().enabled = true;

        yield return null;
    }
}
