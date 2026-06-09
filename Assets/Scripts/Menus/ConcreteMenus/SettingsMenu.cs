using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SettingsMenu : BaseMenu
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Text masterVolText;
    [SerializeField] private TMP_Text musicVolText;
    [SerializeField] private TMP_Text sfxVolText;

    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider sfxVolSlider;



    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        state = MenuStates.SettingsMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in SettingsMenu. Please ensure there are Button components in the children of this GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Credits")) button.onClick.AddListener(() => context.JumpTo(MenuStates.CreditsMenu));
            if (button.name.Contains("Back")) button.onClick.AddListener(() => context.JumpBack());
        }

        if (masterVolSlider && masterVolText)
        {
            SetupSliderInformation(masterVolSlider, masterVolText, "MasterVol");
            OnSliderValueChanged(masterVolSlider.value, masterVolSlider, masterVolText, "MasterVol");
        }

        if (musicVolSlider && musicVolText)
        {
            SetupSliderInformation(musicVolSlider, musicVolText, "MusicVol");
            OnSliderValueChanged(musicVolSlider.value, musicVolSlider, musicVolText, "MusicVol");
        }

        if (sfxVolSlider && sfxVolText)
        {
            SetupSliderInformation(sfxVolSlider, sfxVolText, "SFXVol");
            OnSliderValueChanged(sfxVolSlider.value, sfxVolSlider, sfxVolText, "SFXVol");
        }
    }

    private void SetupSliderInformation(Slider slider, TMP_Text text, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, text, parameterName));
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text text, string parameterName)
    {
        if (value == 0)
        {
            value = -80f;
            text.text = "0%";
        }
        else
        {
            value = Mathf.Log10(value) * 20;
            text.text = $"{Mathf.RoundToInt(slider.value * 100)}%";
        }

        audioMixer.SetFloat(parameterName, value);
    }
}