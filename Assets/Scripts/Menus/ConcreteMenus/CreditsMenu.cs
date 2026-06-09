using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        state = MenuStates.CreditsMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in CreditsMenu. Please ensure there are Button components in the children of this GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Settings")) button.onClick.AddListener(() => context.JumpTo(MenuStates.SettingsMenu));
            if (button.name.Contains("Back")) button.onClick.AddListener(() => context.JumpBack());
        }
    }
}