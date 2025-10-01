using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickup;
    [SerializeField] private AudioClip _maleDamage;
    [SerializeField] private AudioClip _swing;
    [SerializeField] private AudioClip _hit;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _trash;

    private AudioSource _audioSource;
    private static AudioManager _instance = null;

    public enum Clip
    {
        CoinPickup,
        MaleDamage,
        Swing,
        Hit,
        Jump,
        Trash
    }

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(Clip clip)
    {
        AudioClip currentClip;

        switch (clip)
        {
            case AudioManager.Clip.CoinPickup:
                currentClip = _coinPickup;
                break;
            case AudioManager.Clip.MaleDamage:
                currentClip = _maleDamage;
                break;
            case AudioManager.Clip.Swing:
                currentClip = _swing;
                break;
            case AudioManager.Clip.Hit:
                currentClip = _hit;
                break;
            case AudioManager.Clip.Jump:
                currentClip = _jump;
                break;
            case AudioManager.Clip.Trash:
                currentClip = _trash;
                break;
            default:
                currentClip = _coinPickup;
                break;
        }

        _audioSource.PlayOneShot(currentClip, 1f);
    }
}
