namespace Glee.Graphics;



public class Material
{
    public Shader ShaderSource { get; private set; }
    public Color MainColor { get; set; }

    public Material(Shader shader)
    {
        ShaderSource = shader;
    }
}