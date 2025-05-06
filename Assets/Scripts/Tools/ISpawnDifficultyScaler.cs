public interface ISpawnDifficultyScaler
{
    float CurrentInterval { get; }

    void AdjustDifficulty();

    void Reset();
}