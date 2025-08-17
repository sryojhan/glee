
using System;
using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee.ECS;



public abstract class Component: GleeObject
{
    public bool Enabled { get; set; } = true;
    public Entity Owner { get; private set; } = null;

}