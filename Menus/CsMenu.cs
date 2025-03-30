namespace PukiTools.GodotSharp;

/// <summary>
/// A template for a menu. Relies heavily on Control nodes.
/// </summary>
[GlobalClass] public abstract partial class CsMenu : Control
{
    /// <summary>
    /// The Control node that's initially focused on when bringing up this menu.
    /// </summary>
    [Export] public Control InitialFocus;

    /// <summary>
    /// Every Control node in the menu that can be focused on.
    /// </summary>
    [Export] public Control[] Focusable = [];

    /// <summary>
    /// If toggled, triggers <see cref="UpdateSelection"/> with null arguments when the mouse exits a menu object.
    /// </summary>
    [Export] public bool UnfocusOnMouseExit = false;
    
    public override void _Ready()
    {
        base._Ready();
        
        for (int i = 0; i < Focusable.Length; i++)
        {
            Control cur = Focusable[i];
            cur.MouseEntered += cur.GrabFocus;
            cur.FocusEntered += () => UpdateSelection(cur);
            
            if (UnfocusOnMouseExit)
                cur.MouseExited += () => UpdateSelection(null);
        }
    }
    
    /// <summary>
    /// Triggered every time a menu option is focused.
    /// </summary>
    /// <param name="focused">The focused Control</param>
    public abstract void UpdateSelection(Control focused);
}