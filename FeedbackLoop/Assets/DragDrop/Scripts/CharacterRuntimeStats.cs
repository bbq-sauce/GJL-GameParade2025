using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class CharacterRuntimeStats
{
    public CharacterData baseData;

    public float Luck = 0f;
    public float BonusTime = 0f;

    public int TasksDone = 0;
    public int TasksFailed = 0;

    public float CurrentTimeToDoTask => baseData.timeToDoTask + BonusTime;
    public float CurrentSuccessRate => baseData.successRate + Luck;

    public void RecordSuccess()
    {
        TasksDone++;
    }

    public void RecordFailure()
    {
        TasksFailed++;
    }

    public void ApplyWeeklyModifiers()
    {
        Luck += 0.2f * TasksDone;

        if (baseData.characterName == "Warlock")
            BonusTime += TasksFailed / 5f;
        else if (baseData.characterName == "Cleric")
            BonusTime += TasksFailed / 2f;

        baseData.successRate += Luck;
        baseData.timeToDoTask += BonusTime;
        PrintCharacterStats();
        ResetWeekData();
    }

    public void ResetWeekData()
    {
        TasksDone = 0;
        TasksFailed = 0;
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
