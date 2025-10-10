using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject Autors;
    [SerializeField] private GameObject Settings;

    private bool isVideoPlaying = false;
    private bool isAuthorsVisible = false;
    private bool isSettingsVisible = false;

    public AudioClip clickSound;
    public float volume = 1f;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (Autors != null)
        {
            Autors.SetActive(false);
        }
        if (Settings != null)
        {
            Settings.SetActive(false);
        }
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAuthorsVisible)
        {
            PlayClickSound();
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }

        if (isSettingsVisible)
        {
            PlayClickSound();
            Settings.SetActive(false);
            isSettingsVisible = false;
        }
    }

    public float delayBeforeLoad = 1f; 

    public void PlayGame()
    {
        PlayClickSound();
        StartCoroutine(LoadSceneAfterDelay(delayBeforeLoad));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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

    public void CloseInfo()
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

    public void OpenSettings()
    {
        PlayClickSound();
        Settings.SetActive(true);
        isSettingsVisible = true;
    }

    public void CloseSettings()
    {
        PlayClickSound();
        Settings.SetActive(false);
        isSettingsVisible = false;
    }
}
