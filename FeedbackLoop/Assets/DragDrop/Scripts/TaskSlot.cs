using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TaskSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] CanvasGroup canvasGroup;
    private DragAndDrop assignedCharacter;
    private Coroutine taskCoroutine;
    private bool taskStarted = false;
    private bool isActive =true;
    public bool IsRunning => taskCoroutine != null;
    [SerializeField] private Image targetImage;

    public void SetAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = Mathf.Clamp01(alpha); // Ensure value is between 0 and 1
        targetImage.color = color;
    }

    public void SetActiveState(bool active)
    {
        isActive = active;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = active ? 1f : 0.4f;           // make it look faded when inactive
            this.gameObject.SetActive(active);         // only interactable if active
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
        float duration = data.currTimeToDoTask;
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
        bool success = roll <= data.currSuccessRate;
        int points = success ? data.pointsPerSuccess : data.pointsPerFailure;
        GameController.Instance.AddScore(points);

        if (data.characterName == "Warlock")
        {
            ProgressionManager.Instance.warlockStats.RecordTaskCount();
            if (success)
                ProgressionManager.Instance.warlockStats.RecordSuccess();
            else
                ProgressionManager.Instance.warlockStats.RecordFailure();
        }
        else if (data.characterName == "Cleric")
        {
            ProgressionManager.Instance.clericStats.RecordTaskCount();
            if (success)
                ProgressionManager.Instance.clericStats.RecordSuccess();
            else
                ProgressionManager.Instance.clericStats.RecordFailure();
        }


        SetAlpha(0.5f);
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