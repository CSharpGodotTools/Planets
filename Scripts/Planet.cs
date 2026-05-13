using System;
using Godot;

namespace Planets;

public partial class Planet : Node3D
{
    [Export] private PlanetSettings _settings;

    public override void _Ready()
    {
        //GetViewport().DebugDraw = Viewport.DebugDrawEnum.Overdraw;

        ArgumentNullException.ThrowIfNull(_settings);

        Icosahedron icosahedron = new();
        Vector3[] vertices = icosahedron.Vertices;
        int[] indices = icosahedron.Triangles;

        for (int i = 0; i < indices.Length; i += 3)
        {
            Vector3 posA = vertices[indices[i]];
            Vector3 posB = vertices[indices[i + 1]];
            Vector3 posC = vertices[indices[i + 2]];

            AddChild(new MeshInstance3D
            {
                Mesh = ChunkUtils.GenerateMesh(posA, posB, posC, _settings),
                MaterialOverride = new StandardMaterial3D
                {
                    VertexColorUseAsAlbedo = true
                }
            });
        }
    }
}
