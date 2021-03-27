Project requirements:
.Net Framework above v4.x
Unity 2019.1.7f1 or newer
Game uses Unity text or TextMeshPro

Features:
Works on Windows and Mac
Easy installation
Text to speech functionality
Supports voices installed on the system 
Supports Unity text and TextmeshPro
Limited support for reading through render to texture

Integration into the project:
Import the plugin into the project
Make sure the .Net Framework in the player settings is above v4.x or the game will build but DiSLECTEK will not work.
If textMeshPro is in your project everything should em ok but if you are just using unitys built in text  you will have to comment out  a line in “TTS_Interface.cs” so it looks like this, neer the top of this script change “define TextMeshPro” to “#define TextMeshPro” Save the file and you should be good to go.
Quick install, add the TTSPrefab to the root scene of your game. The prefab is a DoNotDestroyOnLoad object, so will be available from when it’s loaded for the first time. 
Options menu install (check sample scene for example), EnableDisablePlugin.cs , use the functions “enablePlugin()” “disablePlugin()”. “togglePlugin()” Can be attached to buttons. Add the script to the button, then in the button script add a OnClick event, drag the same object into the field in the bottom left of the onClick filed then in the dropdown on the right select enableDisablePlugin/togglePlugin. If using “togglePlugin()” you can set default on or off in the inspector buby toggling the bool “IsPluginActive”.
You can configure a few of options in the inspector with the prefab selected in the scene
“Disable With Esc” toggle weather escape can exit the overlay 
“Key Binding ” Change the key bindings, ‘T’ Key toggles the TTS UI by default. There are options to change the button and have multiple buttons.
“Pause All Audio” toggles weather game audio will pause when the overlay is active.

Sample scene:
This scene demonstrates most of the options.
Take a look at how the render texture is set up.
Taken a look at how to enable and disable DiSLECTEK

Some technical notes:
Time is scaled to 0 when the TTS UI is active and reset to its original value 
The keyboard inputs are not blocked when the speech UI is activated. Developers need to do this themselves. There are events available as follows:
	o extractTextOnClick.TTSActivated()
	o extractTextOnClick.TTSDeactivated()
	o There is a Boolean: extractTextOnClick.UIActive which also gives the current activated state of the TTS system.

Text baked into images:
This step requires a little more set up but can be used for infrequent cases  like wall graffiti or custom logos baked into images.
Create a text object over the object you with to be readable
Scale adjust the transform to match the size of the object
Set the text color to fully transparent
Type intended text in the text field
This should work now

Render to Texture:
We have basic support for render to texture at this time. We know that some games use render to texture for some ui so wanted some support for this but a full implementation of this. We are looking into this but it will require more work from the developer and make this less of a plugin and play solution. Probably assigning scripts to render to texture objects in the scene.
It should just work for simple examples already
The problem is that it uses all the camera coordinates at the same time, ie if text overlaps in the same area of the cameras view it will read them both

End user flow:
The user will press the “T” key or the button or whatever you remapped it too, and the ui overlay will animate in. 
They can then move the mouse over any text and click to hear it spoken aloud.
The rate of speech , volume and voice selected can be selected in the options menu.
Change whether the text continues to read when the menu is closed, allowing them to continue playing while listening to text.
Choose whether the game pauses when the menu is active to give more time to hear all the options.

