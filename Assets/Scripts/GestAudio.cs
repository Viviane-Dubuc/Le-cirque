using System.Collections;
using UnityEngine;

public class GestAudio : MonoBehaviour
{
    [SerializeField] float _dureeFondu = 1;
    [SerializeField] float _pitchMaxEffetSonore = 1.1f;
    [SerializeField] float _pitchMinEffetSonore = 0.9f;
    [SerializeField, Range(0, 1)] float _volumeMusiqueRef = 0.5f;
    public float volumeMusiqueRef => _volumeMusiqueRef;
    [SerializeField] SOPiste[] _tPistes;
    public SOPiste[] tPistes => _tPistes;
    AudioSource _sourceEffetsSonores;
    static GestAudio _instance;
    static public GestAudio instance => _instance;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else { Destroy(gameObject); return; }
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        CreerLesSourcesAudio();
    }

    void CreerLesSourcesAudio()
    {
        foreach (SOPiste piste in _tPistes)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            piste.Initialiser(source);
        }
    }

    public void JouerEffetSonore(AudioClip clip)
    {
        _sourceEffetsSonores.pitch = Random.Range(_pitchMinEffetSonore, _pitchMaxEffetSonore);
        _sourceEffetsSonores.PlayOneShot(clip);
    }
    public void ChangerVolume(float volume, TypePiste typePiste)
    {
        foreach (SOPiste piste in _tPistes) if (piste.typePiste == typePiste) StartCoroutine(CoroutineChangerVolume(piste, volume));
    }

    IEnumerator CoroutineChangerVolume(SOPiste piste, float volumeFinal)
    {
        float tempsInitial = Time.time;
        float tempsActuel = tempsInitial;
        float tempsFinal = tempsInitial + _dureeFondu;
        float volumeInitial = piste.source.volume;
        _volumeMusiqueRef = volumeFinal;
        while (tempsActuel < tempsFinal)
        {
            tempsActuel = Time.time;
            float pourcentage = (tempsActuel - tempsInitial) / _dureeFondu;
            float nouveauVolume = Mathf.Lerp(volumeInitial, volumeFinal, pourcentage);
            piste.source.volume = nouveauVolume;
            yield return null;
        }
    }
}