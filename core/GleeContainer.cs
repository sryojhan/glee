
using System;
using System.Collections;
using System.Collections.Generic;

namespace Glee.Engine;

public class GleeContainer : GleeObject, IDictionary, IEnumerable
{
    private readonly Dictionary<UID, GleeObject> gleeObjects;

    public bool IsFixedSize => false;

    public bool IsReadOnly => false;

    public ICollection Keys => gleeObjects.Keys;

    public ICollection Values => gleeObjects.Values;

    public int Count => gleeObjects.Count;

    public bool IsSynchronized => false;

    public object SyncRoot => this;

    public object this[object key]
    {
        get
        {
            if (key is not UID uid)
                throw new ArgumentException("Key must be of type UID", nameof(key));

            if (!Contains(uid)) return null;

            return gleeObjects[uid];
        }
        set{
            if (key is not UID uid)
                throw new ArgumentException("Key must be of type UID", nameof(key));

            if (value is not GleeObject gleeObj)
                throw new ArgumentException("Key must be of type GleeObject", nameof(gleeObj));

            gleeObjects[uid] = gleeObj;
        }
    }


    public GleeContainer()
    {
        gleeObjects = [];
    }

    public void Add(GleeObject gleeObj)
    {
        gleeObjects.Add(gleeObj, gleeObj);
    }

    public void Add(object key, object value)
    {
        if (key is not UID uid)
            throw new ArgumentException("Key must be of type UID", nameof(key));

        if (value is not GleeObject gleeObj)
            throw new ArgumentException("Value must be of type GleeObject", nameof(value));

        gleeObjects.Add(uid, gleeObj);
    }

    public bool Remove(UID uid)
    {
        if (Contains(uid))
        {
            Remove(uid);
            return true;
        }

        return false;
    }


    public void Remove(object key)
    {
        if (key is not UID uid)
            throw new ArgumentException("Value must be of type UID", nameof(key));

        Remove(uid);
    }

    public bool Contains(UID uid)
    {
        return gleeObjects.ContainsKey(uid);
    }


    public bool Contains(object key)
    {
        if (key is not UID uid)
            throw new ArgumentException("Value must be of type UID", nameof(key));

        return Contains(uid);
    }

    public void Clear()
    {
        gleeObjects.Clear();
    }


    public IDictionaryEnumerator GetEnumerator()
    {
        return gleeObjects.GetEnumerator();
    }


    public void CopyTo(Array array, int index)
{
    // 1. Argument validation:
    // It's crucial to validate the arguments to prevent runtime errors.
    if (array == null)
    {
        throw new ArgumentNullException(nameof(array));
    }

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
    if (array is KeyValuePair<UID, GleeObject>[] keyValuePairs)
    {
        // If the array is of the correct type, we can copy the elements directly.
        foreach (var kvp in gleeObjects)
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
}