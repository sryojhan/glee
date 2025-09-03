using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Glee.Assets;
using Glee.Assets.Text;
using Glee.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Glee.Engine
{


    //TODO: resource container: store globally in the service, and others in each world 
    //Resouce types: Texture, Sprite, Shader, Material, Font, Animation


    //In the future: Sound, Music, Particle, Json, CSV, XML
    public class Resources : CoreService
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
                { typeof(Font), Font.Create },
                { typeof(Shader), Shader.Create },
                { typeof(Texture), Texture.Create },
                { typeof(TextAsset), TextAsset.Create },
                { typeof(JSON), JSON.Create },
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

            if (typeof(ResourceType) == typeof(Animation))
            {
                return LoadAnimation(name) as ResourceType;
            }

            if (!factories.TryGetValue(typeof(ResourceType), out GleeResourceFactory factory))
            {
                GleeError.ResourceFactoryNotFound(typeof(ResourceType).ToString());
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


        /// <param name="combineName">Whether to combine the name with texture name</param>
        /// <returns></returns>
        public Sprite CreateSprite(string baseTexture, string spriteName, VectorInt position, VectorInt size, bool combineName)
        {

            Texture texture = Load<Texture>(baseTexture);
            return CreateSprite(texture, spriteName, position, size, combineName);
        }

        public Sprite CreateSprite(Texture baseTexture, string spriteName, VectorInt position, VectorInt size, bool combineName)
        {
            if (combineName) spriteName = Sprite.CombineName(baseTexture, spriteName);

            if (resources.ContainsKey(spriteName))
            {
                GleeError.ResourceAlreadyExists(spriteName);
            }

            Sprite spr = Sprite.Create(baseTexture, spriteName, new Rectangle(position, size));

            if (!spr) return null;

            resources[spriteName] = spr;

            return spr;
        }


        public Sprite LoadSpriteFromTexture(Texture texture, string spriteName)
        {
            return LoadSprite(Sprite.CombineName(texture, spriteName));
        }

        public Sprite LoadSprite(string name)
        {
            GleeResource resource = Get(name);

            if (!resource) return null;

            return Convert<Sprite>(resource);
        }


        public Animation CreateAnimation(string name, ICollection<ITexture> frames, float speed)
        {
            if (resources.ContainsKey(name))
            {
                GleeError.ResourceAlreadyExists(name);
            }

            Animation animation = Animation.Create(name, frames, speed);

            if (!animation) return null;

            resources[name] = animation;

            return animation;
        }

        public Animation LoadAnimation(string name)
        {
            GleeResource resource = Get(name);

            if (!resource) return null;

            return Convert<Animation>(resource);
        }


        public Shader LoadShader(string name)
        {
            return Load<Shader>(name);
        }

        public Material LoadMaterial(string name)
        {
            return Load<Material>(name);
        }

        public Font LoadFont(string name)
        {
            return Load<Font>(name);
        }


        public TextAsset LoadText(string name)
        {
            return Load<TextAsset>(name);
        }

        public JSON LoadJson(string name)
        {
            return Load<JSON>(name);
        }

        public TargetClass LoadJson<TargetClass>(string name) where TargetClass : class
        {
            JSON json = Load<JSON>(name);

            if (json)
                return json.Cast<TargetClass>();

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

        public GleeObject GetSafe(string name)
        {
            if (!resources.TryGetValue(name, out GleeResource output))
            {
                return null;
            }

            return output;
        }


        public void RegisterNewFactory<ResourceType>(GleeResourceFactory factory)
        {
            factories.Add(typeof(ResourceType), factory);
        }

    }

}

