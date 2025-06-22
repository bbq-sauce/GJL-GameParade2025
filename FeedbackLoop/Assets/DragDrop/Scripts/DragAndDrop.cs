using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image taskProgressBar;
    public CharacterData characterData;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        ShowProgressBar(false);
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void LockDragging(bool value)
    {
        canvasGroup.blocksRaycasts = !value;
        canvasGroup.interactable = !value;
    }

    public void SetProgress(float value)
    {
        if (taskProgressBar != null)
            taskProgressBar.fillAmount = Mathf.Clamp01(value);
    }


    public void ShowProgressBar(bool show)
    {
        if (taskProgressBar != null)
            taskProgressBar.gameObject.SetActive(show);
    }
}
