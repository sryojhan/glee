using Glee.Engine;

namespace Glee;


public static class Utils
{
    public static Vector Left => new(-1, 0);
    public static Vector Right => new(1, 0);
    public static Vector Down => new(0, -1);
    public static Vector Up => new(0, 1);

    public static Vector Normalized(this Vector vector2)
    {
        if (vector2 == Vector.Zero) return Vector.Zero;
        vector2.Normalize();

        return vector2;
    }

    public static VectorInt ToVectorInt(this Vector vector)
    {
        return vector.ToPoint();
    }

    public static Vector ToVector(this VectorInt vector)
    {
        return vector.ToVector2();
    }

    public const float Delta = 0.00001f;
    public delegate void Callback();



    public static class Alignment
    {
        public static void Fit(EntityRaw entity, Vector min, Vector max)
        {
            if (min.X > max.X || min.Y > max.Y) GleeError.Throw("MAX vector is greater than MAX vector");

            Vector size = max - min;
            Vector center = min + size * 0.5f;

            entity.Position = center;
            entity.Size = size;
        }

        public static void Pivot(EntityRaw entity, Vector point, Vector pivot)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, entity.HalfSize.Y) - entity.Size * pivot;
        }


        public static void AlignLeft(EntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Right * entity.HalfSize;
        }

        public static void AlignTop(EntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Down * entity.HalfSize;
        }

        public static void AlignRight(EntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Left * entity.HalfSize;
        }

        public static void AlignBottom(EntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Up * entity.HalfSize;
        }



        public static void AlignTopLeft(EntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, -entity.HalfSize.Y);
        }
        public static void AlignTopRight(EntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(-entity.HalfSize.X, -entity.HalfSize.Y);
        }

        public static void AlignBottomRight(EntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(-entity.HalfSize.X, entity.HalfSize.Y);
        }

        public static void AlignBottomLeft(EntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, entity.HalfSize.Y);
        }


        //Getters
        public static Vector Left(EntityRaw entity) => entity.Position + Utils.Left * entity.HalfSize;
        public static Vector Right(EntityRaw entity) => entity.Position + Utils.Right * entity.HalfSize;
        public static Vector Bottom(EntityRaw entity) => entity.Position + Utils.Down * entity.HalfSize;
        public static Vector Top(EntityRaw entity) => entity.Position + Utils.Up * entity.HalfSize;




        public static Vector TopLeft(EntityRaw entity) => entity.Position + new Vector(-entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector TopRight(EntityRaw entity) => entity.Position + new Vector(entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector BottomRight(EntityRaw entity) => entity.Position + new Vector(entity.HalfSize.X, -entity.HalfSize.Y);
        public static Vector BottomLeft(EntityRaw entity) => entity.Position + new Vector(-entity.HalfSize.X, -entity.HalfSize.Y);

    }

}