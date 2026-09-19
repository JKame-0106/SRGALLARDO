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
        //TODO
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
