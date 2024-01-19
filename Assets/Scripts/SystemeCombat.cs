using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PhaseCombat
{
    COMMENCER, TOURPERSO1, TOURPERSO2, TOURPERSO3, TOURPERSO4, TOURENNEMI, GAGNER, PERDRE
}

public class SystemeCombat : MonoBehaviour
{
    [SerializeField] AudioClip _attaquerEnnemi;
    [SerializeField] AudioClip _attaquerPerso;
    [SerializeField] AudioClip _gagner;
    [SerializeField] AudioClip _hurt;
    [SerializeField] AudioClip _manquer;
    [SerializeField] AudioClip _soigner;
    [SerializeField] List<AffichageCombat> _affichagesPerso;
    [SerializeField] List<Button> _boutons;
    [SerializeField] List<GameObject> _listeEnnemis;
    [SerializeField] List<GameObject> _listePersos;
    [SerializeField] List<Sprite> _listeSalles;
    [SerializeField] List<Transform> _stationCombat;
    [SerializeField] SONavigation _navigation;
    [SerializeField] SpriteRenderer _salle;
    [SerializeField] TextMeshProUGUI _texteDialogue;
    private List<Troupe> _listeTroupe = new List<Troupe>();
    private PhaseCombat _phase;
    private Troupe _ennemi;
    private Troupe _perso1;
    private Troupe _perso2;
    private Troupe _perso3;
    private Troupe _perso4;
    private int _combat = 0;

    void Start()
    {
        CommencerPartie();
    }

    private void CommencerPartie()
    {
        _phase = PhaseCombat.COMMENCER;
        StartCoroutine(CommencerCombat());
    }

    private IEnumerator CommencerCombat()
    {
        int random = Random.Range(0, 4);
        if (_combat == 2 && random != 0) _combat = 3;
        else if (_combat == 2)
        {
            GestAudio.instance.ChangerVolume(GestAudio.instance.volumeMusiqueRef, TypePiste.musiqueSecret);
            GestAudio.instance.ChangerVolume(0, TypePiste.musiqueCombat);
        }
        ActiverBoutons(false);
        GameObject GOEnnemi = Instantiate(_listeEnnemis[_combat], _stationCombat[0]);
        _ennemi = GOEnnemi.GetComponent<Troupe>();
        _salle.sprite = _listeSalles[_combat];
        if (_combat == 0)
        {
            MelangerListe(_listePersos);
            GameObject GOPerso1 = Instantiate(_listePersos[0], _stationCombat[1]);
            _perso1 = GOPerso1.GetComponent<Troupe>();
            _listeTroupe.Add(_perso1);
            GameObject GOPerso2 = Instantiate(_listePersos[1], _stationCombat[2]);
            GOPerso2.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _perso2 = GOPerso2.GetComponent<Troupe>();
            _listeTroupe.Add(_perso2);
            GameObject GOPerso3 = Instantiate(_listePersos[2], _stationCombat[3]);
            GOPerso3.GetComponent<SpriteRenderer>().sortingOrder = 2;
            _perso3 = GOPerso3.GetComponent<Troupe>();
            _listeTroupe.Add(_perso3);
            GameObject GOPerso4 = Instantiate(_listePersos[3], _stationCombat[4]);
            GOPerso4.GetComponent<SpriteRenderer>().sortingOrder = 3;
            _perso4 = GOPerso4.GetComponent<Troupe>();
            _listeTroupe.Add(_perso4);
            _navigation._listeTroupe = _listeTroupe;
        }
        else Destroy(_stationCombat[0].GetChild(0).gameObject);
        _texteDialogue.text = MettreMajuscule(_ennemi.donnees.nom) + " se présente!";
        Afficher();
        yield return new WaitForSeconds(2);
        _phase = PhaseCombat.TOURPERSO1;
        StartCoroutine(TourPerso(_perso1));
    }

    private IEnumerator TourPerso(Troupe perso)
    {
        if (perso.donnees.PVActuel <= 0)
        {
            _texteDialogue.text = MettreMajuscule(perso.donnees.nom) + " ne peut plus combattre!";
            yield return new WaitForSeconds(2);
            GererTour();
        }
        else
        {
            ChangerBouton(perso);
            StartCoroutine(SeDeplacer(perso.gameObject.transform, -1));
            _texteDialogue.text = "Que va faire " + perso.donnees.nom + "?";
        }
    }

