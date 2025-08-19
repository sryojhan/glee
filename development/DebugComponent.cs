using Glee.Behaviours;
using Glee.Engine;


namespace Glee.Debug;


public class DebugComponent : Component, IUpdatable
{
    public void Update()
    {
        Entity.Position += new Vector2(1, 0);
        //Entity.Size += new Vector2(2, 1);
    }
}
