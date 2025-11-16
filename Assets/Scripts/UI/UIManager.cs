using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define this with whatever UI Views you will have.
/// </summary>
public enum UIViewType
{
    Main,
    Settings,
    Inventory,
    BuffStore,
    MechantStore,
    GameMessage,
    GatherConfirm,
    GatherResult,
}

public class UIManager : SimpleSingleton<UIManager>
{
    private Dictionary<UIViewType,UIVIew> views = new Dictionary<UIViewType, UIVIew>();
    private Dictionary<string, UIElement> elements = new Dictionary<string, UIElement>();
    private PlayerInputAction action;
    public static void Register(UIVIew menu)
    {
        if (!instance.views.ContainsKey(menu.Type))
            instance.views.Add(menu.Type, menu);
        else
            Debug.Log($"Another instance of menu type {menu.Type.ToString()} tried to register");
    }

    public static void DeRegister(UIVIew menu)
    {
        if(instance.views.ContainsKey(menu.Type))
            instance.views.Remove(menu.Type);
        else
            Debug.Log($"Another instance of menu type {menu.Type.ToString()} tried to deregister");
    }

    public static void Register(string id, UIElement element)
    {
        if (!instance.elements.ContainsKey(id))
            instance.elements.Add(id, element);
        else
            Debug.Log($"Another instance of element id {id} tried to register");
    }

    public static void Deregister(string id)
    {
        if (instance.elements.ContainsKey(id))
            instance.elements.Remove(id);
        else
            Debug.Log($"Another instance of element id {id} tried to deregister");
    }

    public UIVIew ShowView(UIViewType type)
    {
        UIVIew view;
        views.TryGetValue(type, out view);
        if (view == null)
        {
            Debug.LogError($"Menu: {type.ToString()} is null");
            return null;
        }
        if(!view.IsShown)
        {
            view.Show();
        }
        return view;
    }

    public UIVIew HideView(UIViewType type)
    {
        UIVIew view;
        views.TryGetValue(type, out view);
        if (view == null)
        {
            Debug.LogError($"Menu: {type.ToString()} is null");
            return null;
        }
        if (view.IsShown)
        {
            view.Hide();
        }
        return view;
    }
}
