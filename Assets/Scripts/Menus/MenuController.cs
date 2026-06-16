using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public BaseMenu[] allMenus;
    public MenuStates initState = MenuStates.MainMenu;

    [SerializeField] private bool startHidden = false;

    public BaseMenu currentMenu => _currentMenu;
    private BaseMenu _currentMenu;

    private Dictionary<MenuStates, BaseMenu> menuDictionary = new Dictionary<MenuStates, BaseMenu>();
    private Stack<MenuStates> menuHistory = new Stack<MenuStates>();

    void Start()
    {
        if (allMenus.Length == 0)
        {
            allMenus = GetComponentsInChildren<BaseMenu>(true);
        }

        foreach (BaseMenu menu in allMenus)
        {
            if (menu == null) continue;

            menu.Initialize(this);

            if (!menuDictionary.ContainsKey(menu.state))
            {
                menuDictionary.Add(menu.state, menu);
            }
            else
            {
                Debug.LogWarning($"Duplicate menu state detected: {menu.state}. Only the first one will be used.");
            }

            menu.Exit();
        }

        if (!startHidden)
        {
            JumpTo(initState);
        }
    }

    public void JumpTo(MenuStates newState, bool isBackNavigation = false)
    {
        if (!menuDictionary.ContainsKey(newState))
        {
            Debug.LogError($"Menu state {newState} does not exist.");
            return;
        }

        if (_currentMenu != null)
        {
            _currentMenu.Exit();
        }

        _currentMenu = menuDictionary[newState];
        _currentMenu.Enter();

        if (!isBackNavigation)
        {
            menuHistory.Push(newState);
        }
    }

    public void JumpBack()
    {
        if (menuHistory.Count <= 1)
        {
            Debug.LogWarning("No previous menu to jump back to.");
            return;
        }

        menuHistory.Pop();

        MenuStates previousState = menuHistory.Peek();
        JumpTo(previousState, true);
    }

    public void HideCurrentMenu()
    {
        if (_currentMenu != null)
        {
            _currentMenu.Exit();
            _currentMenu = null;
        }

        menuHistory.Clear();
    }
}