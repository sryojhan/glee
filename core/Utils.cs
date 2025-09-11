using Glee.Engine;

namespace Glee;

//TODO: Divide this into various classes
//TODO: change name to GleeUtils

//TODO: Dividir entre varios ficheros
public delegate void Callback();
public delegate bool Condition();


public static partial class Utils
{

    public static void Loop(Callback logic, int count)
    {
        for (int i = 0; i < count; i++)
        {
            logic();
        }
    }

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


    public static class Alignment
    {
        public static void Fit(GleeEntityRaw entity, Vector min, Vector max)
        {
            if (min.X > max.X || min.Y > max.Y) GleeError.Throw("MAX vector is greater than MAX vector");

            Vector size = max - min;
            Vector center = min + size * 0.5f;

            entity.Position = center;
            entity.Size = size;
        }

        public static void Pivot(GleeEntityRaw entity, Vector point, Vector pivot)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, entity.HalfSize.Y) - entity.Size * pivot;
        }


        public static void AlignLeft(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Right * entity.HalfSize;
        }

        public static void AlignTop(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Down * entity.HalfSize;
        }

        public static void AlignRight(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Left * entity.HalfSize;
        }

        public static void AlignBottom(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + Utils.Up * entity.HalfSize;
        }



        public static void AlignTopLeft(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, -entity.HalfSize.Y);
        }
        public static void AlignTopRight(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(-entity.HalfSize.X, -entity.HalfSize.Y);
        }

        public static void AlignBottomRight(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(-entity.HalfSize.X, entity.HalfSize.Y);
        }

        public static void AlignBottomLeft(GleeEntityRaw entity, Vector point)
        {
            entity.Position = point + new Vector(entity.HalfSize.X, entity.HalfSize.Y);
        }


        //Getters
        public static Vector Left(GleeEntityRaw entity) => entity.Position + Utils.Left * entity.HalfSize;
        public static Vector Right(GleeEntityRaw entity) => entity.Position + Utils.Right * entity.HalfSize;
        public static Vector Bottom(GleeEntityRaw entity) => entity.Position + Utils.Down * entity.HalfSize;
        public static Vector Top(GleeEntityRaw entity) => entity.Position + Utils.Up * entity.HalfSize;




        public static Vector TopLeft(GleeEntityRaw entity) => entity.Position + new Vector(-entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector TopRight(GleeEntityRaw entity) => entity.Position + new Vector(entity.HalfSize.X, entity.HalfSize.Y);
        public static Vector BottomRight(GleeEntityRaw entity) => entity.Position + new Vector(entity.HalfSize.X, -entity.HalfSize.Y);
        public static Vector BottomLeft(GleeEntityRaw entity) => entity.Position + new Vector(-entity.HalfSize.X, -entity.HalfSize.Y);

    }


    public static Time GetAssociatedTime(GleeObject obj)
    {
        if (obj is Component component) return component.Time;
        if (obj is GleeEntity entity) return entity.Time;
        if (obj is World world) return world.Time;

        //TODO: kind of ugly
        return Services.Fetch<WorldManager>().Spotlight.Time;
    }

}