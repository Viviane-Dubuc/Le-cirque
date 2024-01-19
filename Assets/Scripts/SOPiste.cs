using UnityEngine;

[CreateAssetMenu(menuName = "Piste musicale", fileName = "DonneesPiste")]
public class SOPiste : ScriptableObject
{
    [SerializeField] AudioClip _clip;
    public AudioClip clip => _clip;
    [SerializeField] bool _estActifParDefaut;
    [SerializeField] TypePiste _typePiste;
    public TypePiste typePiste => _typePiste;
    AudioSource _source;
    public AudioSource source => _source;

    public void Initialiser(AudioSource source)
    {
        _source = source;
        _source.clip = _clip;
        _source.loop = true;
        _source.playOnAwake = false;
        _source.Play();
        AjusterVolume();
    }

    public void AjusterVolume()
    {
        if (_estActifParDefaut) _source.volume = GestAudio.instance.volumeMusiqueRef;
        else _source.volume = 0;
    }
}
