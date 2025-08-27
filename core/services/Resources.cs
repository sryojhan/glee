using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Glee.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Glee.Engine
{



    //Resouce types: Texture, Sprite, Shader, Material, Font, Animation


    //In the future: Sound, Music, Particle, Json, CSV, XML
    public class Resources : Service
    {
        public delegate GleeResource GleeResourceFactory(string name);

        private readonly Dictionary<Type, GleeResourceFactory> factories;
        private readonly Dictionary<string, GleeResource> resources;

        //TODO: global and local content managers
        public ContentManager ActiveContentManager => GleeCore.Content;

        public Resources()
        {
            resources = [];


            factories = new() {
                { typeof(Texture), Texture.Create }
            };

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




        //TODO: now all resources will be loaded globally
        public new ResourceType Load<ResourceType>(string name) where ResourceType : GleeResource
        {
            if (resources.TryGetValue(name, out GleeResource gleeResouce))
            {
                ResourceType resource = Convert<ResourceType>(gleeResouce);
                return resource;
            }

            if (typeof(ResourceType) == typeof(Sprite))
            {
                return LoadSprite(name) as ResourceType;
            }


            if (!factories.TryGetValue(typeof(ResourceType), out GleeResourceFactory factory)) {

                GleeError.InvalidInitialization(typeof(ResourceType).ToString());
                return null;
            }


            if (factory.Invoke(name) is not ResourceType newResource)
            {
                // Exception is thrown inside the factory in case of error
                return null;
            }

            resources.Add(name, newResource);

            return newResource;
        }


        public Texture LoadTexture(string name)
        {
            return Load<Texture>(name);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseTexture"></param>
        /// <param name="spriteName"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="combineName">Whether to combine the name with texture name</param>
        /// <returns></returns>
        public Sprite CreateSprite(string baseTexture, string spriteName, Point position, Point size, bool combineName)
        {

            Texture texture = Load<Texture>(baseTexture);
            return CreateSprite(texture, spriteName, position, size, combineName);
        }

        public Sprite CreateSprite(Texture baseTexture, string spriteName, Point position, Point size, bool combineName)
        {
            if (combineName) spriteName = Sprite.CombineName(baseTexture, spriteName);
            

            if (resources.ContainsKey(spriteName))
            {
                GleeError.ResourceAlreadyExists(spriteName);
            }

            Sprite spr = new(baseTexture, spriteName, new Rectangle(position, size));

            resources[spriteName] = spr;

            return spr;
        }


        public Sprite LoadSpriteFromTexture(Texture texture, string spriteName)
        {
            return LoadSprite(Sprite.CombineName(texture, spriteName));
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