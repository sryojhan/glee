using Glee.Engine;

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

    public const float Delta = 0.00001f;
    public delegate void Callback();

    public static readonly Vector2 Zero = Vector2.Zero;




    public static class Alignment
    {
        public static void Fit(EntityRaw entity, Vector2 min, Vector2 max)
        {
            if (min.X > max.X || min.Y > max.Y) GleeError.Throw("MAX vector is greater than MAX vector");

            Vector2 size = max - min;
            Vector2 center = min + size * 0.5f;

            entity.Position = center;
            entity.Size = size;
        }

        public static void Pivot(EntityRaw entity, Vector2 point, Vector2 pivot)
        {
            entity.Position = point + new Vector2(entity.HalfSize.X, entity.HalfSize.Y) - entity.Size * pivot;
        }


        public static void AlignLeft(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + Utils.Right * entity.HalfSize;
        }

        public static void AlignTop(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + Utils.Down * entity.HalfSize;
        }

        public static void AlignRight(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + Utils.Left * entity.HalfSize;
        }

        public static void AlignBottom(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + Utils.Up * entity.HalfSize;
        }



        public static void AlignTopLeft(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + new Vector2(entity.HalfSize.X, -entity.HalfSize.Y);
        }
        public static void AlignTopRight(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + new Vector2(-entity.HalfSize.X, -entity.HalfSize.Y);
        }

        public static void AlignBottomRight(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + new Vector2(-entity.HalfSize.X, entity.HalfSize.Y);
        }

        public static void AlignBottomLeft(EntityRaw entity, Vector2 point)
        {
            entity.Position = point + new Vector2(entity.HalfSize.X, entity.HalfSize.Y);
        }


        //Getters
        public static Vector2 Left(EntityRaw entity) => entity.Position + Utils.Left * entity.HalfSize;
        public static Vector2 Right(EntityRaw entity) => entity.Position + Utils.Right * entity.HalfSize;
        public static Vector2 Bottom(EntityRaw entity) => entity.Position + Utils.Down * entity.HalfSize;
        public static Vector2 Top(EntityRaw entity) => entity.Position + Utils.Up * entity.HalfSize;




        public static Vector2 TopLeft(EntityRaw entity) => entity.Position + new Vector2(-entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector2 TopRight(EntityRaw entity) => entity.Position + new Vector2(entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector2 BottomRight(EntityRaw entity) => entity.Position + new Vector2(entity.HalfSize.X, -entity.HalfSize.Y);
        public static Vector2 BottomLeft(EntityRaw entity) => entity.Position + new Vector2(-entity.HalfSize.X, -entity.HalfSize.Y);

    }

}