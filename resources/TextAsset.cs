using System;
using Glee.Assets.Internal;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;

namespace Glee.Assets
{

    namespace Internal
    {
        public class TextAssetRaw
        {
            public string Value { get; set; }
        }


        public class TextAssetReader : ContentTypeReader<TextAssetRaw>
        {
            protected override TextAssetRaw Read(ContentReader input, TextAssetRaw existingInstance)
            {
                return new() { Value = input.ReadString() };
            }
        }

    }


    public class TextAsset : GleeResource
    {
        public string Content { get; private set; }
        protected override IDisposable DisposableObj => null;

        protected TextAsset(string name, string value) : base(name)
        {
            Content = value;
        }

        public static TextAsset Create(string name)
        {
            try
            {
                TextAssetRaw raw = Get<Resources>().ActiveContentManager.Load<TextAssetRaw>($"texts/{name}");
                return new TextAsset(name, raw.Value);
            }
            catch (ContentLoadException)
            {
                GleeError.AssetNotFound(name);
                return null;
            }

        }
    }
}