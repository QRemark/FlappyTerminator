using UnityEngine;

public interface ISpawnPositionProvider
{
    Vector3 GetValidPosition(Transform player);
}