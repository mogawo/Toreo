shader_type canvas_item;

uniform float alpha : hint_range(0, 1) = 0.5;
uniform vec4 color_overlay = vec4(0,0,0,0);

void fragment()
{
	COLOR = texture(TEXTURE, UV);
	COLOR.a = COLOR.a * alpha;
	COLOR.rgba = COLOR.rgba + color_overlay;
}