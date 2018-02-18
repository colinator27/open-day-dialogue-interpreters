## Unity interpreter

If you wish to use the Unity interpreter, you only need to copy the `Assets -> Scripts -> Open Day Dialogue` folder/directory.
The rest is example code.

Read the wiki for information on how to set this up for your Unity project.

## Example project

This project comes with an example scene (and code). The example is meant to run on desktop platforms only.

The example binary file, `Assets -> StreamingAssets -> example.opdac`, is generated from the following code:
```
namespace example:
	definitions menu:
		label.control_spacebar="Press spacebar to advance dialogue and select chocies."
		label.control_arrows="Use left and right arrow keys to navigate choices."
	scene test:
		char TestPerson
		"Hello, world!"
		char AnotherPerson
		"It seems that it works?"
		char TestPerson
		"Yay!"
		char Player
		"Hmmm..."
		choice:
			"So, like, how does this work?":
				char TestPerson
				"This dialogue is compiled bytecode!"
				char Player
				"Whoa."
				char AnotherPerson
				"Someone used the Open Day Dialogue compiler to generate it."
				char Player
				"Huh."
			"Can we just end this scene?":
				char TestPerson
				"Of course!"
				exit
			"Anything cool we can do here?":
				char TestPerson
				"Yeah."
				"We can make the background change colors!"
				"Okay, so here goes. I'll change it to red."
				bg red
				char Player
				"Wow."
				"Cool."
				char TestPerson
				"I'll change it to grey now."
				bg grey
				char Player
				"Nice."
				char AnotherPerson
				"Enough of this nonsense!"
				"Back to the original color."
				bg black
		char Player
		"So."
		"Is that it?"
		char TestPerson
		"I guess so."
		"See you around!"
		char AnotherPerson
		"Seeya!"
```
