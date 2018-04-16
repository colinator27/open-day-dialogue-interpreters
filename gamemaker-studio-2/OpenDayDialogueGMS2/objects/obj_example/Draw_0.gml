draw_set_color(bgColor);
draw_rectangle(0, 0, room_width, room_height, false);

draw_set_color(c_white);
draw_set_font(fnt_arial);
draw_set_halign(fa_left);
draw_set_valign(fa_top);

if (odd_instance_get_current_scene(global.odd_instance) != undefined)
	draw_text(300, 300, displayText);
else
	draw_text(300, 300, "Completed!");
	
draw_text(300, 500, scr_example_get("example.menu.label.control_spacebar"));
draw_text(300, 540, scr_example_get("example.menu.label.control_arrows"));