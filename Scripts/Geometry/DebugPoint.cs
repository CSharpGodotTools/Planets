using Godot;

namespace Planets;

/// <summary>
/// A labelled sphere mesh, useful for when debugging 3D procedural meshes. The label uses
/// a billboard so it always faces the camera.
/// </summary>
public class DebugPoint
{
    private const float SphereRadius = 0.03f;
    private const float SphereHeight = SphereRadius * 2;
    private const int SphereRings = 8;
    private const int SphereRadialSegments = SphereRings * 2;
    private const int FontSize = 64;
    private const float FontScale = 0.1f;
    private const float FontOffset = 125f;

    private readonly SphereMesh _mesh;
    private readonly MeshInstance3D _meshInstance;
    private readonly Label3D _label;

    public DebugPoint(string labelText = "")
    {
        _mesh = new SphereMesh()
        {
            Radius = SphereRadius,
            Height = SphereHeight,
            Rings = SphereRings,
            RadialSegments = SphereRadialSegments,
            Material = new StandardMaterial3D 
            { 
                AlbedoColor = Colors.Green,
                // Debug points are unaffected by environment lighting
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded
            }
        };
        
        _meshInstance = new MeshInstance3D
        {
            Mesh = _mesh
        };

        _label = new Label3D
        {
            Text = labelText,
            FontSize = FontSize,
            Scale = Vector3.One * FontScale,
            Offset = new Vector2(0, FontOffset), // Offset Y is always up unlike position
            Billboard = BaseMaterial3D.BillboardModeEnum.Enabled, // Always face camera
            RenderPriority = 2, // Render in front of Godot gizmos
            OutlineRenderPriority = 1,
            DoubleSided = false // Since it has billboard we will never see behind
        };

        _meshInstance.AddChild(_label);
    }

    /// <summary>
    /// Sets the position of this mesh.
    /// </summary>
    public DebugPoint At(Vector3 position)
    {
        _meshInstance.Position = position;
        return this;
    }

    /// <summary>
    /// Sets the color of this mesh.
    /// </summary>
    public DebugPoint WithColor(Color color)
    {
        ((StandardMaterial3D)_mesh.Material).AlbedoColor = color;
        return this;
    }

    /// <summary>
    /// Sets the scale of this mesh.
    /// </summary>
    public DebugPoint WithScale(float scale)
    {
        _mesh.Radius = scale;
        _mesh.Height = scale * 2;
        return this;
    }

    /// <summary>
    /// Sets the number of rings this mesh has.
    /// </summary>
    public DebugPoint WithRings(int rings)
    {
        _mesh.Rings = rings;
        return this;
    }

    /// <summary>
    /// Sets the number of radial segments this mesh has.
    /// </summary>
    public DebugPoint WithRadialSegments(int segments)
    {
        _mesh.RadialSegments = Mathf.Max(4, segments);
        return this;
    }

    /// <summary>
    /// Updates the text of the label.
    /// </summary>
    public void UpdateText(string text)
    {
        _label.Text = text;
    }

    /// <summary>
    /// Adds this debug point to a child of a <paramref name="parent"/>.
    /// </summary>
    public void ChildOf(Node parent)
    {
        parent.AddChild(_meshInstance);
    }
}
