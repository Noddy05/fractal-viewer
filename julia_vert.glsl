#version 400 core

layout (location = 0) in vec2 aPosition;

uniform int iterations;

out vec2 vPosition;
out float vIterations;

void main(){
    vIterations = iterations;

    vPosition = aPosition;
    gl_Position = vec4(aPosition, 1.0, 1.0);
}