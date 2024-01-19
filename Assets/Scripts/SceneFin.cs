using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFin : MonoBehaviour
{
    [SerializeField] List<Image> _imagesPersos = new List<Image>();
    [SerializeField] SONavigation _navigation;

    void Start()
    {
        for (int i = 0; i < _imagesPersos.Count; i++) _imagesPersos[i].sprite = _navigation._listeTroupe[i].donnees._sprite;
    }
}