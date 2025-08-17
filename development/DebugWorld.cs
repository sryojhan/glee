using Glee;
using Glee.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Glee.Components;
using System.Collections.Generic;

namespace Glee.Debug;



public class DebugWorld : World
{
    public DebugWorld()
    {
        backgroundColor = Color.Bisque;
    }

    public override void CreateWorld()
    {
        EntityComposed entity = CreateComposedEntity("Debug element");
        entity.CreateComponent<DebugComponent>();

        Texture2D animeGirl = Content.Load<Texture2D>("images/anime-girl");
        entity.CreateComponent<SpriteComponent>().Sprite = animeGirl;
    }
}