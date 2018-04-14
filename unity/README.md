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
		TestPerson: "Hello, world!"
		AnotherPerson: "It seems that it works?"
		TestPerson: "Yay!"
        
        // Label for jumping back to later
        example_test_hmm:
        
		Player: "Hmmm..."
		choice:
			"So, like, how does this work?":
				TestPerson: "This dialogue is compiled bytecode!"
				Player: "Whoa."
				AnotherPerson: "Someone used the Open Day Dialogue compiler to generate it."
				Player: "Huh."
			"Can we just end this scene?":
				TestPerson: "Of course!"
				exit
            "Can we jump to me talking for the first time?":
                TestPerson: "Yeah!"
                : example_test_hmm
			"Anything cool we can do here?":
				TestPerson: "Yeah."
				 "We can make the background change colors!"
				 "Okay, so here goes. I'll change it to red."
				bg red
				Player: "Wow."
				 "Cool."
				TestPerson: "I'll change it to grey now."
				bg grey
				Player: "Nice."
				AnotherPerson: "Enough of this nonsense!"
				 "Back to the original color."
				bg black
		Player: "So."
		 "Is that it?"
		TestPerson: "I guess so."
		 "See you around!"
		AnotherPerson: "Seeya!"
```
