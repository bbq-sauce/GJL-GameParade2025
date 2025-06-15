using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class TaskSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] CanvasGroup canvasGroup;
    private DragAndDrop assignedCharacter;
    private Coroutine taskCoroutine;
    private bool taskStarted = false;
    private bool isActive =true;
    public bool IsRunning => taskCoroutine != null;

    public void SetActiveState(bool active)
    {
        isActive = active;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = active ? 1f : 0.4f;           // make it look faded when inactive
            canvasGroup.blocksRaycasts = active;              // only interactable if active
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isActive || taskStarted || assignedCharacter != null)
            return;

        if (eventData.pointerDrag != null)
        {
            assignedCharacter = eventData.pointerDrag.GetComponent<DragAndDrop>();
            assignedCharacter.transform.SetParent(transform);
            assignedCharacter.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            StartTask(assignedCharacter.characterData);

            // Optional: tell GameController what task was assigned
            GameController.Instance.SetTaskAssignment(assignedCharacter.characterData.characterName, gameObject.name);
        }
    }


    public void StartTask(CharacterData data)
    {
        if (taskCoroutine != null) StopCoroutine(taskCoroutine);
        assignedCharacter.LockDragging(true);
        taskCoroutine = StartCoroutine(RunTask(data));
        taskStarted = true;
    }


    private IEnumerator RunTask(CharacterData data)
    {
        float duration = data.timeToDoTask;
        float elapsed = 0f;

        assignedCharacter.ShowProgressBar(true);
        assignedCharacter.LockDragging(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            assignedCharacter.SetProgress(1f - progress); // fill goes from full to empty
            yield return null;
        }

        assignedCharacter.ShowProgressBar(false);
        assignedCharacter.LockDragging(false);

        float roll = Random.Range(0f, 100f);
        bool success = roll <= data.successRate;
        int points = success ? data.pointsPerSuccess : data.pointsPerFailure;
        GameController.Instance.AddScore(points);

        assignedCharacter.LockDragging(false);
        taskCoroutine = null;
        taskStarted = false;
    }


    public void PenalizeUnassigned()
    {
        if (isActive && !taskStarted && assignedCharacter == null)
        {
            GameController.Instance.AddScore(-30);
        }
    }
    public void ResetSlot()
    {
        if (assignedCharacter != null)
        {
            assignedCharacter.ShowProgressBar(false);
            assignedCharacter.LockDragging(false);
        }

        assignedCharacter = null;
        taskStarted = false;

        if (taskCoroutine != null)
        {
            StopCoroutine(taskCoroutine);
            taskCoroutine = null;
        }
    }

}