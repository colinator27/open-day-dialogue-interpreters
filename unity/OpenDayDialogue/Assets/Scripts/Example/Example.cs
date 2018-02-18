using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour 
{
	public Text dialogueText;
	public Text controlsText;
	public Camera mainCamera;

	OpenDayDialogue.Binary binary;
	OpenDayDialogue.Interpreter interpreter;
	string currentChar = "";
	bool inChoice = false;
	bool waitingForInput = false;
	List<string> choices;
	int selectedChoice;

	void Start () 
	{
		// Load the binary
		using(System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, "example.opdac"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
		{
			binary = new OpenDayDialogue.Binary(fs);
		}

		// Start the interpreter
		OpenDayDialogue.CommandHandler cmdHandler = new OpenDayDialogue.CommandHandler();
		cmdHandler.AddNewCommand("char", x => currentChar = x[0].valueRawIdentifier);
		cmdHandler.AddNewCommand("exit", x => interpreter.StopScene());
		cmdHandler.AddNewCommand("bg", 
			x => { 
				switch(x[0].valueRawIdentifier){ 
					case "red": 
						mainCamera.backgroundColor = Color.red; 
						break; 
					case "grey": 
						mainCamera.backgroundColor = Color.grey; 
						break; 
					case "black": 
						mainCamera.backgroundColor = Color.black; 
						break; 
				} 
			});
		interpreter = new OpenDayDialogue.Interpreter(binary, new ExampleVariableStore(), new OpenDayDialogue.FunctionHandler(), cmdHandler,
			
			// Handle text
			x => {
				dialogueText.text = currentChar + ": " + x;
				waitingForInput = true; 
			},
		
			// Handle choices
			x => {
				dialogueText.text = "< " + x[0] + " >";
				choices = new List<string>(x);
				selectedChoice = 0;
				inChoice = true;
				waitingForInput = true;
			});

		// Get control text
		controlsText.text = interpreter.GetDefinition("example.menu.label.control_spacebar") + "\n" + interpreter.GetDefinition("example.menu.label.control_arrows");

		// Run the scene
		interpreter.RunScene("example.test");
	}
	
	void Update () 
	{
		if(waitingForInput)
		{
			if(inChoice)
			{
				if(Input.GetKeyDown(KeyCode.RightArrow))
				{
					if(selectedChoice < choices.Count - 1)
					{
						selectedChoice++;
					} else
					{
						selectedChoice = 0;
					}
				}
				if(Input.GetKeyDown(KeyCode.LeftArrow))
				{
					if(selectedChoice > 0)
					{
						selectedChoice--;
					} else
					{
						selectedChoice = choices.Count - 1;
					}
				}
				if(Input.GetKeyDown(KeyCode.Space))
				{
					interpreter.SelectChoice(selectedChoice);
					dialogueText.text = "";
					waitingForInput = false;
					inChoice = false;
				}
				dialogueText.text = "< " + choices[selectedChoice] + " >";
			} else
			{
				if(Input.GetKeyDown(KeyCode.Space))
				{
					interpreter.Resume();
					dialogueText.text = "";
					waitingForInput = false;
				}
			}
		}
		interpreter.Update();
	}
}

public class ExampleVariableStore : OpenDayDialogue.VariableStore
{
	Dictionary<string, OpenDayDialogue.Value> variables = new Dictionary<string, OpenDayDialogue.Value>();

	public override void SetVariable(string name, OpenDayDialogue.Value value)
	{
		variables[name] = value;
	}

	public override OpenDayDialogue.Value GetVariable(string name)
	{
		return variables[name];
	}

	public override void CleanUp()
	{
		variables.Clear();
	}
}