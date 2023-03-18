#version 330 core

in vec2 vPosition;
in vec2 vOffset;
in vec2 vScale;
in float vIterations;

uniform mat4 position;

float maxDistance = 100;

out vec4 aColor;
float epsilon = 0.001;

vec3 spheres[] = vec3[](
	vec3(0, 0, 3)
);
vec3 boxes[] = vec3[](
	vec3(-2, 5, 5)
);

float estimateDistanceSphere(vec3 rayPosition, vec3 sphere, float radius){
	return length(sphere - rayPosition) - radius;
}

float estimateDistanceBox(vec3 box, float radius){
	vec3 b = vec3(5, 5, 1);
	vec3 q = abs(box) - b;
	return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

vec3 calculateNormalSphere(vec3 rayPosition, vec3 closest, float radius){
	float distance = estimateDistanceSphere(rayPosition, closest, radius);
	float distanceA = estimateDistanceSphere(rayPosition + vec3(epsilon, 0, 0), closest, radius);
	float distanceB = estimateDistanceSphere(rayPosition + vec3(0, epsilon, 0), closest, radius);
	float distanceC = estimateDistanceSphere(rayPosition + vec3(0, 0, epsilon), closest, radius);

	return (vec3(distanceA, distanceB, distanceC) - distance) / epsilon;
}

void main(){
	vec3 direction = normalize((vec4(0, 0, 1, 0) * (position)).xyz);
	
	vec3 rayPosition = direction / 100;

	vec4 sunlightPosition = normalize(vec4(normalize(vec3(-500, 500, 0)), 0));
	
	float radius = 0.2;
	vec4 color = vec4(1);

	float distanceTravelled = 0;
	for(int march = 0; march < 1000; march++){
		if(distanceTravelled > maxDistance)
			break;

		float minDistance = 100;
		vec3 normal = vec3(0);
		for(int i = 0; i < spheres.length; i++){
			float distance = estimateDistanceSphere(rayPosition, spheres[i], radius);

			if(minDistance > distance){
				minDistance = distance;
				//normal = calculateNormalSphere(rayPosition, (vec4(spheres[i], 1)).xyz, radius);
				//normal = (vCRotation * vec4(normal, 1)).xyz;
			}
		}

		distanceTravelled += minDistance;
		rayPosition += direction * minDistance;
		
		if(minDistance < epsilon)
		{
			float sunlight = dot(normal, normalize(rayPosition-sunlightPosition.xyz));
			color = vec4(normal, 1);
			break;
		}

	}

	aColor = color;
}