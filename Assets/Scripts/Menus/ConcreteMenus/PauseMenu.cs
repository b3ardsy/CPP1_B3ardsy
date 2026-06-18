using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        state = MenuStates.PauseMenu;

        foreach (Button button in allButtons)
        {
            if (button == null) continue;

            if (button.name.Contains("Resume"))
                button.onClick.AddListener(ResumeGame);

            if (button.name.Contains("Settings"))
                button.onClick.AddListener(() => context.JumpTo(MenuStates.SettingsMenu));

            if (button.name.Contains("Controls"))
                button.onClick.AddListener(() => context.JumpTo(MenuStates.ControlsMenu));

            if (button.name.Contains("Quit"))
                button.onClick.AddListener(QuitGame);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        context.HideCurrentMenu();
    }
}