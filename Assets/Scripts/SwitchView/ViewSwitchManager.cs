using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSwitchManager : Singleton<ViewSwitchManager>
{
    [SerializeField] View[] views;

    public void OpenViewByName(string menuName)
    {
        for (int i = 0; i < views.Length; i++)
        {
            if (views[i].viewName == menuName)
            {
                OpenView(views[i]);
            }
            else if (views[i].open)
            {
                CloseView(views[i]);
            }
        }
    }

    public void OpenView(View menu)
    {
        for (int i = 0; i < views.Length; i++)
        {
            if (views[i].open)
            {
                CloseView(views[i]);
            }
        }
        menu.Open();
    }

    public void CloseView(View menu)
    {
        menu.Close();
    }
}
