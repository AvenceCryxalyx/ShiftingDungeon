using System.Collections.Generic;
using UnityEngine;

public enum UIViewType
{
    Main,
    Settings,
    Inventory,
    BuffStore,
    MechantStore,
    GameMessage,
}

public class UIManager : SimpleSingleton<UIManager>
{
    private Dictionary<UIViewType,UIVIew> views = new Dictionary<UIViewType, UIVIew>();
    private Dictionary<string, UIElement> importantElements = new Dictionary<string, UIElement>();
    private PlayerInputAction action;
    public static void Register(UIVIew menu)
    {
        if (!instance.views.ContainsKey(menu.Type))
            instance.views.Add(menu.Type, menu);
        else
            Debug.Log(string.Format("Another instance of menu type {0} tried to register", menu.Type.ToString()));
    }

    public static void DeRegister(UIVIew menu)
    {
        if(instance.views.ContainsKey(menu.Type))
            instance.views.Remove(menu.Type);
        else
            Debug.Log(string.Format("Another instance of menu type {0} tried to deregister", menu.Type.ToString()));
    }

    public static void Register(string id, UIElement element)
    {
        if (!instance.importantElements.ContainsKey(id))
            instance.importantElements.Add(id, element);
        else
            Debug.Log(string.Format("Another instance of element id {0} tried to register", id));
    }

    public static void DeRegister(string id, UIElement element)
    {
        if (instance.importantElements.ContainsKey(id))
            instance.importantElements.Remove(id);
        else
            Debug.Log(string.Format("Another instance of element id {0} tried to deregister", id));
    }

    public void ShowView(UIViewType type)
    {
        UIVIew ui;
        views.TryGetValue(type, out ui);
        if (ui == null)
        {
            Debug.LogError(string.Format("Menu: {0} is null", type.ToString()));
            return;
        }
        if(!ui.IsShown)
        {
            ui.Show();
        }
    }

    public void HideView(UIViewType type)
    {
        UIVIew menu;
        views.TryGetValue(type, out menu);
        if (menu == null)
        {
            Debug.LogError(string.Format("Menu: {0} is null", type.ToString()));
            return;
        }
        if (menu.IsShown)
        {
            menu.Hide();
        }
    }
}
