using Glee.Components;

namespace Glee.Physics;




public interface ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B);


    /// <summary>
     /// Cast ICollisions to the selected Types. The param order doesn't matter and it's automatically asigned the asked position
    /// </summary>
    public static (BoundsA, BoundsB) BoundCast<BoundsA, BoundsB>(Bounds A, Bounds B, bool recursive = false)
    where BoundsA : Bounds
    where BoundsB : Bounds
    {
        if (A is not BoundsA castingA || B is not BoundsB castingB)
        {
            if (recursive)
            {
                throw new System.Exception(
                    $"Collision resolver casting error. Expected {nameof(BoundsA) + "," + nameof(BoundsB)} but received{B.GetType() + ", " + A.GetType()}");
            }

            return BoundCast<BoundsA, BoundsB>(B, A, true);
        }

        return (castingA, castingB);
    }
}