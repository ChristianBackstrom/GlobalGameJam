using UnityEngine;

public class SoundManager : MonoBehaviour
{
  #region Singleton Pattern
  public static SoundManager Instance { get; private set; }
  private void InitializeSingleton()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(this.gameObject);
    }
  }
  #endregion

  [SerializeField] private Sound[] sounds;
  [SerializeField] private AudioSource audioSourcePrefab;
  private void Awake()
  {
    InitializeSingleton();

    // Initialize audio sources
    for (int i = 0; i < sounds.Length; i++)
    {
      AudioSource source = Instantiate(audioSourcePrefab, this.transform);
      source.clip = sounds[i].clip;
      source.pitch = sounds[i].pitch;
      source.volume = sounds[i].volume;
      sounds[i].source = source;
    }
  }

  public void PlaySound(string soundName)
  {
    foreach (var sound in sounds)
    {
      if (sound.name == soundName)
      {
        sound.source.Play();
        return;
      }
    }
    Debug.LogWarning("Sound not found: " + soundName);
  }

  [System.Serializable]
  internal struct Sound
  {
    public string name;
    public AudioClip clip;
    public float pitch;
    public float volume;
    [HideInInspector]
    public AudioSource source;
  }

}
