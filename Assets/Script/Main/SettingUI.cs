using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] Image Black;
    [SerializeField] Image BACK;
    [SerializeField] Image FX_ON;
    [SerializeField] Image FX_OFF;
    [SerializeField] Image BGM_ON;
    [SerializeField] Image BGM_OFF;
    [SerializeField] Image EXIT;

    [SerializeField] Sprite SelectON;
    [SerializeField] Sprite SelectOFF;
    [SerializeField] Sprite not_SelectON;
    [SerializeField] Sprite not_SelectOFF;

    void Start()
    {
        CloseSETTING();

        if (SoundManager.Instance != null)
        {
            if (SoundManager.Instance.BGMplaying) OnBGM();
            else OffBGM();
            if (SoundManager.Instance.SEplaying) OnFX();
            else OffFX();
        }
    }

    public void OpenSETTING()
    {
        Black.gameObject.SetActive(true);
    }

    public void CloseSETTING()
    {
        Black.gameObject.SetActive(false);
    }

    public void OnFX()
    {
        FX_ON.sprite = SelectON;
        FX_OFF.sprite = not_SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OnSE();
    }

    public void OffFX()
    {
        FX_ON.sprite = not_SelectON;
        FX_OFF.sprite = SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OffSE();
    }

    public void OnBGM()
    {
        BGM_ON.sprite = SelectON;
        BGM_OFF.sprite = not_SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OnBGM();
    }

    public void OffBGM()
    {
        BGM_ON.sprite = not_SelectON;
        BGM_OFF.sprite = SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OffBGM();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
