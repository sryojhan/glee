using Glee;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Glee.Components;

public class SpriteComponent : Component, IRenderizable
{
    public Texture2D Sprite { get; set; }


    public void Render()
    {
        if (Sprite == null) return;

        Entity entity = Entity;

        //TODO: refactor this

        //TODO: Add color
        //TODO: Add Origin
        //TODO: Add Scale
        //TODO: Add effects
        //TODO: Set as fixed size, not as a scale
        //TODO: Add effects
        //TODO: Add layer depth 



        Renderer.Render(Sprite, entity.Position, entity.Rotation);
    }
}
