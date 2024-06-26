﻿#version 400 core

in vec2 vPosition;
in vec2 vOffset;
in vec2 vScale;
in float vIterations;
uniform float NPower;

out vec4 aColor;
uniform float t;

const float e = 2.71828182846;
const float pi = 3.14159265359;
vec2 Add(vec2 z1, vec2 z2){
	vec2 a = vec2(z1.x, z1.y);
	vec2 b = vec2(z2.x, z2.y);
	
	return a + b;
}
vec2 Sub(vec2 z1, vec2 z2){
	vec2 a = vec2(z1.x, z1.y);
	vec2 b = vec2(z2.x, z2.y);
	
	return a - b;
}
vec2 Mult(vec2 z1, vec2 z2){
	float r1 = length(z1);
	float theta1 = atan(z1.y, z1.x);
	float r2 = length(z2);
	float theta2 = atan(z2.y, z2.x);
	float newA = r1 * r2 * cos(theta1 + theta2);
	float newB = r1 * r2 * sin(theta1 + theta2);
	return vec2(newA, newB);
}
vec2 Div(vec2 z1, vec2 z2){
	float r1 = length(z1);
	float theta1 = atan(z1.y, z1.x);
	float r2 = length(z2);
	float theta2 = atan(z2.y, z2.x);
	float newA = r1 / r2 * cos(theta1 - theta2);
	float newB = r1 / r2 * sin(theta1 - theta2);
	return vec2(newA, newB);
}
vec2 Pow(vec2 z, float power){
	float r = length(z);
	float theta = atan(z.y, z.x);
	float newA = pow(r, power) * cos(theta * power);
	float newB = pow(r, power) * sin(theta * power);
	return vec2(newA, newB);
}
vec2 Pow(float c, vec2 z){
	float a = z.x;
	float b = z.y;
	float newA = cos(b * log(c));
	float newB = sin(b * log(c));

	return pow(c, a) * vec2(newA, newB);
}
vec2 EPow(vec2 z){
	float a = z.x;
	float b = z.y;
	float newA = cos(b);
	float newB = sin(b);

	return pow(e, a) * vec2(newA, newB);
}
vec2 Pow(vec2 z1, vec2 z2){
	float r1 = length(z1);
	float theta1 = atan(z1.y, z1.x);
	float r2 = length(z2);
	float theta2 = atan(z2.y, z2.x);

	vec2 rPart = Pow(r1, r2 * vec2(cos(theta2), sin(theta2)));
	vec2 ePart = Pow(r1, theta1 * r2 * Mult(EPow(vec2(9, theta2)), vec2(0, 1)));
	return Mult(rPart, ePart);
}
vec2 Sin(vec2 z){
	vec2 negativeConjugate = Mult(z, vec2(0, 1));
	return Mult(vec2(0, -0.5), Sub(EPow(negativeConjugate), EPow(-negativeConjugate)));
}
vec2 Cos(vec2 z){
	vec2 negativeConjugate = Mult((z + pi / 2.0), vec2(0, 1));
	return Mult(vec2(0, -0.5), Sub(EPow(negativeConjugate), EPow(-negativeConjugate)));
}
vec2 Tan(vec2 z){
	return Sin(z) / Cos(z);
}
vec2 CircleInversion(float radius, vec2 center, vec2 z){
	vec2 fromCenterToZ = (z - center);
	return fromCenterToZ * (radius * radius) 
		/ (fromCenterToZ.x * fromCenterToZ.x + fromCenterToZ.y * fromCenterToZ.y);
}

float interpolate(float a, float b, float v){
	return a + (b - a) * v;
}

void main(){
	float newT = 0.2 * 3.14;
	//aColor = vec4((vPosition.x + 1) / 2, (vPosition.y + 1) / 2, 1.0, 1.0);
	float mappedPositionX = float(vPosition.x / 2 * vScale.x + vOffset.x);
	float mappedPositionY = float(vPosition.y / 2 * vScale.y + vOffset.y);
	int n = 0;

	vec2 offset = vec2(0);
	vec2 c = vec2(mappedPositionX, mappedPositionY);
	//c = CircleInversion(1, vec2(0, 0), c);

	vec2 z = c;
	while(n <= floor(vIterations)){
        //z = Add(Mult(z, Add(z * cos(t), Sin(z) * sin(t))), c);
		//z = Add(Pow(z, 2), Div(tan(t) + Mult(vec2(0, 1), c), Add(vec2(0, 1), c * tan(t))));
		//z = Pow(z, 2) + Pow(vec2(0, 1) / tan(t) + c, -1) + Pow(Pow(c, -1) + tan(t) * Pow(vec2(0, 1), -1), -1);
		//z = Pow(z, 2) + c; // (Offset = vec2(0.25, 0))
		z = Pow(z + offset, 2) + c;

		if(pow(z.x, 2) + pow(z.y, 2) > 4)
			break;

		n++;
	}

	//float brightness = max(0, sqrt((n - log(log(sqrt(z.x * z.x + z.y * z.y)) / log(2)) / log(2)) / (vIterations - n)) * 2);
	float brightness = pow(n / vIterations * 2, 0.66) * 2;
	vec4 color = vec4(interpolate(0.224, 0.851, brightness), interpolate(0.075, 0.412, brightness), interpolate(0.318, 0.055, brightness), 1.0);
	if(n - 1 == floor(vIterations))
		color = vec4(0.224, 0.075, 0.318, 1.0) / 1.3;

	aColor = color;
}