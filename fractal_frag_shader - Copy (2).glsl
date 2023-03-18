#version 400 core

in vec2 vPosition;
in vec2 vOffset;
in vec2 vScale;
in float vIterations;
in float vPower;

out vec4 aColor;

float interpolate(float a, float b, float t){
	return a + (b - a) * t;
}
float cosInterpolate(float a, float b, float t){
	return interpolate(a, b, (1 - cos(t * 3.141)) / 2);
}

void main(){
	//aColor = vec4((vPosition.x + 1) / 2, (vPosition.y + 1) / 2, 1.0, 1.0);
	float mappedPositionX = float(vPosition.x / 2 * vScale.x + vOffset.x);
	float mappedPositionY = float(vPosition.y / 2 * vScale.y + vOffset.y);
	float a = mappedPositionX;
	float b = mappedPositionY;
	
    float N = 2.5;
    float r = sqrt(a * a + b * b);
    float theta = atan(b, a);
    float maxDistance = pow(2, 1 / (N - 1));

	int n = 0;
	while(n < vIterations){
        a = pow(r, N) * cos(N * theta) + mappedPositionX;
        b = pow(r, N) * sin(N * theta) + mappedPositionY;
        r = sqrt(a * a + b * b);
        theta = atan(b, a);

		if(a * a + b * b > 4)
			break;

		n++;
	}

	float brightness = cosInterpolate(0, 1.5, cosInterpolate(0, 1, sqrt(sqrt(n / float(vIterations)))));
	vec4 color = vec4(interpolate(0.224, 0.851, brightness), interpolate(0.075, 0.412, brightness), interpolate(0.318, 0.055, brightness), 1.0);
	if(n == vIterations)
		color = vec4(0.224, 0.075, 0.318, 1.0) / 1.3;

	aColor = color;
}