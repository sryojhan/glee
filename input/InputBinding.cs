using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Glee.Input;



public class InputBinding
(
    string name,
    ICollection<GenericButton> positive = null, ICollection<GenericButton> negative = null,
    ICollection<GenericButton> positiveAlternative = null, ICollection<GenericButton> negativeAlternative = null
)
{
    public string Name { get; private set; } = name;

    private List<GenericButton> positive = positive != null ? [.. positive] : new();
    private List<GenericButton> negative = negative != null ? [.. negative] : new();
    private List<GenericButton> positiveAlternative = positiveAlternative != null ? [.. positiveAlternative] : new();
    private List<GenericButton> negativeAlternative = negativeAlternative != null ? [.. negativeAlternative] : new();


    public InputBinding Bind(GenericButton button)
    {
        positive.Add(button);
        return this;
    }

    public InputBinding BindNegative(GenericButton button)
    {
        negative.Add(button);
        return this;
    }

    public InputBinding BindSecondary(GenericButton button)
    {
        positiveAlternative.Add(button);
        return this;
    }

    public InputBinding BindSecondaryNegative(GenericButton button)
    {
        negativeAlternative.Add(button);
        return this;
    }




    public bool IsDown => positive.Find(button => button.IsDown) != null;
    public bool IsUp => positive.Find(button => button.IsDown) == null;
    public bool IsJustDown => positive.Find(button => button.IsJustDown) != null;
    public bool IsJustUp => positive.Find(button => button.IsJustUp) != null;



    public float Value
    {
        get
        {
            float output = 0;

            foreach (GenericButton button in positive)
            {
                output += button.Value;
            }

            foreach (GenericButton button in negative)
            {
                output -= button.Value;
            }

            output = MathHelper.Clamp(output, 0, 1);

            return output;
        }
    }
    

    public Vector2 Value2D
    {
        get
        {
            Vector2 output = Vector2.Zero;

            foreach (GenericButton button in positive)
            {
                output += button.Value2D;
            }

            foreach (GenericButton button in negative)
            {
                output -= button.Value2D;
            }

            output = Vector2.Clamp(output, Vector2.Zero, Vector2.One);

            return output;
        }
    }

}