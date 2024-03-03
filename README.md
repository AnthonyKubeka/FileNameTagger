I wrote this tool to initially help me tag video files with specific tags (like a title, or groups of tags like genres etc) by adding these tags to the FileName. 
The tool allows you to select a file, which will display its tag name (and if it's a video file, it will try to read some of the properties) and then allow you to tag this 
according to the tags defined in a tag template you've loaded. 
There are a few examples in the repository, but essentially, these are just JSON files defining how you want to create your new file name. 

The types of tags are 
1. Text - Just text, creates an input box on the GUI.
2. TextList - A list of items that can be 'checked'. Creates a list of these with checkboxes and add / delete buttons on the GUI.
3. Enum - Creates a dropdown of editable and selectable values. You can only select one.
4. Date - Creates a date picker with a given date.

This *can* be generalized beyond video files. I'm still thinking of uses! But if you need to organize random clips downloaded online and what the organisation to be in the file name, give this a go!
