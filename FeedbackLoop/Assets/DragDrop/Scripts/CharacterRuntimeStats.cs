using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterRuntimeStats
{
    public CharacterData baseData;

    public float Luck = 0f;
    public float BonusTime = 0f;

    public int TasksDone = 0;
    public int TasksFailed = 0;
    public int DailyTasksCount = 0;
    public int DayTaskFailed = 0;

    public float CurrentTimeToDoTask => baseData.currTimeToDoTask + BonusTime;
    public float CurrentSuccessRate => baseData.currSuccessRate + Luck;

    public void RecordSuccess()
    {
        TasksDone++;
    }

    public void RecordFailure()
    {
        TasksFailed++;
        DayTaskFailed++;
    }


    public void RecordTaskCount()
    {
        DailyTasksCount++;
    }

    public void ApplyWeeklyModifiers()
    {
        Luck += 0.2f * TasksDone;

        if (baseData.characterName == "Warlock")
            BonusTime += TasksFailed / 5f;
        else if (baseData.characterName == "Cleric")
            BonusTime += TasksFailed / 2f;

        baseData.currSuccessRate += Luck;
        baseData.currTimeToDoTask += BonusTime;
        PrintCharacterStats();
        ResetWeekData();
    }

    public void ResetDayData()
    {
        DayTaskFailed = 0;
        DailyTasksCount = 0;
    }
    public void ResetWeekData()
    {
        Luck = 0f;
        BonusTime = 0f;
        TasksDone = 0;
        TasksFailed = 0;
    }

    public void ResetWeeklyProgressionOnNewGame()
    {
        baseData.currSuccessRate =baseData.baseLuck;
        baseData.currTimeToDoTask = baseData.baseTime;
    }

    public void PrintCharacterStats()
    {
        string characterLog = $"[{baseData.characterName} Stats]\n" +
                            $"→ Luck: {Luck}\n" +
                            $"→ Bonus Time: {BonusTime}\n" +
                            $"→ Tasks Done: {TasksDone}\n" +
                            $"→ Tasks Failed: {TasksFailed}\n" +
                            $"→ Current Success Rate: {CurrentSuccessRate}\n" +
                            $"→ Current Time To Do Task: {CurrentTimeToDoTask}";

        

        Debug.Log(characterLog);
    }

    private int GetSuccesses()
    {
        return TasksDone;
    }

    private int GetFailures() { return TasksFailed; }
}
