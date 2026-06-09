using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public BaseMenu[] allMenus;
    public MenuStates initState = MenuStates.MainMenu;

    public BaseMenu currentMenu => _currentMenu;
    private BaseMenu _currentMenu;

    private Dictionary<MenuStates, BaseMenu> menuDictionary = new Dictionary<MenuStates, BaseMenu>();
    private Stack<MenuStates> menuHistory = new Stack<MenuStates>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        }

        JumpTo(initState);
    }

    public void JumpBack()
    {
        if (menuHistory.Count <= 0)
        {
            Debug.LogWarning("No previous menu to jump back to.");
            return;
        }

        menuHistory.Pop(); // Remove current menu from history
        JumpTo(menuHistory.Peek(), true);
    }

    public void JumpTo(MenuStates newState, bool isBackNavigation = false)
    {
        if (!menuDictionary.ContainsKey(newState))
        {
            Debug.LogError($"Menu state {newState} does not exist in the menu dictionary.");
            return;
        }
        if (_currentMenu == menuDictionary[newState])
        {
            Debug.LogWarning($"Already on menu state {newState}. No transition needed.");
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
            if (menuHistory.Count > 0 && menuHistory.Contains(newState))
            {
                List<MenuStates> tempStack = new List<MenuStates>();
                while (menuHistory.Peek() != newState)
                {
                    tempStack.Add(menuHistory.Pop());
                }
                menuHistory.Pop(); // Remove the duplicate state
                for (int i = tempStack.Count - 1; i >= 0; i--)
                {
                    menuHistory.Push(tempStack[i]);
                }
            }
            menuHistory.Push(newState);
        }
    }
}