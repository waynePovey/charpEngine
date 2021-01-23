#version 460

in vec3 rectColor;

out vec4 FragColor;

void main()
{
	FragColor = vec4(rectColor, 1.0);
}