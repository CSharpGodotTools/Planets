using Godot;

namespace Planets;

[GlobalClass]
public partial class PlanetSettings : Resource
{
    [Export] public FastNoiseLite Noise { get; set; } = new() { Frequency = 0.003f };
    [Export] public float NoiseStrength { get; set; } = 1000f;
    [Export] public float Radius { get; set; } = 10f;
    [Export] public int Resolution { get; set; } = 128;
}
