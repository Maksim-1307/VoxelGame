#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec3 light;

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    vec4 color = texture(texture0, texCoord);
    if (color.a == 0.0) discard;
    color = mix(color, vec4(0.0, 0.0, 0.0, 1.0), light.r);
    outputColor = color;
}