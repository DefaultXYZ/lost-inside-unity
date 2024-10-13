using UnityEngine;

[CreateAssetMenu(fileName = "StoryNoteTexts", menuName = "Scriptable Objects/StoryNoteTexts")]
public class StoryNoteTexts : ScriptableObject
{
    [SerializeField]
    private string text;
}
