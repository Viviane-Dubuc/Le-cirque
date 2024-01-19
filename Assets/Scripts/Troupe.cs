using UnityEngine;

public class Troupe : MonoBehaviour
{
    [SerializeField] SONavigation _donneesNavigation;
    [SerializeField] SOTroupe _donnees;
    public SpriteRenderer _spriteRenderer;
    public SOTroupe donnees
    {
        get { return _donnees; }
        set { _donnees = value; }
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.white;
        _spriteRenderer.sprite = _donnees._idle;
        _donneesNavigation.evenementInitialiser.AddListener(Initialiser);
        Initialiser();
    }

    public string PrendreDegat(int degat, bool estFort)
    {
        bool estReussi = estFort ? EtreReussi(50) : EtreReussi(10);
        if (!estReussi) return "Raté!";
        int coupCritique = estFort ? 2 : 1;
        int random = Random.Range(-5, 5);
        int attaque = degat * coupCritique + random;
        donnees.PVActuel -= attaque;
        _spriteRenderer.sprite = _donnees._hurt;
        if (donnees.PVActuel <= 0)
        {
            donnees.PVActuel = 0;
            return MettreMajuscule(donnees.nom) + " ne peut plus combattre!";
        }
        else if (donnees.classe != "Ennemi" && random > 0) return "Coup critique! " + MettreMajuscule(donnees.nom) + " perd " + attaque + " dégâts!";
        else if (donnees.classe != "Ennemi") return MettreMajuscule(donnees.nom) + " perd " + attaque + " dégâts!";
        else return "L'attaque fait " + attaque + " dégâts!";
    }

    public string SeSoigner()
    {
        donnees.PVActuel = donnees.PV;
        return MettreMajuscule(donnees.nom) + " regagne de l'énergie!";
    }

    private void Initialiser()
    {
        _donnees.PVActuel = _donnees.PV;
        _donnees.PMActuel = _donnees.PM;
    }

    private bool EtreReussi(int chance)
    {
        int random = Random.Range(0, 100);
        return random > chance;
    }

    private string MettreMajuscule(string texte)
    {
        string texteInitial = texte[0].ToString();
        texteInitial = texteInitial.ToUpper();
        texte = texte.Remove(0, 1);
        texte = texte.Insert(0, texteInitial);
        return texte;
    }

    void OnApplicationQuit()
    {
        _donneesNavigation.evenementInitialiser.RemoveAllListeners();
    }
}