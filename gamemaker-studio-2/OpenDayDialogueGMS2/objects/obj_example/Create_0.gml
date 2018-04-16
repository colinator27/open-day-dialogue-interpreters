global.odd_binary = odd_load_binary("example.opdac");
global.odd_variables = ds_map_create();
global.odd_instance = 
	odd_create_instance(
		global.odd_binary,
		scr_example_variable_set,
		scr_example_variable_get,
		scr_example_variable_cleanup,
		scr_example_handle_text,
		scr_example_handle_choice
	);
odd_instance_command_add(global.odd_instance, "char", scr_example_cmd_char);
odd_instance_command_add(global.odd_instance, "bg", scr_example_cmd_bg);
odd_instance_command_add(global.odd_instance, "exit", scr_example_cmd_exit);
	
displayText = "";
choiceSelection = 0;
choices = undefined;
bgColor = c_black;

odd_instance_run_scene(global.odd_instance, "example.test");