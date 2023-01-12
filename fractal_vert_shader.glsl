#version 330 core

layout (location = 0) in vec2 aPosition;

uniform vec2 offset;
uniform vec2 scale = vec2(3);

out vec2 vPosition;
out vec2 vOffset;
out vec2 vScale;

void main(){
    vOffset = offset;
    vScale = scale;

    vPosition = aPosition;
    gl_Position = vec4(aPosition, 1.0, 1.0);
}