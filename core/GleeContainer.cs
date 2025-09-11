
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Glee.Engine;

public class GleeContainer : GleeObject, ICollection
{
    private readonly Dictionary<UID, GleeObject> gleeObjects; //TODO: cambiar por un hashset

    public int Count => gleeObjects.Count;

    public bool IsSynchronized => false;

    public object SyncRoot => this;


    public GleeContainer()
    {
        gleeObjects = [];
    }

    public void Add(GleeObject gleeObj)
    {
        //TODO: check error
        gleeObjects.Add(gleeObj, gleeObj);
    }


    public bool Remove(UID uid)
    {
        if (Contains(uid))
        {
            gleeObjects.Remove(uid);
            return true;
        }

        return false;
    }

    public bool Contains(UID uid)
    {
        return gleeObjects.ContainsKey(uid);
    }

    public void Clear()
    {
        gleeObjects.Clear();
    }


    public IEnumerator GetEnumerator()
    {
        return gleeObjects.Values.GetEnumerator();
    }


    public void CopyTo(Array array, int index)
    {
        // 1. Argument validation:
        // It's crucial to validate the arguments to prevent runtime errors.
        ArgumentNullException.ThrowIfNull(array);

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "The index cannot be negative.");
        }

        // Check if the destination array has enough space.
        if (Count > array.Length - index)
        {
            throw new ArgumentException("The destination array is too small to copy all elements.", nameof(array));
        }

        // 2. Efficient type casting and copying:
        // We try to cast the generic 'Array' to the specific type of our elements.
        // This allows for a more efficient and type-safe copy.
        if (array is GleeObject[] keyValuePairs)
        {
            // If the array is of the correct type, we can copy the elements directly.
            foreach (var kvp in gleeObjects.Values)
            {
                keyValuePairs[index++] = kvp;
            }
        }
        else
        {
            // If the array is of a different type (e.g., object[]),
            // we use the SetValue method which handles the assignment safely.
            // This is necessary because we can't directly use 'array[index] = ...'
            // on a non-generic 'Array'.
            foreach (var kvp in gleeObjects)
            {
                array.SetValue(kvp, index++);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    //TODO random utils class
    public GleeObject Random()
    {
        if (Count == 0) return null;

        int r = new Random().Next(Count);
        return gleeObjects.ElementAt(r).Value;
    }

}