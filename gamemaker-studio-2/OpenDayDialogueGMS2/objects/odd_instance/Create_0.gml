binary = undefined;
variableStore_setVariable = undefined;
variableStore_getVariable = undefined;
variableStore_cleanUp = undefined;
functionHandlers = ds_map_create();
commandHandlers = ds_map_create();
handleText = undefined;
handleChoice = undefined;
textMainProcessor = undefined;
textChoiceProcessor = undefined;
textDefinitionProcessor = undefined;

programCounter = 0;
stack = ds_stack_create();
currentScene = undefined;
currentText = undefined;
currentChoicesLabels = ds_list_create();
currentChoicesTexts = ds_list_create();
inChoice = false;
pause = false;
debugCurrentLine = -1;