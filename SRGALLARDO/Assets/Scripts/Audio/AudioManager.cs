using UnityEngine;

public class AudioManager : MonoBehaviour

{
    public static AudioManager Instance {get; private set;}

    [Header("Clips del combate (genericos)")]
    [SerializeField] private AudioClip clipShoot;
    [SerializeField] private AudioClip clipImpact;
    [SerializeField] private AudioClip clipReachedCoop;

    [Header("Clips de derota de enemigos R-A-V")]
    [SerializeField] private AudioClip[] arrClipsEnemyDefeatType;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShoot() => PlayClip(clipShoot);
    public void PlayImpact() => PlayClip(clipImpact);
    public void PlayReachedCoop() => PlayClip(clipReachedCoop);

    public void PlayEnemyDefeat(EColorType eNewType)
    {
        int iIndex = (int)eNewType;

        if (arrClipsEnemyDefeatType != null && iIndex < arrClipsEnemyDefeatType.Length)
        {
            PlayClip(arrClipsEnemyDefeatType[iIndex]);
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }

}
