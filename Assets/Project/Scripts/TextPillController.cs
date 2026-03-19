using System.Collections;
using UnityEngine;

public class TextPillController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.35f;
    [SerializeField] private float hiddenYOffset = 200f;

    [Header("Timing")]
    [SerializeField] private float secondsShowing = 2f;

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        shownPosition = rectTransform.anchoredPosition;
        hiddenPosition = shownPosition + Vector2.up * hiddenYOffset;

        SetHiddenImmediate();
    }

    public void Show()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        yield return Animate(
            fromPosition: hiddenPosition,
            toPosition: shownPosition,
            fromAlpha: 0f,
            toAlpha: 1f,
            duration: animationDuration
        );

        yield return new WaitForSeconds(secondsShowing);

        yield return Animate(
            fromPosition: shownPosition,
            toPosition: hiddenPosition,
            fromAlpha: 1f,
            toAlpha: 0f,
            duration: animationDuration
        );

        currentRoutine = null;
    }

    private IEnumerator Animate(
    Vector2 fromPosition,
    Vector2 toPosition,
    float fromAlpha,
    float toAlpha,
    float duration)
    {
        float elapsed = 0f;

        rectTransform.anchoredPosition = fromPosition;
        canvasGroup.alpha = fromAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            rectTransform.anchoredPosition = Vector2.Lerp(fromPosition, toPosition, easedT);
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, easedT);

            yield return null;
        }

        rectTransform.anchoredPosition = toPosition;
        canvasGroup.alpha = toAlpha;
    }

    private void SetHiddenImmediate()
    {
        rectTransform.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 0f;
    }
}