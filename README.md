# CastleDB Unity Importer
Define all your data in the external CastleDB software and enjoy macro-like intellisense for your data inside your code editor

## How it works

## Implementation

## Limitations
* Currently there is no validation of column names, so column names in CastleDB need to be valid field names in C#. I.E. don't use spaces in your column names, weird characters, etc.
* All sheets need to have some sort of GUID column that defines the name of a row. This defaults to "id" with a string type.


## TODO
* WORKING ON LIST SUPPORT
* Easier way to add in SimpleJSON than a DLL? Unity's AssemblyBuilder won't take the SimpleJSON reference unless it's passed as a DLL reference. Maybe a namespace issue?
* Add in List support
* Add in Color support
* Figure out a better guide for adding CastleDB Custom Trypes
* Document way to add in your own CustomType to match with a predefined Type in Unity
* Rebuilt DLL doesn't have new columns
* Unclear how references are supposed to work in CastleDB