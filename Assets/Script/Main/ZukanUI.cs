using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZukanUI : MonoBehaviour
{
    [SerializeField] GameObject GREEN;
    [SerializeField] GameObject YELLOW;
    [SerializeField] Image MIGI;
    [SerializeField] Image HIDARI;

    [SerializeField] Sprite aru;
    [SerializeField] Sprite nai;

    void Start()
    {
        MIDORI();
    }

    public void MIDORI()
    {
        if (HIDARI.sprite == aru)
        {
            YELLOW.SetActive(false);
            GREEN.SetActive(true);
            HIDARI.sprite = nai;
            MIGI.sprite = aru;
        }
    }

    public void KIIRO()
    {
        if (MIGI.sprite == aru)
        {
            GREEN.SetActive(false);
            YELLOW.SetActive(true);
            HIDARI.sprite = aru;
            MIGI.sprite = nai;
        }
    }
}
