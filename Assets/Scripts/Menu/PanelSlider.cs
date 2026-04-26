using UnityEngine;
using System.Collections;

public class PanelSlider : MonoBehaviour
{
    public enum SlideDirection
    {
        LeftToRight,
        RightToLeft
    }

    public float duration = 0.5f;

    private float screenWidth;

    private void Awake()
    {
        screenWidth = Screen.width;
    }

    public void Slide(RectTransform fromPanel, RectTransform toPanel, SlideDirection direction = SlideDirection.RightToLeft)
    {
        if (fromPanel == null || toPanel == null)
        {
            Debug.LogError("Panels cannot be null.");
            return;
        }

        if (fromPanel == toPanel)
        {
            Debug.LogWarning("From and To panels are the same. No sliding will occur.");
            return;
        }

        StartCoroutine(SlideCoroutine(fromPanel, toPanel, direction));
    }

    private IEnumerator SlideCoroutine(RectTransform fromPanel, RectTransform toPanel, SlideDirection direction)
    {
        float time = 0f;

        Vector2 fromStart = fromPanel.anchoredPosition;
        Vector2 fromEnd;
        Vector2 toStart;
        Vector2 toEnd = Vector2.zero;

        if (direction == SlideDirection.RightToLeft)
        {
            fromEnd = fromStart + new Vector2(-screenWidth, 0);
            toStart = new Vector2(screenWidth, 0);
        }
        else
        {
            fromEnd = fromStart + new Vector2(screenWidth, 0);
            toStart = new Vector2(-screenWidth, 0);
        }

        toPanel.anchoredPosition = toStart;
        toPanel.gameObject.SetActive(true);

        while (time < duration)
        {
            float t = time / duration;

            fromPanel.anchoredPosition = Vector2.Lerp(fromStart, fromEnd, t);
            toPanel.anchoredPosition = Vector2.Lerp(toStart, toEnd, t);

            time += Time.deltaTime;
            yield return null;
        }

        fromPanel.anchoredPosition = fromEnd;
        toPanel.anchoredPosition = toEnd;

        fromPanel.gameObject.SetActive(false);
    }
}