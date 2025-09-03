
using System.ComponentModel;
using Glee.Engine;

namespace Glee;


public abstract class Component : ComponentRaw
{
    public new Entity entity { get; init; }
    public ComponentType GetComponent<ComponentType>() where ComponentType : Component => entity.GetComponent<ComponentType>();
}