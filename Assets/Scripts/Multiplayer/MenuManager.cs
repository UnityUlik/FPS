using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Menu[] _menus;

    void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (var menuElem in _menus)
        {
            if (menuElem.menuName == menuName)
            {
                OpenMenu(menuElem);
            }
            else if (menuElem.isOpen)
            {
                CloseMenu(menuElem);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        foreach (var menuElem in _menus) 
        {
            if (menuElem.isOpen)
            {
                CloseMenu(menuElem);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
