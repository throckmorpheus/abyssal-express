shader_type canvas_item;

float rand(vec2 st) {
	
    return fract(sin(dot(st.xy, vec2(12.9898,72.233))) * 53758.5453123);
}

void fragment() {
	vec2 resolution = 1.0 / SCREEN_PIXEL_SIZE;
	vec2 fragCoord = FRAGCOORD.xy;
	fragCoord = vec2(fragCoord.x, fragCoord.y - resolution.y );
	
	float stars = 0.0;
	
	if (rand(SCREEN_UV.xy / 20.0) > 0.995) //0.996
	{
		float r = rand(SCREEN_UV.xy);
		stars = r * (0.5 * sin(TIME * (r * 2.0) + 1000.0 * r) + 0.5);
	}

    COLOR = vec4(stars, stars, stars, 1.0);
}