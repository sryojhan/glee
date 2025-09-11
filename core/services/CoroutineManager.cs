using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Glee.Behaviours;


namespace Glee.Engine;



class CoroutineManager : CoreService, IUpdatable
{
    readonly List<Coroutine> coroutines = [];

    public Coroutine Launch(GleeObject obj, IEnumerator enumerator)
    {
        Coroutine coroutine = new(enumerator);

        if (!enumerator.MoveNext()) return null;

        if (enumerator.Current is not Wait wait)
        {
            //TODO: error
            GleeError.Throw("Invalid coroutine wait object");
            return null;
        }

        coroutine.wait = wait;
        coroutine.Owner = obj;

        coroutines.Add(coroutine);

        return coroutine;
    }


    public void Update()
    {
        for (int i = coroutines.Count - 1; i >= 0; i--)
        {
            Coroutine co = coroutines[i];

            if (co.wait != null && !co.wait.NewFrame(Utils.GetAssociatedTime(co.Owner)))
            {
                continue;
            }

            if (!co.enumerator.MoveNext())
            {
                coroutines.RemoveAt(i);
                continue;
            }

            if (co.enumerator.Current is not Wait wait)
            {
                //TODO: error
                GleeError.Throw("Invalid coroutine wait object");
                continue;
            }

            co.wait = wait;
        }
    }

}



public partial class GleeObject
{
    protected void Launch(IEnumerator coroutine)
    {
        Get<CoroutineManager>().Launch(this, coroutine);
    }

}