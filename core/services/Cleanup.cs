using System;
using System.Collections.Generic;
using Glee.Behaviours;
using Glee.Engine;

namespace Glee
{
    public class Cleanup : CoreService, IUpdatable
    {
        readonly GleeContainer elementsToRemove = [];

        public void SetToRemove(GleeObject obj)
        {
            elementsToRemove.Add(obj);
        }


        public class RemoveComponent(Component component) : GleeEvent
        {
            public Component component = component;
        }

        public class RemoveEntity(GleeEntityRaw entity) : GleeEvent
        {
            public GleeEntityRaw entity = entity;
        }

        public class RemoveWorld(World world) : GleeEvent
        {
            public World world = world;
        }

        public class RemoveService(Service service) : GleeEvent
        {
            public Service service = service;
        }

        public class RemoveResource(GleeResource resource) : GleeEvent
        {
            public GleeResource resource = resource;
        }


        public void Update()
        {
            foreach (GleeObject obj in elementsToRemove)
            {
                if (obj is GleeEntityRaw entity)
                    Raise(new RemoveEntity(entity), Events.Scope.Global);
                else if (obj is Component component)
                    Raise(new RemoveComponent(component), Events.Scope.Global);
                else if (obj is Service service)
                    Raise(new RemoveService(service), Events.Scope.Global);
                else if (obj is World world)
                    Raise(new RemoveWorld(world), Events.Scope.Global);
                else if (obj is GleeResource resource)
                    Raise(new RemoveResource(resource), Events.Scope.Global);
                else
                {
                    //TODO: error
                }

                obj.Delete();
            }

            elementsToRemove.Clear();
        }

    }

    namespace Engine
    {
        public partial class GleeObject
        {
            public void Destroy()
            {
                Destroy(this);
            }

            public static void Destroy(GleeObject obj)
            {
                Get<Cleanup>().SetToRemove(obj);
            }
        }
    }
}