using System;
using System.Collections.Generic;
using Glee.Engine;

namespace Glee.Physics;



public class CollisionMatrix()
{
    //TODO: make layers case insensitive
    //TODO: when order doesn't matter change from list to hashset
    readonly HashSet<string> allLayers = [];

    //TODO: use another data structure
    readonly HashSet<(string, string)> collisionMatrix = [];

    public bool ContainsLayer(string layer)
    {
        return allLayers.Contains(layer);
    }

    public void RegisterLayer(string layer, bool collideWithTheRest = false)
    {
        if (allLayers.Contains(layer))
        {
            //TODO: error message
            GleeError.Throw("The layer is already added to the game");
            return;
        }


        if (collideWithTheRest)
        {
            foreach (string other in allLayers)
            {
                DoCollision(layer, other);
            }
        }

        allLayers.Add(layer);
    }

    public bool TryAddLayer(string layer)
    {
        if (allLayers.Contains(layer)) return false;
        RegisterLayer(layer);
        return true;
    }


    public void RemoveLayer(string layer)
    {
        CheckLayerForErrors(layer);

        collisionMatrix.RemoveWhere(pair => InPair(layer, pair));
        allLayers.Remove(layer);
    }


    public void DoCollision(string layerA, string layerB)
    {
        CheckLayerForErrors(layerA);
        CheckLayerForErrors(layerB);

        collisionMatrix.Add(MakePair(layerA, layerB));
    }

    public void IgnoreCollision(string layerA, string layerB)
    {
        CheckLayerForErrors(layerA);
        CheckLayerForErrors(layerB);

        collisionMatrix.Remove(MakePair(layerA, layerB));
    }


    public bool Collides(string layerA, string layerB)
    {
        CheckLayerForErrors(layerA);
        CheckLayerForErrors(layerB);        

        //TODO: use an EPIC data structure so we do not need to seach for this in everyframe
        foreach ((string, string) pair in collisionMatrix)
        {
            if (InPair(layerA, pair) && InPair(layerB, pair)) return true;
        }
        return false;
    }


    private static (string, string) MakePair(string layerA, string layerB)
    {
        bool greater = string.Compare(layerA, layerB) > 0;

        string a = greater ? layerA : layerB;
        string b = greater ? layerB : layerA;

        return (a, b);
    }


    private static bool InPair(string layer, (string, string) pair)
    {
        if (pair.Item1 == layer) return true;
        if (pair.Item2 == layer) return true;
        return false;
    }


    /// <returns>Returns true in case of error</returns>
    private bool CheckLayerForErrors(string layer)
    {
        if (!ContainsLayer(layer))
        {
            GleeError.Throw($"Physics doesn't exist [{layer}]");
            return true;
        }

        return false;
    }

}
