using Glee;


namespace Glee.Debug;



public class DebugWorld : World
{
    public DebugWorld()
    {
        backgroundColor = Color.Bisque;
    }

    public override void CreateWorld()
    {
        EntityComposed entity = CreateComposedEntity("Debug element");
        entity.CreateComponent<DebugComponent>();

    }
}