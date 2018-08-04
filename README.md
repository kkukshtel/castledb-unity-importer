# CastleDB Unity Importer
*Requires Unity 2018 + Scripting Runtime 4.X*  
Define all your data in CastleDB and then directly access your data in code in Unity with full intellisense support!

## Why
*Read about this here*  
Unity doesn't provide an easy way to globally manage game data, so this plugin is an attempt to fix that. CastleDB was built for this explicit purpose, but until now has only had an easy interop with games made in Haxe. This plugin is meant to provide Unity devs with not only CastleDB support but also the ability to use the database like the original Haxe API.

## What is CastleDB?
From the [CastleDB](http://castledb.org/) site:  
CastleDB is used to input structured static data. Everything that is usually stored in XML or JSON files can be stored and modified with CastleDB instead.

For instance, when you are making a game, you can have all of your items and monsters including their names, description, logic effects, etc. stored in CastleDB.

## Getting Started
To use this plugin, download and import the latest Unity package from [the releases page here](). 
The example scene comes with a CastleDB file to test with, but you'll also need to download and install [CastleDB]() so you can work with it further.

When you import the package, you should see a folder appear in your Assets/ folder as well that wasn't in the package. This is the code generated from the default CastleDB file. **Any time you make a change or reimport your .cdb file, this code will be auto-generated**. The intention of this plugin is to be mostly invisible, so after you've imported the package and this intial generation runs, you're good to go!

Try adding in new sheets/rows/columns via CastleDB to the default provided CastleDB file to get a feel for how the code generation works. Also be sure to check out the Limitations section below for some general guidelines on how to use CastleDB with Unity.

## Config File
A config file is provided that allows you to define the general behavior of the type generation by allowing you to set custom values for three diffferent fields:
* **GUIDColumnName** is the name of the column you use in each sheet to define how you want the item to be accessed.
* **GeneratedTypesLocation** is the folder, relative to Assets, that you want to put the generated types in. I.e. a location of "Cool Stuff/Generated Types" would put the types in Assets/Cool Stuff/Generated Types.
* **GeneratedTypesNamespace** is the namespace you want to use for the types you generate. This is important because you'll need to use this namespace to access the types you generated. Ideally you have a project namespace you already use and you can just set this value to that so you don't need any additional using statements.

## How this plugin works  
By leveraging Unity's AssetImporter API, this plugin makes it so that Unity understands and recgonizes a CastleDB file (.cdb). Upon a .cdb file being imported or updated, this plugin generates (or regenerates) types from the sheets/rows/columns in your database.  

Because this does code generation, it's important to understand what is happening and structure your database accordingly. The main concepts employed are:
* A **sheet** is considered a **Type**
* A **column** in a sheet is considered the sheet's **fields**
* A **row** in a sheet is considered an **instance** of the sheet's Type.

## Limitations  
* All sheets need to have some sort of GUID column that defines the name of a row. This defaults to "id" with a string type.
* Objects are currently created at runtime and do not share direct internal references, so circular references of any kind will likely break object creation. Don't make a line in a sheet with a reference field that points to a line in another sheet with a reference field that points to an object in the first sheet.
* A current annoyance is that if you add more columns or rows to a given type, the solution in Unity will properly update but you're need to reload your solution in your editor. This manifests as previous mentions of your type in code saying "Can't be found". To fix this, just reload the assembly. In VSCode, this is as easy as just clicking your .sln file at the bottom of the window and reselecting it.
* Currently there is no validation of column names, so column names in CastleDB need to be valid field names in C#. I.E. don't use spaces in your column names, weird characters, etc.
* The above item also applies to text in whatever you decide to call your GUID column because these names become strongly typed. Because of this, make your GUID name something easy to remember/use, and have an additional text column that is the actual name of your row. 


## TODO  
* Add in Color support
* Need to add reference fields in to create new types of that reference.
* Figure out a better guide for adding CastleDB Custom Trypes
* Document way to add in your own CustomType to match with a predefined Type in Unity
* Currently do not have file types or Image types implemented. This would require some preconfiguration steps that seem unique to every user so I'm not sure if it should be added.
* CastleDB also has a map/level creator that is not used at all here. Could be interesting to implement.

## Custom Types  

## References
[CastleDB Wedbsite](http://castledb.org/)
[Best CastleDB Tutorial/Walkthrough](https://translate.google.com/translate?sl=auto&tl=en&js=y&prev=_t&hl=en&ie=UTF-8&u=http%3A%2F%2Fhaxeflixel.2dgames.jp%2Findex.php%3FCastleDB%252FHaxe&edit-text=)
https://docs.unity3d.com/Manual/ScriptedImporters.html
http://castledb.org/
http://www.gamefromscratch.com/post/2015/11/14/CastleDB-A-Game-Database-A-Level-Editor-Its-Both.aspx
https://github.com/ncannasse/castle
http://wiki.unity3d.com/index.php/SimpleJSON