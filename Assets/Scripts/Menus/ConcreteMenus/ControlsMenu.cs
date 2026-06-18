using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        state = MenuStates.ControlsMenu;

        foreach (Button button in allButtons)
        {
            if (button == null) continue;

            if (button.name.Contains("Back"))
                button.onClick.AddListener(() => context.JumpBack());

            if (button.name.Contains("Settings"))
                button.onClick.AddListener(() => context.JumpTo(MenuStates.SettingsMenu));
        }
    }
}