namespace Glee.Physics;


public class Rect(Entity entity) : Bounds(entity)
{

}


public class Circle(Entity entity) : Bounds(entity)
{
    public float Radius => entity.Size.X * 0.5f;
}