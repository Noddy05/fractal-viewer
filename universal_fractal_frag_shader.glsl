#version 330 core

in vec2 vPosition;
in vec2 vOffset;
in vec2 vScale;
in float vIterations;

out vec4 aColor;

float interpolate(float a, float b, float t){
	return a + (b - a) * t;
}
float cosInterpolate(float a, float b, float t){
	return interpolate(a, b, (1 - cos(t * 3.141)) / 2);
}
int factorial(int i)
{
    int product = 1;
    for (int j = 2; j <= i; j++)
    {
        product *= j;
    }
    return product;
}
float raiseToPower(float a, int b)
{
    float product = 1;
    for (int i = 0; i < b; i++)
    {
        product *= a;
    }
    return product;
}

void main(){
	//aColor = vec4((vPosition.x + 1) / 2, (vPosition.y + 1) / 2, 1.0, 1.0);
	float mappedPositionX = float(vPosition.x / 2 * vScale.x + vOffset.x);
	float mappedPositionY = float(vPosition.y / 2 * vScale.y + vOffset.y);
	float a = mappedPositionX;
	float b = mappedPositionY;

	int n = 0;
	int power = 2;
	while(n < vIterations){
		float newA = mappedPositionX;
		float newB = mappedPositionY;

		for(int k = 0; k <= power; k++){
            int multiplier = 1;
            for (int i = 0; i < k; i++)
            {
                if(i % 2 == 1)
                    multiplier *= -1;
            }

            if (k % 2 == 0)
                newA += factorial(power) / (factorial(k) * factorial(power - k)) * raiseToPower(a, power - k) * raiseToPower(b, k) * multiplier;
            else
                newB += factorial(power) / (factorial(k) * factorial(power - k)) * raiseToPower(a, power - k) * raiseToPower(b, k) * multiplier;
		}

		a = newA;
		b = newB;

		if(a * a + b * b > 4)
			break;
		n++;
	}

	float brightness = cosInterpolate(0, 1.5, cosInterpolate(0, 1, sqrt(sqrt(n / float(vIterations)))));
	vec4 color = vec4(interpolate(0.224, 0.851, brightness), interpolate(0.075, 0.412, brightness), interpolate(0.318, 0.055, brightness), 1.0);
	if(n == vIterations)
		color = vec4(0.224, 0.075, 0.318, 1.0) / 1.3;

	float sqrMagnitude = mappedPositionX * mappedPositionX + mappedPositionY * mappedPositionY;
	float sqrMagnitudeOffset = (mappedPositionX + 0.5) * (mappedPositionX + 0.5) + mappedPositionY * mappedPositionY;
	float sqrMagnitudeOffset2 = (mappedPositionX + 1) * (mappedPositionX + 1) + mappedPositionY * mappedPositionY;
	float radius = 1 / 4.0;
	float strokeWeight = 0.005;
	if(sqrMagnitude <= radius * radius && sqrMagnitude >= radius * radius - strokeWeight * radius)
		color = vec4(1);

	if(sqrMagnitudeOffset <= radius * radius && sqrMagnitudeOffset >= radius * radius - strokeWeight * radius)
		color = vec4(1);

	if(sqrMagnitudeOffset2 <= radius * radius && sqrMagnitudeOffset2 >= radius * radius - strokeWeight * radius)
		color = vec4(1);

	aColor = color;
}