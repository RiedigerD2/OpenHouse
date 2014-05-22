

The Information for this application is stored in xml files. The files should be stored in PrototypeOne/Resources/Information.
Each file represents a menu where the catagorys in the file represent the divisions of the menu. The format of these files are as follows.

Creating Config Files

 xml version 1.0. 
The root element is ArrayOfCatagory

Fill this structure with Catagory elements
<ArrayOfCatagory xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" >
</ArrayOfCatagory>

once the xml file is open. Go to properties/Schemas and add FormatSchema in Prototype/PrototypeOne
this will alow for text completion and help with finding errors in the format file



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
					BackGroundColor. Images for the BackgroundImage must be stored in in \bin\Release\Resources\Images or \bin\Debug\Resources\Images

	SubCatagoryFile: File name of an xml file in either \bin\Debug\Resources\Information or \bin\Release\Resources\Information
			depending on the run the extension must be included in the name
	Image: string relative path from PrototypeOne/Resources/Images to an image to be displayed 
	
	Video: string relative path from \bin\Debug\Resources\Video or \bin\Release\Resources\Video to an video to be displayed

	CONFIG FILES:

	In the PrototypeOne\Resources\Information directory only one file is Mandatory that is "Top.xml" this serves as the Main menu
all catagories in this file should have a SubCatagoryFile to follow.

all other files in \Information are refered to in a file already existing in the file. eg."Top.xml" as a
 SubCatagoryFile

All the config files, for the menus, are actually read from \bin\Debug\Resources\Information or \bin\Release\Resources\Information inorder for them to be
updated when the files in PrototypeOne\Resources\Information are updated the PrototypeOne.csproj file must be updated. 
Note PrototypeOne.csproj cannot be opened while visual studios has to project open. If you try you will open PrototypeOne.csproj.user which is not the file you need.

When a file is first added to a Visual studio project it first appears in PrototypeOne/PrototypeOne.csproj like so

<Resource Include="Resources\Information\Top.xml" />

simply change that line to look like this

    <Content Include="Resources\Information\Top.xml">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>


Now it will update the correct files everytime you rebuild your solution.

Any file in Resources\Images or Resources\Videos will be stored as resources and do not need this step

LOGING 

 A LogFile is created and stored as \bin\Debug\Log.txt or \bin\Release\Log.txt. To reset this file simply delete it.


LOADING APPLICATION ONTO SURFACE.	
	

	Place the file PrototypeOne in C:\Surface Code Samples\SDKSamples\Core
	The Permisions of the PrototypeOne file must be changed to read and write allowed. Right click the file open properties and deselect ReadOnly.

	create a shortcut of \bin\release\PrototypeOne.xml and place it in C:\ProgramData\Microsoft\Surface\v2.0\Programs



	Inorder to Change the Center Logo replace circle.png and ring.png.

	circle.png is in the center and does not rotate.

	ring.png is a ring around  circle.png the center must be transparent ring.png will rotate