    public void CliquerPoing()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) StartCoroutine(PersoAttaquer(_perso1, false));
        else if (_phase == PhaseCombat.TOURPERSO2) StartCoroutine(PersoAttaquer(_perso2, false));
        else if (_phase == PhaseCombat.TOURPERSO3) StartCoroutine(PersoAttaquer(_perso3, false));
        else StartCoroutine(PersoAttaquer(_perso4, false));
    }

    public void CliquerPied()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) StartCoroutine(PersoAttaquer(_perso1, true));
        else if (_phase == PhaseCombat.TOURPERSO2) StartCoroutine(PersoAttaquer(_perso2, true));
        else if (_phase == PhaseCombat.TOURPERSO3) StartCoroutine(PersoAttaquer(_perso3, true));
        else StartCoroutine(PersoAttaquer(_perso4, true));
    }

    public void CliquerAttaque()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) StartCoroutine(PersoAttaquer(_perso1, false));
        else if (_phase == PhaseCombat.TOURPERSO2) StartCoroutine(PersoAttaquer(_perso2, false));
        else if (_phase == PhaseCombat.TOURPERSO3) StartCoroutine(PersoAttaquer(_perso3, false));
        else StartCoroutine(PersoAttaquer(_perso4, false));
    }

    public void CliquerEpee()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) StartCoroutine(PersoAttaquer(_perso1, true));
        else if (_phase == PhaseCombat.TOURPERSO2) StartCoroutine(PersoAttaquer(_perso2, true));
        else if (_phase == PhaseCombat.TOURPERSO3) StartCoroutine(PersoAttaquer(_perso3, true));
        else StartCoroutine(PersoAttaquer(_perso4, true));
    }

    public void CliquerMagie()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) PersoMagie(_perso1);
        else if (_phase == PhaseCombat.TOURPERSO2) PersoMagie(_perso2);
        else if (_phase == PhaseCombat.TOURPERSO3) PersoMagie(_perso3);
        else PersoMagie(_perso4);
    }

    public void CliquerSoin()
    {
        ActiverBoutons(false);
        if (_phase == PhaseCombat.TOURPERSO1) StartCoroutine(PersoSoin(_perso1));
        else if (_phase == PhaseCombat.TOURPERSO2) StartCoroutine(PersoSoin(_perso2));
        else if (_phase == PhaseCombat.TOURPERSO3) StartCoroutine(PersoSoin(_perso3));
        else StartCoroutine(PersoSoin(_perso4));
    }

    private IEnumerator PersoAttaquer(Troupe perso, bool estFort)
    {
        GestAudio.instance.JouerEffetSonore(_attaquerPerso);
        if (estFort) perso._spriteRenderer.sprite = perso.donnees._critique;
        else perso._spriteRenderer.sprite = perso.donnees._attaque;
        _texteDialogue.text = MettreMajuscule(perso.donnees.nom) + " attaque!";
        yield return new WaitForSeconds(2);
        string ennemiMort = _ennemi.PrendreDegat(perso.donnees.PA, estFort);
        if (!ennemiMort.Contains("Raté!")) GestAudio.instance.JouerEffetSonore(_hurt);
        else GestAudio.instance.JouerEffetSonore(_manquer);
        _texteDialogue.text = ennemiMort;
        Afficher();
        yield return new WaitForSeconds(2);
        StartCoroutine(SeDeplacer(perso.gameObject.transform, 1));
        perso._spriteRenderer.sprite = perso.donnees._idle;
        GererTour();
    }

    private void PersoMagie(Troupe perso)
    {
        if (perso.donnees.PMActuel == 0)
        {
            StartCoroutine(SeDeplacer(perso.gameObject.transform, 1));
            GestAudio.instance.JouerEffetSonore(_manquer);
            _texteDialogue.text = "Raté!";
            GererTour();
        }
        else
        {
            perso.donnees.PMActuel -= 10;
            StartCoroutine(PersoAttaquer(perso, true));
        }
    }

    private IEnumerator PersoSoin(Troupe perso)
    {
        if (perso.donnees.PMActuel == 0)
        {
            StartCoroutine(SeDeplacer(perso.gameObject.transform, 1));
            GestAudio.instance.JouerEffetSonore(_manquer);
            _texteDialogue.text = "Raté!";
            GererTour();
        }
        else
        {
            perso._spriteRenderer.sprite = perso.donnees._critique;
            _texteDialogue.text = MettreMajuscule(perso.donnees.nom) + " soigne!";
            yield return new WaitForSeconds(2);
            float valeur = float.PositiveInfinity;
            int index = -1;
            for (int i = 0; i < _listeTroupe.Count; i++)
            {
                if (_listeTroupe[i].donnees.PVActuel < valeur && _listeTroupe[i].donnees.PVActuel != _listeTroupe[i].donnees.PV && _listeTroupe[i].donnees.PVActuel > 0)
                {
                    index = i;
                    valeur = _listeTroupe[i].donnees.PVActuel;
                }
            }
            if (index == -1) _texteDialogue.text = "L'énergie de tous est au maximum!";
            else
            {
                perso.donnees.PMActuel -= 10;
                string persoSoigne = _listeTroupe[index].SeSoigner();
                StartCoroutine(ChangerCouleur(_listeTroupe[index]._spriteRenderer, Color.white, Color.green));
                GestAudio.instance.JouerEffetSonore(_soigner);
                _texteDialogue.text = persoSoigne;
            }
            Afficher();
            yield return new WaitForSeconds(2);
            if (index != -1) StartCoroutine(ChangerCouleur(_listeTroupe[index]._spriteRenderer, Color.green, Color.white));
            StartCoroutine(SeDeplacer(perso.gameObject.transform, 1));
            perso._spriteRenderer.sprite = perso.donnees._idle;
            GererTour();
        }
    }

    private void GererTour()
    {
        if (_ennemi.donnees.PVActuel <= 0)
        {
            StartCoroutine(ChangerCouleur(_ennemi._spriteRenderer, Color.white, new Color(0, 0, 0, 0)));
            _phase = PhaseCombat.GAGNER;
            FinirCombat();
        }
        else
        {
            _ennemi._spriteRenderer.sprite = _ennemi.donnees._idle;
            if (_phase == PhaseCombat.TOURPERSO1)
            {
                _phase = PhaseCombat.TOURPERSO2;
                StartCoroutine(TourPerso(_perso2));
            }
            else if (_phase == PhaseCombat.TOURPERSO2)
            {
                _phase = PhaseCombat.TOURPERSO3;
                StartCoroutine(TourPerso(_perso3));
            }
            else if (_phase == PhaseCombat.TOURPERSO3)
            {
                _phase = PhaseCombat.TOURPERSO4;
                StartCoroutine(TourPerso(_perso4));
            }
            else
            {
                _phase = PhaseCombat.TOURENNEMI;
                StartCoroutine(TourEnnemi());
            }
        }
    }

    private IEnumerator TourEnnemi()
    {
        _texteDialogue.text = MettreMajuscule(_ennemi.donnees.nom) + " va attaquer!";
        yield return new WaitForSeconds(2);
        int cible = Random.Range(0, 4);
        string persoMort = "";
        for (int i = 0; i < _listeTroupe.Count; i++)
        {
            if (_listeTroupe[cible].donnees.PVActuel > 0)
            {
                persoMort = _listeTroupe[cible].PrendreDegat(_ennemi.donnees.PA, false);
                break;
            }
            cible++;
            if (cible == 4) cible = 0;
        }
        if (!persoMort.Contains("Raté!")) GestAudio.instance.JouerEffetSonore(_attaquerEnnemi);
        else GestAudio.instance.JouerEffetSonore(_manquer);
        if (persoMort.Contains("Coup critique!")) _ennemi._spriteRenderer.sprite = _ennemi.donnees._critique;
        else _ennemi._spriteRenderer.sprite = _ennemi.donnees._attaque;
        _texteDialogue.text = persoMort;
        Afficher();
        yield return new WaitForSeconds(2);
        _ennemi._spriteRenderer.sprite = _ennemi.donnees._idle;
        if (_listeTroupe[cible].donnees.PVActuel > 0) _listeTroupe[cible]._spriteRenderer.sprite = _listeTroupe[cible].donnees._idle;
        else StartCoroutine(ChangerCouleur(_listeTroupe[cible]._spriteRenderer, Color.white, new Color(1, 1, 1, 0.5f)));
        if (_perso1.donnees.PVActuel == 0 && _perso2.donnees.PVActuel == 0 && _perso3.donnees.PVActuel == 0 && _perso4.donnees.PVActuel == 0)
        {
            _phase = PhaseCombat.PERDRE;
            FinirCombat();
        }
        else
        {
            _phase = PhaseCombat.TOURPERSO1;
            StartCoroutine(TourPerso(_perso1));
        }
    }

    private void FinirCombat()
    {
        if (_phase == PhaseCombat.GAGNER)
        {
            GestAudio.instance.JouerEffetSonore(_gagner);
            _texteDialogue.text = "La troupe gagne!";
            if (_combat == 3) Invoke("Gagner", 2f);
            else
            {
                _combat++;
                if (_combat == 3)
                {
                    GestAudio.instance.ChangerVolume(GestAudio.instance.volumeMusiqueRef, TypePiste.musiqueCombat);
                    GestAudio.instance.ChangerVolume(0, TypePiste.musiqueSecret);
                }
                Invoke("CommencerPartie", 2f);
            }
        }
        else
        {
            _texteDialogue.text = "La troupe perd...";
            Invoke("Perdre", 2f);
        }
    }

    private void Gagner()
    {
        _navigation.ChargerScene("Reussite");
    }

    private void Perdre()
    {
        _navigation.ChargerScene("Defaite");
    }

    private IEnumerator SeDeplacer(Transform perso, float x)
    {
        Vector3 posDebut = perso.position;
        Vector3 posFin = new Vector3(perso.position.x + x, perso.position.y, perso.position.z);
        float tempsTotal = 0.25f;
        float temps = 0;
        while (temps < tempsTotal)
        {
            perso.position = Vector3.Lerp(posDebut, posFin, temps / tempsTotal);
            temps += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ChangerCouleur(SpriteRenderer spriteRenderer, Color couleurDebut, Color couleurFin)
    {
        float tempsTotal = 0.25f;
        float temps = 0;
        while (temps < tempsTotal)
        {
            spriteRenderer.color = Color.Lerp(couleurDebut, couleurFin, temps / tempsTotal);
            temps += Time.deltaTime;
            yield return null;
        }
    }

    private void ActiverBoutons(bool estActif)
    {
        for (int i = 0; i < _boutons.Count; i++) _boutons[i].gameObject.SetActive(estActif);
    }

    private void ChangerBouton(Troupe perso)
    {
        if (perso.donnees.classe == "Combattant")
        {
            _boutons[0].gameObject.SetActive(true);
            _boutons[1].gameObject.SetActive(true);
        }
        else if (perso.donnees.classe == "Soigneur")
        {
            _boutons[2].gameObject.SetActive(true);
            _boutons[3].gameObject.SetActive(true);
        }
        else if (perso.donnees.classe == "Mage")
        {
            _boutons[2].gameObject.SetActive(true);
            _boutons[4].gameObject.SetActive(true);
        }
        else
        {
            _boutons[0].gameObject.SetActive(true);
            _boutons[5].gameObject.SetActive(true);
        }
    }

    private void Afficher()
    {
        for (int i = 0; i < _listeTroupe.Count; i++) _affichagesPerso[i].ChangerAffichage(_listeTroupe[i].donnees);
    }

    private void MelangerListe<GameObject>(List<GameObject> liste)
    {
        for (int i = 0; i < liste.Count - 1; i++)
        {
            GameObject GO = liste[i];
            int random = Random.Range(i, liste.Count);
            liste[i] = liste[random];
            liste[random] = GO;
        }
    }

    private string MettreMajuscule(string texte)
    {
        string texteInitial = texte[0].ToString();
        texteInitial = texteInitial.ToUpper();
        texte = texte.Remove(0, 1);
        texte = texte.Insert(0, texteInitial);
        return texte;
    }
}