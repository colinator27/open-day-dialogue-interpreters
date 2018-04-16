if (odd_instance_get_paused(global.odd_instance))
{
	if (keyboard_check_pressed(vk_space))
	{
		if (odd_instance_get_in_choice(global.odd_instance))
		{
			odd_instance_select_choice(global.odd_instance, choiceSelection);
		} else
		{
			odd_instance_resume(global.odd_instance);
		}
	}
	
	if (keyboard_check_pressed(vk_left))
	{
		if (odd_instance_get_in_choice(global.odd_instance))
		{
			choiceSelection--;
			if (choiceSelection < 0)
				choiceSelection = (array_length_1d(choices) - 1);
			displayText = "< " + choices[choiceSelection] + " >";
		}
	}	
	if (keyboard_check_pressed(vk_right))
	{
		if (odd_instance_get_in_choice(global.odd_instance))
		{
			choiceSelection++;
			if (choiceSelection >= array_length_1d(choices))
				choiceSelection = 0;
			displayText = "< " + choices[choiceSelection] + " >";
		}
	}
}