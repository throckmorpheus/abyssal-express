shader_type canvas_item;

uniform float timeScaleFactor = .04;

uniform sampler2D noise_texture: repeat_enable, filter_linear;
uniform sampler2D gradient;

uniform float brightness: hint_range(0.0, 3.0, 0.01) = 1.0;
uniform float clouds_resolution: hint_range(0.0, 10.0, 0.01) = 3.0;
uniform float clouds_intesity: hint_range(-0.06, 0.0, 0.001) = 0.0;
uniform float waveyness: hint_range(0.0, 10.0, 0.01) = 0.5;
uniform float fragmentation: hint_range(0.0, 100.0, 0.01) = 7.0;
uniform float distortion: hint_range(0.0, 10.0, 0.01) = 0.5;
uniform float clouds_alpha: hint_range(0.4, 0.6, 0.01) = 0.5;
uniform float movement: hint_range(0.7, 1.3, 0.01) = 1.0;
uniform float blur: hint_range(0.0, 10.0, 0.01) = 1.4;
uniform float blur2: hint_range(0.0, 0.01, 0.00001) = 0.01;

float localTime() {
	return TIME * timeScaleFactor;
}

mat2 makem2(float theta) {
	float c = cos(theta);
	float s = sin(theta);
	return mat2(vec2(c, -s), vec2(s, c));
}

float noise_x(vec2 x) {
	return texture(noise_texture, x * blur2).x;
}
float noise_y(vec2 y) {
	return texture(noise_texture, y * blur2).y;
}

vec2 gradn(vec2 p) {
	float ep = .09;
	float gradx = noise_x(vec2(p.x + ep, p.y)) - noise_x(vec2(p.x - ep, p.y));
	float grady = noise_y(vec2(p.x, p.y + ep)) - noise_y(vec2(p.x, p.y - ep));
	return vec2(gradx, grady);
}

float flow(vec2 p) {
	float z = 2.;
	float rz = clouds_intesity;
	vec2 bp = p;

	for(float i = 1.; i < 7.; ++i) {
		p += localTime() * .6;
		bp += localTime() * 5.9;
		vec2 gr = gradn(i * p * .34 + localTime() * 1.);
		gr *= makem2(localTime() * 6. - (.05 * p.x + .03 * p.y) * 90.);
		p += gr * waveyness;
		rz += (sin(noise_x(p) * fragmentation) * distortion + clouds_alpha) / z;
		p = mix(bp, p, movement);
		z *= blur;
		p *= 2.;
		bp *= 1.9;
	}
	return rz;
}

float rand(vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,72.233))) * 53758.5453123);
}



void fragment() {
	vec2 resolution = 1.0 / SCREEN_PIXEL_SIZE;
	vec2 fragCoord = FRAGCOORD.xy;
	fragCoord = vec2(fragCoord.x, fragCoord.y - resolution.y );

	vec2 p = fragCoord / resolution - 0.5;

	p.x *= resolution.x / resolution.y;

	p *= clouds_resolution;

	float value = 0.1 / max(flow(p), 0.);
	value = pow(value, brightness);

	COLOR = texture(gradient, vec2(value, value));
}