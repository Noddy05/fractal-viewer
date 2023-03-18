#version 330 core

in vec2 vCoordinates;

out vec4 color;
uniform sampler2D texture_sampler;

void main()
{
	vec4 texture_color = texture(texture_sampler, vCoordinates);
    color = texture_color;
}