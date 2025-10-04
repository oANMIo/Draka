using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour 
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject Autors; 

    private bool isVideoPlaying = false;
    private bool isAuthorsVisible = false; 

    [Tooltip("Саунд-эффект, который проигрывается при открытии/закрытии")]
    public AudioClip clickSound;
    public float volume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        // Скрываем окно авторов при старте
        if (Autors != null)
        {
            Autors.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Если окно авторов активно, скрываем его при клике
        if (isAuthorsVisible)
        {
            PlayClickSound();
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }
    }

    public void PlayGame()
    {
    SceneManager.LoadScene(1);
    }

    public void Info()
    {
        if (Autors != null)
        {
            PlayClickSound();
            Autors.SetActive(true);
            isAuthorsVisible = true;
        }
    }

    public void Exit()
    {
        PlayClickSound();
        Application.Quit();
    }

    private void CloseInfo()
    {
        PlayClickSound();
        Autors.SetActive(false);
        isAuthorsVisible = false;
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
    }
}
