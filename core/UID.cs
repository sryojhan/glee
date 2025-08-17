

using System;

namespace Glee.Engine;


public struct UID
{
    private static uint next = 0;

    private readonly uint uid;

    public UID()
    {
        uid = ++next;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return this == (UID)obj;
    }

    public static bool operator ==(UID A, UID B) {

        return A.uid == B.uid;
    }
      
    public static bool operator !=(UID A, UID B) {

        return A.uid != B.uid;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
