

namespace Glee.Physics;

public abstract class Bounds
{
    public EntityRaw entity { get; internal set; }

    public Vector Position => entity.Position;
    public Vector Size => entity.Size;


    public abstract bool Raycast(Vector origin, Vector direction, float distance, out RaycastHit hit);
}