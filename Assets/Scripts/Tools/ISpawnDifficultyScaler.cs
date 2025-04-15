using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnDifficultyScaler
{
    float CurrentInterval { get; }
    void AdjustDifficulty();
    void Reset();
}