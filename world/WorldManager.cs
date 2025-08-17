

using System.Collections.Generic;
using Glee.ECS;
using Glee.ECS.Behaviour;
using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee.ECS;


public class WorldManager
{

    //TODO: cambiar world manager para que acepte un delegado que construya una clase y no se cree la clase hasta el final del bucle de juego

    private Stack<World> worldStack;


    public WorldManager()
    {
        worldStack = new Stack<World>();


        worldStack.Push(new DefaultWorld());
    }

    //TODO: world manager

    public World Spotlight
    {
        get {
            return worldStack.Peek();
        }
    }

    public bool LiftWorld(World world)
    {
        if (!worldStack.Contains(world))
        {
            return false;
        }

        Stack<World> newStack = new Stack<World>();

        foreach (World current in worldStack)
        {

            if (current != world)
                newStack.Push(current);
        }

        newStack.Push(world);
        worldStack = newStack;

        return true;
    }

    public void StackWorld(World world)
    {

    }

    public void SwapWorld()
    {

    }

    public void PopWorld()
    {

    }


    public void Udpate(GameTime gameTime)
    {
        Spotlight.Udpate(gameTime);
    }

    public void Render(GameTime gameTime)
    {
        Spotlight.Render(gameTime);
    }

}



class DefaultWorld : World, IUpdatable
{
    public override void CreateWorld()
    {

    }

    public void Update()
    {
        Print("Hola soy una escena");
    }
}



