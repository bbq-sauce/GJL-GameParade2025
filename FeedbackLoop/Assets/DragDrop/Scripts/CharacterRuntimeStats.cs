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

        ResetWeekData();
    }

    public void ResetWeekData()
    {
        TasksDone = 0;
        TasksFailed = 0;
    }
}
