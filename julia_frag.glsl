#version 400 core

uniform vec2 c;
in vec2 vPosition;
in float vIterations;
uniform float NPower;
uniform float t;

out vec4 aColor;
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
float interpolate(float a, float b, float t){
	return a + (b - a) * t;
}

void main(){
	float mappedPositionX = float(vPosition.x) * 2;
	float mappedPositionY = float(vPosition.y) * 2;
	int n = 0;
	
	vec2 offset = vec2(0.75-t / 10000, 0);
	offset = vec2(0);
	vec2 c_const = CircleInversion(1, vec2(0), vec2(c.y, c.x));
	c_const = c;

	vec2 z = CircleInversion(1, vec2(0), vec2(mappedPositionY, mappedPositionX));
	z = vec2(mappedPositionX, mappedPositionY);
	while(n < vIterations){
		z = Pow(z + offset, 2) + c_const; // (Offset = vec2(0.25, 0))

		if(pow(z.x, 2) + pow(z.y, 2) > 4)
			break;

		n++;
	}
	
	float brightness = sqrt(n / float(vIterations / 16));
	vec4 color = vec4(interpolate(0.224, 0.851, brightness), interpolate(0.075, 0.412, brightness), interpolate(0.318, 0.055, brightness), 1.0);
	if(n == floor(vIterations))
		color = vec4(0.224, 0.075, 0.318, 1.0) / 1.3;

	aColor = color;
}