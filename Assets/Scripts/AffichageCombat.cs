using TMPro;
using UnityEngine;

public class AffichageCombat : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _texteVie;
    [SerializeField] TextMeshProUGUI _texteMagie;

    public void ChangerAffichage(SOTroupe troupe)
    {
        _texteVie.text = troupe.PVActuel.ToString();
        _texteMagie.text = troupe.PMActuel.ToString();
    }
}