

using System.Collections.Generic;
using System.Linq;
using Glee.Behaviours;
using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee;


public class WorldManager
{
    //TODO: cambiar world manager para que acepte un delegado que construya una clase y no se cree la clase hasta el final del bucle de juego
    // Lo que se puede hacer es tener una referencia al spotlight actual. Al terminar el frame anular esa referencia y poner la nueva. Asi no hay problema de modificar la pila de escenas durante la aplicacion
    private LinkedList<World> loadedWorlds;

    private World spotlight;
    private Queue<World> worldsToBeAddedOnTop;
    private Queue<World> worldsToBeAddedOnBottom;
    private Queue<World> worldsToBeRemoved;

    private bool deleteAll = false;

    public WorldManager()
    {
        loadedWorlds = new LinkedList<World>();
        worldsToBeAddedOnTop = new Queue<World>();
        worldsToBeAddedOnBottom = new Queue<World>();
        worldsToBeRemoved = new Queue<World>();



        loadedWorlds.AddLast(new DefaultWorld());
        spotlight = loadedWorlds.Last();
    }

    //TODO: world manager

    public World Spotlight
    {
        get
        {
            return spotlight;
        }
    }

    public bool LiftWorld(World world)
    {
        if (!loadedWorlds.Contains(world))
        {
            return false;
        }

        var it = loadedWorlds.Find(world);

        loadedWorlds.Remove(it);
        loadedWorlds.AddLast(it);

        return true;
    }

    /// <summary>
    /// Stack a new world in the world queue
    /// </summary>
    public void StackWorld(World world)
    {
        worldsToBeAddedOnTop.Enqueue(world);
    }

    public void StackWorldBottom(World world)
    {
        worldsToBeAddedOnBottom.Enqueue(world);
    }


    /// <summary>
    /// Removes the current world and adds the new one to the queue
    /// </summary>
    public void UpdateWorld(World world)
    {
        worldsToBeAddedOnTop.Enqueue(world);
        worldsToBeRemoved.Enqueue(loadedWorlds.Last());
    }

    /// <summary>
    /// Removes the current world, being the previous world the one to gain the spotlight
    /// </summary>
    public void PopWorld()
    {
        worldsToBeRemoved.Enqueue(loadedWorlds.Last());
    }


    public void PopWorldBottom()
    {
        worldsToBeRemoved.Enqueue(loadedWorlds.First());
    }

    /// <summary>
    /// Removes all worlds and closes the game
    /// </summary>
    public void Close()
    {
        deleteAll = true;
    }

    public void Udpate(GameTime gameTime)
    {
        Spotlight.Udpate(gameTime);
    }

    public void Render(GameTime gameTime)
    {
        Spotlight.Render(gameTime);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>returns true if the stack is empty: close the app</returns>
    public bool UpdateStack()
    {
        if (deleteAll) {

            foreach (World world in loadedWorlds.Reverse())
            {
                //TODO: check that deletion is in reverse order

                if (world is IRemovable removable)
                    removable.Remove();
            }
            return true;
        }

        foreach (World world in worldsToBeRemoved)
        {
            loadedWorlds.Remove(world);

            if (world is IRemovable removable)
            {
                removable.Remove();
            }
        }
        worldsToBeRemoved.Clear();

        foreach (World world in worldsToBeAddedOnBottom)
        {
            loadedWorlds.AddFirst(world);
            world.Initialize();
        }
        worldsToBeAddedOnBottom.Clear();

        foreach (World world in worldsToBeAddedOnTop)
        {
            loadedWorlds.AddLast(world);
            world.Initialize();   
        }
        worldsToBeAddedOnTop.Clear();

        spotlight = loadedWorlds.Last();

        return false;
    }

}



class DefaultWorld : World
{
    public override void CreateWorld(){ }
}



