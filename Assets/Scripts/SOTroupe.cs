using UnityEngine;

[CreateAssetMenu(fileName = "Troupe", menuName = "Troupe", order = 1)]
public class SOTroupe : ScriptableObject
{
    [SerializeField] public Sprite _idle;
    [SerializeField] public Sprite _attaque;
    [SerializeField] public Sprite _critique;
    [SerializeField] public Sprite _hurt;
    [SerializeField] public Sprite _sprite;
    [SerializeField] string _nom;
    [SerializeField] string _classe;
    [SerializeField] int _PA;
    [SerializeField] int _PM;
    [SerializeField] int _PMActuel;
    [SerializeField] int _PV;
    [SerializeField] int _PVActuel;
    public string nom
    {
        get { return _nom; }
        set { _nom = value; }
    }
    public string classe
    {
        get { return _classe; }
        set { _classe = value; }
    }
    public int PA
    {
        get { return _PA; }
        set { _PA = value; }
    }
    public int PM
    {
        get { return _PM; }
        set { _PM = value; }
    }
    public int PMActuel
    {
        get { return _PMActuel; }
        set { _PMActuel = Mathf.Clamp(value, 0, _PM); }
    }
    public int PV
    {
        get { return _PV; }
        set { _PV = value; }
    }
    public int PVActuel
    {
        get { return _PVActuel; }
        set { _PVActuel = Mathf.Clamp(value, 0, _PV); }
    }
}