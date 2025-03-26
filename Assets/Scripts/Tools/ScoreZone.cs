using UnityEngine;

public class ScoreZone : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Игрок заработал очки!");
    }
}