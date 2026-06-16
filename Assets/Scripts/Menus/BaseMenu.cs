using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenu : MonoBehaviour
{
    public Button[] allButtons;
    [HideInInspector] public MenuStates state;

    protected MenuController context;

    public virtual void Initialize(MenuController context)
    {
        this.context = context;
        allButtons = GetComponentsInChildren<Button>(true);
    }

    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void JumpTo(MenuStates newState)
    {
        context.JumpTo(newState);
    }
}

public enum MenuStates
{
    MainMenu,
    PauseMenu,
    SettingsMenu,
    CreditsMenu
}