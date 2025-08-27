namespace Glee.Graphics;



public class Material
{
    public Shader ShaderSource { get; private set; }
    public Color MainColor { get; set; }

    public Material(Shader shader)
    {
        MainColor = Color.White;
        //TODO: make a clone of the shader to have custom properties
        ShaderSource = shader;
    }

    public bool HasCustomShader
    {
        get
        {
            return ShaderSource != null;
        }
    }
}