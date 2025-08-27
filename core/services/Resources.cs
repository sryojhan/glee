using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Glee.Graphics;

namespace Glee.Engine
{



    //Resouce types: Texture, Sprite, Shader, Material, Font, Animation


    //In the future: Sound, Music, Particle, Json, CSV, XML
    public class Resources : Service
    {
        private Dictionary<string, GleeResource> resources;

        public Resources()
        {
            resources = [];
        }


        public static T Convert<T>(GleeResource resource) where T : GleeResource
        {
            if (resource is not T conversion)
            {
                GleeError.ResourceTypeMismatch(typeof(T).ToString(), resource.GetType().ToString());
                return null;
            }

            return conversion;
        }




        //TODO: for now all resources will be loaded globally
        public new ResourceType Load<ResourceType>(string name) where ResourceType : GleeResource, new()
        {
            if (resources.TryGetValue(name, out GleeResource gleeResouce))
            {
                ResourceType resource = Convert<ResourceType>(gleeResouce);
                return resource;
            }

            ResourceType newResource = new();

            if (!newResource.Load(name, GleeCore.Content))
                return null;

            resources.Add(name, newResource);

            return newResource;
        }


        public Texture LoadTexture(string name)
        {
            return Load<Texture>(name);
        }


        public Sprite CreateSprite(string baseTexture, string spriteName, Point position, Point size)
        {

            Texture texture = Load<Texture>(baseTexture);
            return CreateSprite(texture, spriteName, position, size);
        }

        public Sprite CreateSprite(Texture baseTexture, string spriteName, Point position, Point size)
        {
            if (resources.ContainsKey(spriteName))
            {
                GleeError.ResourceAlreadyExists(spriteName);
            }

            Sprite spr = new(baseTexture, spriteName, new Rectangle(position, size));


            resources[spriteName] = spr;

            return spr;
        }


        public Sprite LoadSprite(string name)
        {
            if (!resources.TryGetValue(name, out GleeResource resource))
            {
                GleeError.AssetNotFound(name);
                return null;
            }

            return Convert<Sprite>(resource);
        }


        public Animation CreateAnimation(string name, ICollection<ITexture> frames, float speed)
        {
            return null;
        }

        public Animation LoadAnimation(string name)
        {
            return null;
        }


        public Shader LoadShader(string name)
        {
            return null;
        }

        public Material LoadMaterial(string name)
        {
            return null;
        }

        public Font LoadFont(string name)
        {
            return null;
        }


        // Methods for custom resource types outside of the engine
        public void RegisterResource(string name, GleeResource resource)
        {
            if (resources.ContainsKey(name))
            {
                GleeError.ResourceAlreadyExists(name);
            }

            resources[name] = resource;
        }

        public GleeResource Get(string name)
        {
            if (!resources.TryGetValue(name, out GleeResource output))
            {
                GleeError.AssetNotFound(name);

                return null;
            }

            return output;
        }


    }

}


namespace Glee
{
    public static class ResourcesExtension
    {
        public static ResourceType Load<ResourceType>(this Engine.GleeObject obj, string name)
        {
            return Services.Fetch<Engine.Resources>().Load<ResourceType>(name);
        }
    }
}