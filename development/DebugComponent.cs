using Glee.Behaviours;


namespace Glee.Debug;


public class DebugComponent : Component, IInitializable, IUpdatable
{
    private int counter;

    public void Initialize()
    {
        Print("initialising");
        counter = 0;
    }

    public void Update()
    {
        Print(counter++);
    }
}
