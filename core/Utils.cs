namespace Glee;


public static class Utils
{
    public static Vector2 Left => new(-1, 0);
    public static Vector2 Right => new(1, 0);
    public static Vector2 Down => new(0, -1);
    public static Vector2 Up => new(0, 1);

    public static Vector2 Normalized(this Vector2 vector2)
    {
        if (vector2 == Vector2.Zero) return Vector2.Zero;
        vector2.Normalize();

        return vector2;
    }

}