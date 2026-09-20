using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] public AudioSource UiAudioSourceChannel1;
    [SerializeField] public AudioSource UiAudioSourceChannel2;
    [SerializeField] public AudioSource UiAudioSourceChannel3;
    /*Audioclips IDs:
     0-Alarm.mp3
     1-
     2-
     3-     
     */
    [SerializeField]  AudioClip[] audioClips;
    [Header("Fabricas")]
    [SerializeField]  UIFactory[] Uifactories;
    [SerializeField]  Image gallardo;
    [Header("Sprites")]
    [SerializeField]  Sprite gallardoSunglasses;
    [SerializeField]  Sprite gallardoSleepMask;
    [SerializeField]  Sprite gallardoSleepHurt;
    [SerializeField]  Sprite gallardoHurt;
    [Header("Textos")]
    [SerializeField]  public TMP_Text seedCount;
    [SerializeField]  public TMP_Text blueEggCount;
    [SerializeField]  public TMP_Text redEggCount;
    [SerializeField]  public TMP_Text greenEggCount;
    [SerializeField]  public TMP_Text waveCount;
    [SerializeField]  public TMP_Text dayCount;
    [Header("Misc")]
    private Coroutine hurtCoroutineInstance;

    bool _isSleeping = false;
    public bool isSleeping
    {
        get
        {
            return  _isSleeping;
        }
        set
        {
            gallardo.sprite = value ?  gallardoSleepMask : gallardoSunglasses;
            _isSleeping = value;
        }
    }
    void Start()
    {
        //TODO
    }
    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            if (seedCount != null) seedCount.text = "x" + ResourceManager.Instance.GetCorn().ToString();
            if (blueEggCount != null) blueEggCount.text = "x" + ResourceManager.Instance.GetAmmo(EColorType.Azul).ToString();
            if (redEggCount != null) redEggCount.text = "x" + ResourceManager.Instance.GetAmmo(EColorType.Rosado).ToString();
            if (greenEggCount != null) greenEggCount.text = "x" + ResourceManager.Instance.GetAmmo(EColorType.Verde).ToString();
        }

        if (DayNightManager.Instance != null)
        {
            if (waveCount != null)
            {
                EGamePhase phase = DayNightManager.Instance.GetActualPhase();
                if (phase == EGamePhase.Day)
                    waveCount.text = "Tienda";
                else if (phase == EGamePhase.Night)
                    waveCount.text = "Oleada " + DayNightManager.Instance.GetActualWave();
                else if (phase == EGamePhase.Meantime)
                    waveCount.text = "Descanso";
                else if (phase == EGamePhase.Victory)
                    waveCount.text = "Victoria";
                else if (phase == EGamePhase.GameOver)
                    waveCount.text = "Derrota";
            }
        }
    }

    public void HurtGallardo()
    {
        if (hurtCoroutineInstance != null)
        {
            StopCoroutine(hurtCoroutineInstance);
            hurtCoroutineInstance = null;
        }
        hurtCoroutineInstance = StartCoroutine(GallardoHurt());
        
    }
    public void Repair(int factoryID) //cambiar parametro a algo mas de ser necesario
    {
        //Reparar la fabrica
        //TODO
    }
    public void Upgrade(int factoryID) //cambiar parametro a algo mas de ser necesario
    {
        //Mejorar la fabrica
        //TODO
    }
    

    public IEnumerator GallardoHurt()
    {
        AudioSource activeChannel= SelectAudioChannel();
        gallardo.sprite = isSleeping ? gallardoSleepHurt : gallardoHurt;
        activeChannel.clip = audioClips[0];
        activeChannel.Play();
        yield return new WaitForSeconds(1);
        gallardo.sprite = isSleeping ?  gallardoSleepMask : gallardoSunglasses;
    }

    AudioSource SelectAudioChannel()
    {
        if (!UiAudioSourceChannel1.isPlaying)
        {
            return UiAudioSourceChannel1;
        }
        else if (!UiAudioSourceChannel2.isPlaying)
        {
            return UiAudioSourceChannel2;
        }
        else if (!UiAudioSourceChannel3.isPlaying)
        {
            return UiAudioSourceChannel3;
        }
        else return UiAudioSourceChannel1;
    }
}
