using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "DragDrop/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public float timeToDoTask; // in seconds
    public float successRate; // 0-100
    public int pointsPerSuccess;
    public int pointsPerFailure;
}
