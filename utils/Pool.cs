using System;
using System.Collections.Generic;
using Glee.Behaviours;
using Glee.Engine;

namespace Glee;




/*
    Pool<Bullet> bulletPool = new Pull<Bullet>(CreateBullet);

    Bullet bullet = bulletPool.Get();


    struct PoolEntity<T>{
    
        ID id;
        T obj;
    
        T Get() {
        
            if(id != obj.id) GleeError.Throw();
            return obj;
        }
    }


    Destroy(bullet); // => this adds bullet back to the pool automatically                                 
*/



public class Pool<T>(Func<T> factory, Callback reset) where T : GleeEntity
{
    public int Count { get; private set; } = 0;
    private List<PoolEntity> entities = new();
    private Func<T> factory = factory;
    private Callback reset = reset;

    /// <summary>
    /// Entity handler. The use of PoolEntity is preferred over using the object directly to avoid bugs.
    /// </summary>
    public class PoolEntity
    {
        private UID uid;
        private T obj;
        internal bool isUsed;

        internal PoolEntity(T entity)
        {
            obj = entity;
            uid = entity.UID;
            isUsed = false;
        }


        public static implicit operator T(PoolEntity entity)
        {
            //TODO: throw errors

            if (entity.uid != entity.obj.UID) return null;
            if (!entity.obj.IsValid) return null;

            return entity.obj;
        }
    }


    public class PoolObserver : Component, IDestroyable
    {
        internal Pool<T> pool;

        public void OnDestroy()
        {
            //TODO: cleanup service
            //TODO: cancel destroy
        }
    }




    public PoolEntity Get()
    {
        if (Count == entities.Count)
        {
            //TODO: parametrizar el numero de elementos que se añaden de cada vez a la pool
            //Por ahora solo hace uno
            Create();
        }


        PoolEntity entity = entities[Count];
        entity.isUsed = true;

        Count++;
        return entity;
    }


    void Create()
    {
        PoolEntity entity = new(factory());
        ((T)entity).CreateComponent<PoolObserver>();
        entities.Add(entity);
    }


    public void Prewarm(int count)
    {
        Utils.Loop(Create, count);
    }


    public void Restore(PoolEntity entity)
    {

    }
    

}
