
using Glee.Engine;

namespace Glee;


public abstract class ComponentRaw : GleeObject
{
    public EntityRaw entity { get; init; } = null;
    public World world => entity.world;
    public Time Time => world.Time;
    public bool Enabled { get; set; } = true;





    internal ComponentType TryGetComponent<ComponentType>() where ComponentType : ComponentRaw
    {
        if (entity is Entity entityComposed)
        {
            return entityComposed.GetComponent<ComponentType>();
        }
        else
        {
            //TODO: custom error
            GleeError.Throw($"Dependency not initialised [{typeof(ComponentType)}]");
            return null;
        }
    }


}