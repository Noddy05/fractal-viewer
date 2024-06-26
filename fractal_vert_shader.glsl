﻿#version 400 core

layout (location = 0) in vec2 aPosition;

uniform vec2 offset;
uniform vec2 scale = vec2(3);
uniform float iterations;

out vec2 vPosition;
out vec2 vOffset;
out vec2 vScale;
out float vIterations;

out mat4 vCPosition;
out mat4 vCRotation;

void main(){
    vOffset = offset;
    vScale = scale;
    vIterations = iterations;

    vPosition = aPosition;
    gl_Position = vec4(aPosition, 1.0, 1.0);
}