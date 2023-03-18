#version 330 core

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aCoordinate;

out vec2 vCoordinates;

void main()
{
    vCoordinates = aCoordinate;
    gl_Position = vec4(aPosition, 1.0, 1.0);
}