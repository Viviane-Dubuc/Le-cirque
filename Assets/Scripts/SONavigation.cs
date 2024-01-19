using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Navigation", menuName = "Navigation")]
public class SONavigation : ScriptableObject
{
    [SerializeField] AudioClip _commencer;
    public List<Troupe> _listeTroupe = new List<Troupe>();
    UnityEvent _evenementInitialiser = new UnityEvent();
    public UnityEvent evenementInitialiser => _evenementInitialiser;

    public void ChargerScene(string scene)
    {
        if (scene == "Intro")
        {
            GestAudio.instance.ChangerVolume(0, TypePiste.musiqueCombat);
            GestAudio.instance.ChangerVolume(0, TypePiste.musiqueSecret);
            GestAudio.instance.ChangerVolume(0, TypePiste.musiqueDefaite);
            GestAudio.instance.ChangerVolume(0, TypePiste.musiqueVictoire);
        }
        else if (scene == "Combat")
        {
            GestAudio.instance.ChangerVolume(GestAudio.instance.volumeMusiqueRef, TypePiste.musiqueCombat);
            GestAudio.instance.JouerEffetSonore(_commencer);
        }
        else if (scene == "Defaite") GestAudio.instance.ChangerVolume(GestAudio.instance.volumeMusiqueRef, TypePiste.musiqueDefaite);
        else GestAudio.instance.ChangerVolume(GestAudio.instance.volumeMusiqueRef, TypePiste.musiqueVictoire);
        SceneManager.LoadScene(scene);
        _evenementInitialiser.Invoke();
    }

    public void Quitter()
    {
        Application.Quit();
#if (UNITY_EDITOR)
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
#endif
    }
}