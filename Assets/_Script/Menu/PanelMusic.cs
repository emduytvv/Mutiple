using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PanelMusic : Singleton<PanelMusic>
{
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Button _btnBack;
    [SerializeField] private CanvasGroup _canvasGroup;
    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSliderMusic();
        LoadSliderSFX();
        LoadBtnBack();
        LoadCanvasGroup();
    }

    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
        Debug.LogWarning(transform.name + ": LoadCanvasGroup()", gameObject);
    }


    protected override void Start()
    {
        base.Start();
        _btnBack.onClick.AddListener(Back);
        _sliderMusic.onValueChanged.AddListener(OnMusicChanged);
        _sliderSFX.onValueChanged.AddListener(OnSFXChanged);
        LoadSavedVolumes();
    }

    private void LoadSliderMusic()
    {
        if (_sliderMusic != null) return;
        _sliderMusic = transform.Find("SliderMusic").GetComponent<Slider>();
        Debug.LogWarning(transform.name + ": LoadSliderMusic()", gameObject);
    }

    private void LoadSliderSFX()
    {
        if (_sliderSFX != null) return;
        _sliderSFX = transform.Find("SliderSFX").GetComponent<Slider>();
        Debug.LogWarning(transform.name + ": LoadSliderSFX()", gameObject);
    }

    private void LoadBtnBack()
    {
        if (_btnBack != null) return;
        _btnBack = transform.Find("Back").GetComponent<Button>();
        Debug.LogWarning(transform.name + ": LoadBtnBack()", gameObject);
    }

    private void Back()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        if (transform.parent.parent.Find("MainMenu") != null) transform.parent.parent.Find("MainMenu").gameObject.SetActive(true);
    }
    private void LoadSavedVolumes()
    {
        _sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", _sliderMusic.value);
        _sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", _sliderSFX.value);
        SetMusicVolume(_sliderMusic.value);
        SetSFXVolume(_sliderSFX.value);
    }
    private void OnMusicChanged(float value) => SetMusicVolume(value);
    private void OnSFXChanged(float value) => SetSFXVolume(value);

    private void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
