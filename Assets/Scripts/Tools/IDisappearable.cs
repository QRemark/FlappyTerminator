public interface IDisappearable
{
    event System.Action<IDisappearable> Disappeared;

    void Disappear();
}