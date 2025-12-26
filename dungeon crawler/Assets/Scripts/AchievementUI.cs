using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    public static AchievementUI Instance;

    [Header("UI Components")]
    public GameObject notificationPanel;
    public Text titleText;
    public Text descriptionText;
    public Image iconImage;

    [Header("Settings")]
    public float displayDuration = 3f;
    private bool isDisplaying = false;

    void Awake()
    {
        Instance = this;
        notificationPanel.SetActive(false);
    }

    public void ShowAchievement(string title, string description, Sprite icon = null)
    {
        if (isDisplaying) return; // Optional: Could use a queue system here
        StartCoroutine(AnimateNotification(title, description, icon));
    }

    IEnumerator AnimateNotification(string title, string description, Sprite icon)
    {
        isDisplaying = true;
        
        titleText.text = title;
        descriptionText.text = description;
        if (icon != null) iconImage.sprite = icon;

        notificationPanel.SetActive(true);
        
        // 1. Play Achievement Sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup); // Or a specific 'Unlock' sound

        // 2. Wait
        yield return new WaitForSeconds(displayDuration);

        // 3. Hide
        notificationPanel.SetActive(false);
        isDisplaying = false;
    }
}