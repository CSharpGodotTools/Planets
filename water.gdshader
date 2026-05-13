shader_type spatial;

uniform vec3 albedo : source_color = vec3(0.1, 0.3, 0.5);
uniform vec3 albedo_fresnel : source_color = vec3(0.5, 0.8, 1.0);
uniform float roughness : hint_range(0.0, 1.0) = 0.1;

uniform sampler2D wave; 
uniform sampler2D texture_normal_1;
uniform sampler2D texture_normal_2;
uniform vec2 wave_direction_1 = vec2(1.0, 0.5);
uniform vec2 wave_direction_2 = vec2(-0.5, 1.0);
uniform float time_scale : hint_range(0.0, 0.2) = 0.025;
uniform float noise_scale = 1.0;

varying vec3 world_pos;
varying vec3 local_normal;

float fresnel(float amount, vec3 normal, vec3 view) {
    return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}

vec3 triplanar_texture(sampler2D p_sampler, vec3 p_weights, vec3 p_pos) {
    vec3 samp = vec3(0.0);
    samp += texture(p_sampler, p_pos.xy).rgb * p_weights.z;
    samp += texture(p_sampler, p_pos.xz).rgb * p_weights.y;
    samp += texture(p_sampler, p_pos.zy).rgb * p_weights.x;
    return samp;
}

void vertex() {
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
    local_normal = NORMAL;
}

void fragment() {
    vec3 blending = abs(local_normal);
    blending /= (blending.x + blending.y + blending.z);
    
    vec3 tri_pos = world_pos * noise_scale;
    vec3 n1 = triplanar_texture(texture_normal_1, blending, tri_pos + vec3(TIME * time_scale * wave_direction_1, 0.0));
    vec3 n2 = triplanar_texture(texture_normal_2, blending, tri_pos + vec3(TIME * time_scale * wave_direction_2, 0.0));
    
    float f_factor = fresnel(5.0, NORMAL, VIEW);
    vec3 surface_color = mix(albedo, albedo_fresnel, f_factor);

    ALBEDO = surface_color;
    NORMAL_MAP = mix(n1, n2, 0.5);
    ROUGHNESS = roughness;
    SPECULAR = 0.2; 
}