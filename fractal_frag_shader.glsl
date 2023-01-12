#version 330 core

in vec2 vPosition;
in vec2 vOffset;
in vec2 vScale;

out vec4 aColor;

void main(){
	//aColor = vec4((vPosition.x + 1) / 2, (vPosition.y + 1) / 2, 1.0, 1.0);

	vec2 mappedPosition = vec2(vPosition) / 2 * vScale + vOffset;
	float a = mappedPosition.x;
	float b = mappedPosition.y;

	int n = 0;
	while(n < 100){
		
		float newA = a * a - b * b + mappedPosition.x;
		float newB = 2 * a * b + mappedPosition.y;

		a = newA;
		b = newB;

		if(a * a + b * b > 4)
			break;

		n++;
	}

	float brightness = sqrt(n / 100.0);
	vec4 color = vec4(brightness + 0.16591251885, brightness / 2 + 0.05731523378, 0.43740573152 - brightness, 1.0);
	if(n == 100)
		color = vec4(0.16591251885, 0.05731523378, 0.43740573152, 1.0);

	aColor = color;
}