using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        state = MenuStates.MainMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in MainMenu. Please ensure there are Button components in the children of this GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Start")) button.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
            if (button.name.Contains("Settings")) button.onClick.AddListener(() => context.JumpTo(MenuStates.SettingsMenu));
            if (button.name.Contains("Credits")) button.onClick.AddListener(() => context.JumpTo(MenuStates.CreditsMenu));
            if (button.name.Contains("Quit")) button.onClick.AddListener(QuitGame);
        }
    }
}