using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class DropdownAnimator : MonoBehaviour
{
    [Header("References")]
    public RectTransform scrollView;     // Scroll View container
    public Button mainButton;            // Main toggle button
    public TMP_Text selectedTextLabel;        // Text showing selected option
    private TMP_Text currentSelectedOption;

    [Header("Height Settings")]
    public float collapsedHeight = 100f; // Starting height
    public float expandedHeight = 400f;  // Final height

    [Header("Animation")]
    public float duration = 0.25f;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isExpanded = false;
    private Coroutine runningAnimation;

    void Start()
    {
        SetHeight(collapsedHeight);
        if (PlayerPrefs.GetString("language", "en") == "en")
        {
            selectedTextLabel.text = "ENGLISH";
        }
        else if (PlayerPrefs.GetString("language", "en") == "pt-BR")
        {
            selectedTextLabel.text = "PORTUGUESE";
        }
        else if (PlayerPrefs.GetString("language", "en") == "ru")
        {
            selectedTextLabel.text = "RUSSIAN";
        }
        else if (PlayerPrefs.GetString("language", "en") == "sp")
        {
            selectedTextLabel.text = "ESPANOL";
        }
        else if (PlayerPrefs.GetString("language", "en") == "fr")
        {
            selectedTextLabel.text = "FRENCH";
        }
        else
        {
            selectedTextLabel.text = "English";
        }

        if (mainButton != null)
            mainButton.onClick.AddListener(ToggleDropdown);
    }

    public void ToggleDropdown()
    {
        float targetHeight = isExpanded ? collapsedHeight : expandedHeight;
        Debug.Log("Toggling Dropdown");

        if (runningAnimation != null)
            StopCoroutine(runningAnimation);

        runningAnimation = StartCoroutine(AnimateHeight(targetHeight));
        isExpanded = !isExpanded;
    }

    public void CollapseDropdown()
    {
        if (runningAnimation != null)
            StopCoroutine(runningAnimation);

        runningAnimation = StartCoroutine(AnimateHeight(collapsedHeight));
        isExpanded = false;

        // ✅ Update label when collapsing
        UpdateSelectedLabel();
    }

    IEnumerator AnimateHeight(float targetHeight)
    {
        float startHeight = scrollView.sizeDelta.y;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float easedT = easeCurve.Evaluate(t);

            float newHeight = Mathf.Lerp(startHeight, targetHeight, easedT);
            SetHeight(newHeight);

            yield return null;
        }

        SetHeight(targetHeight);
    }

    void SetHeight(float height)
    {
        Vector2 size = scrollView.sizeDelta;
        size.y = height;
        scrollView.sizeDelta = size;
    }

    // ✅ Called by option buttons
    public void OnOptionSelected(GameObject clickedObject)
    {
        currentSelectedOption = clickedObject.GetComponent<TMP_Text>();
        PlayerPrefs.SetString("language", currentSelectedOption.text);
        CollapseDropdown();
    }

    void UpdateSelectedLabel()
    {
        if (currentSelectedOption != null && selectedTextLabel != null)
        {
            selectedTextLabel.text = currentSelectedOption.text;
        }
    }
}