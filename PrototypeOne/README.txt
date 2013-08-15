Prototype for open house

Creating Config Files

Information for the Menu system is provided by XML files. xml version 1.0. 
The root element is ArrayOfCatagory useing three schemas "http://www.w3.org/2001/XMLSchema" , "http://www.w3.org/2001/XMLSchema-instance"

Fill this structure with Catagory elements
<ArrayOfCatagory xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" >
</ArrayOfCatagory>

once the xml file is open. Go to properties/Schemas and add FormatSchema in Prototype/PrototypeOne
this will alow for text completion and help with finding errors in the format file

Each file represents a menu. where the catagorys in the file represent the divisions of the menu.

Catagory Information
Must be included:
	Ratio: decimal 0-1 The sum of ratios from all Catagory elements must add to 1.The Ratio dictates the size
		of the element relative the the total size of the menu
		However in the "Top.xml" file the ratio does not dictate the size of the division related
		to the catagory. It however dictates the order the divisions will be desplayed in
		eg 0.5 will show up before 0.2. The Ratios in Top.xml file do not have to add to 1

	BackGroundColor: Can be given in byte form 0-255 argb using Elements of the form <A><R><G><B>,
			 or decimal form 0-1 <ScA><ScR><ScG><scB> do not use both at once.
			Only the second group will be interpreted
	Title: The name of the Catagory will be placed in the center of the division Keep this small
Optional Fields

	TextColor: Same set up as BackGroundColor.Defines the color of text for title and Explanation
		   However if it is not included the text color will be the inverse of the BackGroundColor
	
	Explanation: If no SubCatagoryFile is provided when the subdivision related to this cataory
			is selected this explanation will be presented to the user

	BackGroundImage: will set an image as the background for that button. If this is used still need to provide a 
					BackGroundColor. Images for the BackgroundImage must be stored in in \bin\Release\Images or \bin\Debug\Images

	SubCatagoryFile: File name of an xml file in either \bin\Debug\Information or \bin\Release\Information
			depending on the run the extension must be included in the name
	Image: string relative path from PrototypeOne/Resources/Images to an image to be displayed 
	
	Video: string relative path from \bin\Debug\Video or \bin\Release\Video to an video to be displayed

	In the \bin\Release\Information or \bin\release\Information directory only one file is Mandatory that is "Top.xml" this serves as the Main menu
no catagory in the file should have an explanation seeing as it won't be displayed anyways.
all other file in ?\Information are refered to in a file already existing in the file. eg."Top.xml" as a
 SubCatagoryFile

