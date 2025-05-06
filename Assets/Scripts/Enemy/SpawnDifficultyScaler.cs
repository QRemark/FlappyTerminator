public class SpawnDifficultyScaler : ISpawnDifficultyScaler
{
    private readonly float _initialInterval;
    private readonly float _minInterval;
    private readonly float _decreaseStep;

    public float CurrentInterval { get; private set; }

    public SpawnDifficultyScaler(float initial, float min, float step)
    {
        _initialInterval = initial;
        _minInterval = min;
        _decreaseStep = step;
        CurrentInterval = initial;
    }

    public void AdjustDifficulty()
    {
        if (CurrentInterval > _minInterval)
            CurrentInterval -= _decreaseStep;
    }

    public void Reset()
    {
        CurrentInterval = _initialInterval;
    }
}
