using Glee.Behaviours;
using Glee.Engine;


namespace Glee.Debug;


public class DebugComponent : Component, IUpdatable
{
    public void Update()
    {


        Entity.Position += new Vector2(1, 0);

        //Print(1.0f / GleeCore.Time.ElapsedGameTime.TotalSeconds);
    }
}
