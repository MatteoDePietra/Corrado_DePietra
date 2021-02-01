using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField]
    internal string name;
    [SerializeField]
    internal AudioClip clip;
    [SerializeField]
    internal AudioSource source;
    [Range(0f,1f)]
    [SerializeField]
    internal float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    [SerializeField]
    internal float pitch = 0.5f;
    [Range(0f, 0.5f)]
    [SerializeField]
    internal float randomVolume = 0.5f;
    [Range(0f, 0.5f)]
    [SerializeField]
    internal float randomPitch = 0.5f;
    [SerializeField]
    internal bool loop = false;
    internal void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }
    internal void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    internal void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    internal Sound[] sounds;
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Music");
    }
    internal void SetVolumeSound(float _volume, string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioSource _go = this.transform.GetChild(i).GetComponent<AudioSource>();
            _go.volume = 0.5f * _volume;
        }
    }
    internal void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        Debug.LogWarning("sound non trovato");
    }
    internal void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        Debug.LogWarning("sound non trovato");
    }
}