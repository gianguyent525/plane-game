using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class UIWeatherDisplay : MonoBehaviour
{
    public Image icon;
    public TMP_Text description;

    public void UpdateUI(WeatherData data)
    {
        if (description == null)
        {
            Debug.LogError("Description TMP_Text component is not assigned!");
        }
        else
        {
            description.text = data.description;
            description.gameObject.SetActive(true);
        }
        
        if (icon == null)
        {
            Debug.LogError("Icon Image component is not assigned!");
        }
        else
        {
            LoadIcon(data.iconCode);
        }
    }

    void LoadIcon(string iconCode)
    {
        string path = $"Weather/WeatherIcons/{iconCode}";
        Sprite iconSprite = Resources.Load<Sprite>(path);
        
        if (iconSprite != null && icon != null)
        {
            icon.sprite = iconSprite;
            icon.enabled = true;
            icon.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Could not find weather icon at: {path}");
            if (icon != null)
                icon.gameObject.SetActive(false);
        }
    }

    public void HideUI()
    {
        if (icon != null)
            icon.gameObject.SetActive(false);
        if (description != null)
            description.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        if (icon != null)
            icon.gameObject.SetActive(true);
        if (description != null)
            description.gameObject.SetActive(true);
    }
}
