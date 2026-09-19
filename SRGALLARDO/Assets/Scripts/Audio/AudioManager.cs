using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour

{
    public static AudioManager Instance { get; private set; }

    // Audio Mixer Groups para controlar el volumen maestro de cada categoría//
    [Header("Audio Mixer Group")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup ambientGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;


    [Header("Fuentes de audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música y Ambientes")]
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip dayMusic;
    [SerializeField] private AudioClip nightMusic;
    [SerializeField] private AudioClip dayAmbient;
    [SerializeField] private AudioClip nightAmbient;

    [Header("Clips del combate (genericos)")]
    [SerializeField] private AudioClip clipShoot;
    [SerializeField] private AudioClip clipImpact;
    [SerializeField] private AudioClip clipReachedCoop; // Sonido de alarma cuando el enemigo entra a la granja

    [Header("Clips de derota de enemigos R-A-V (Águila, Serpiente, Lobo)")]
    [SerializeField] private AudioClip[] arrClipsEnemyDefeatType;

    [Header("Clips de Tienda / UI")]
    [SerializeField] private AudioClip clipUpgrade;
    [SerializeField] private AudioClip clipRepair;
    [SerializeField] private AudioClip clipButtonHover;
    [SerializeField] private AudioClip clipButtonClick;

    [Header("Ajustes de Mezcla en  Código")]
    //"El Volumen máximo al que llegará la música después del crossfade."
    [Range(0f, 1f)][SerializeField] private float musicVolume = 0.8f;
    //"El Volumen máximo al que llegará el ambiente después del crossfade."
    [Range(0f, 1f)][SerializeField] private float ambientVolume = 0.45f;
    //"El Volumen máximo al que llegará el SFX después del crossfade."
    [SerializeField] private float crossfadeDuration = 2.0f;


    // Referencias a las corrutinas actuales para poder detenerlas si el jugador cambia de fase muy rápido
    private Coroutine musicCrossfadeRoutine;
    private Coroutine ambientCrossfadeRoutine;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Evita que este GameObject se destruya al cargar una nueva escena de menu a gameplay etc .
        DontDestroyOnLoad(gameObject);


        //----- conexion mixer groups con audio sources 
        // Conecta los AudioSources con sus respectivos canales del Mixer para respetar la mezcla de volumen.
        if (musicSource) musicSource.outputAudioMixerGroup = musicGroup;
        if (ambientSource) ambientSource.outputAudioMixerGroup = ambientGroup;
        if (sfxSource) sfxSource.outputAudioMixerGroup = sfxGroup;
    }


    //CONTROL DE FASES (DÍA / NOCHE / MENÚ)


    //Reproduce la música de inicio. Silencia los ambientes orgánicos.
    public void PlayIntro()
    {
        PlayMusic(introMusic);
        if (ambientSource) ambientSource.Stop();
    }


    /// Inicia la mezcla de audio para la oleada diurna (Música Día + Ambiente Día).

    public void PlayDayPhase()
    {
        PlayMusic(dayMusic);
        PlayAmbient(dayAmbient);
    }

    /// Inicia la mezcla de audio para la oleada nocturna (Música Noche + Ambiente Noche).

    public void PlayNightPhase()
    {
        PlayMusic(nightMusic);
        PlayAmbient(nightAmbient);
    }

    // trancisiones de audio entre fases

    private void PlayMusic(AudioClip newClip)
    {
        if (musicCrossfadeRoutine != null) StopCoroutine(musicCrossfadeRoutine);
        musicCrossfadeRoutine = StartCoroutine(CrossfadeSource(musicSource, newClip, musicVolume));
    }

    private void PlayAmbient(AudioClip newClip)
    {
        if (ambientCrossfadeRoutine != null) StopCoroutine(ambientCrossfadeRoutine);
        ambientCrossfadeRoutine = StartCoroutine(CrossfadeSource(ambientSource, newClip, ambientVolume));
    }

    private IEnumerator CrossfadeSource(AudioSource source, AudioClip newClip, float targetVolume)
    {
        // Si el clip que intentamos reproducir ya está sonando, no hacemos nada.
        if (source.clip == newClip && source.isPlaying) yield break;

        //  Fade Out (Bajar el volumen del clip actual hasta 0)
        if (source.isPlaying)
        {
            float startVol = source.volume;
            while (source.volume > 0)
            {
                source.volume -= startVol * Time.deltaTime / crossfadeDuration;
                yield return null;
            }
            source.Stop();
        }

        //  Cambio de pista
        source.clip = newClip;
        source.volume = 0f;
        source.Play();

        //  Fade In (Subir el volumen progresivamente hasta el targetVolume)
        while (source.volume < targetVolume)
        {
            source.volume += targetVolume * Time.deltaTime / crossfadeDuration;
            yield return null;
        }

        // Asegurar que el volumen quede exactamente en el objetivo al terminar
        source.volume = targetVolume;
    }

    // COMBATE Y EVENTOS GLOBALES



    /// Reproduce el sonido del cañón
    public void PlayShoot()
    {
        PlaySFX(clipShoot);
    }
    /// Reproduce el sonido húmedo del huevo estrellándose.
    public void PlayImpact() => PlaySFX(clipImpact);
    /// Dispara la alarma cuando un enemigo vulnera el granero.
    public void PlayReachedCoop() => PlaySFX(clipReachedCoop);

    /// Reproduce el sonido de derrota correspondiente al tipo de enemigo
    public void PlayEnemyDefeat(EColorType eNewType)
    {
        int iIndex = (int)eNewType;
        // Verifica que el arreglo no esté vacío y que el índice exista para evitar errores (IndexOutOfRange)
        if (arrClipsEnemyDefeatType != null && iIndex < arrClipsEnemyDefeatType.Length)
        {
            PlaySFX(arrClipsEnemyDefeatType[iIndex]);
        }
    }

    // TIENDA Y MENÚ (UI)
    public void PlayUpgrade() => PlaySFX(clipUpgrade);
    public void PlayRepair() => PlaySFX(clipRepair);
    public void PlayButtonHover() => PlaySFX(clipButtonHover);
    public void PlayButtonClick() => PlaySFX(clipButtonClick);

    /// Motor centralizado para reproducir cualquier efecto de sonido corto.
    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(clip);
    }
}