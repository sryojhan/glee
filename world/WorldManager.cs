

using System.Collections.Generic;
using System.Linq;
using Glee.Behaviours;
using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee;


public class WorldManager
{
    // Lo que se puede hacer es tener una referencia al spotlight actual. Al terminar el frame anular esa referencia y poner la nueva. Asi no hay problema de modificar la pila de escenas durante la aplicacion
    private readonly LinkedList<World> loadedWorlds;

    private World spotlight;
    private readonly Queue<World> worldsToBeAddedOnTop;
    private readonly Queue<World> worldsToBeAddedOnBottom;
    private readonly Queue<World> worldsToBeRemoved;

    private bool deleteAll = false;

    public WorldManager()
    {
        loadedWorlds = new LinkedList<World>();
        worldsToBeAddedOnTop = new Queue<World>();
        worldsToBeAddedOnBottom = new Queue<World>();
        worldsToBeRemoved = new Queue<World>();

        spotlight = null;
    }


    public World Spotlight => spotlight;

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

    public void ProcessFrame()
    {
        Spotlight.ProcessFrame();
    }

    public void Render()
    {
        Spotlight.RenderFrame();
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
                if (world is IRemovableObserver removable)
                    removable.OnRemove();
            }
            return true;
        }

        foreach (World world in worldsToBeRemoved)
        {
            loadedWorlds.Remove(world);

            if (world is IRemovableObserver removable)
            {
                removable.OnRemove();
            }
        }
        worldsToBeRemoved.Clear();

        foreach (World world in worldsToBeAddedOnBottom)
        {
            spotlight = world;

            loadedWorlds.AddFirst(world);
            world.Initialize();
        }
        worldsToBeAddedOnBottom.Clear();

        foreach (World world in worldsToBeAddedOnTop)
        {
            spotlight = world;

            loadedWorlds.AddLast(world);
            world.Initialize();   
        }
        worldsToBeAddedOnTop.Clear();

        spotlight = loadedWorlds.Last();

        return false;
    }

}


