# Saint Coinach

A .NET library written in C# for extracting game assets and reading game assets from **Final Fantasy XIV**, now with support for including the Libra Eorzea database.

## Functionality 
### Fully implemented

* Extraction of files from the game's SqPack files based on their friendly name (or by Int32 identifiers, if preferred).
* Conversion of the game's textures to `System.Drawing.Image` objects.
* Parsing and reading from the game's data files (`*.exh` and `*.exd`).
* Decoding of OGG files stored in the game's pack files (some of the `*.scd`).
* OO-representation of the most important game data.
* Self-updating of the mapping between game data and their OO-representation, in case things move around inside the game's files betwen patches.

### Partially implemented

* Decoding of the string format used by the game. Will return a good string for most queries, but more advanced things like conditional texts are not supported.
* Parsing of model data works for *most* models but is still incomplete.
* Inclusion and parsing of data from the Libra Eorzea application.

### To-do

* Support for audio formats other than OGG.


## Usage

### Set-up

**Note:** When building an application using this library make sure to include a copy of [`SaintCoinach/SaintCoinach.History.zip`](/SaintCoinach/SaintCoinach.History.zip) in the application's directory. This should be done automatically if the project is included in the solution and referenced from there.

All important data is exposed by the class `SaintCoinach.ARealmReversed`, so setting up access to it is fairly straightforward.

The following is an example using the game's default installation path and English as default language:

```C#
const string GameDirectory = @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn";
var realm = new SaintCoinach.ARealmReversed(GameDirectory, SaintCoinach.Ex.Language.English);
```

It's that simple. It is recommended, however, to check if the game has been updated since the last time, which is accomplished like this, in this example including the detection of data changes:

```C#
if (!realm.IsCurrentVersion) {
    const bool IncludeDataChanges = true;
    var updateReport = realm.Update(IncludeDataChanges);
}
```

`ARealmReversed.Update()` can also take one additional parameter of type `IProgress<UpdateProgress>` to which progress is reported. The returned `UpdateReport` contains a list of changes that were detected during the update.

### Accessing data

Game files can be access directly through `ARealmReversed.Packs`, game data can be accessed through `ARealmReversed.GameData`.

#### Game Data (`XivCollection`)

Specific collections can be retrieved using the `GetSheet<T>()` method.
**Note:** This only works for objects whose game data files have the same name as the class. This applies for most classes directly inside the `SaintCoinach.Xiv` namespace, so there should be no need to worry about it in most cases.

Special cases are exposed as properties:

* `BNpcs`: This collection contains objects of type `BNpc` that include data of both `BNpcBase` and `BNpcName`. *Note:* Only available when Libra Eorzea data is available.
* `ENpcs`: This collection contains objects of type `ENpc` that include data of both `ENpcBase` and `ENpcResident`.
* `EquipSlots`: There is no actual data for specific equipment slots in the game data, but having access to them makes things more convenient, so they're available here.
* `Items`: This collection combines both `EventItem` and `Item`.
* `Shops`: This collection contains all types of shops.

The following is a simple example that outputs the name and colour of all `Stain` objects to the console:

```C#
var stains = realm.GameData.GetSheet<SaintCoinach.Xiv.Stain>();

foreach(var stain in stains) {
    Console.WriteLine("#{0}: {1} is {2}", stain.Key, stain.Name, stain.Color);
}
```

## Notes

### State of documentation

Only `SaintCoinach` contains documentation, and even that only in some places (anything directly in the `SaintCoinach` namespace as well as some things in `SaintCoinach.Xiv.*`), everything else is virtually void of documentation.

There should, however, be enough documentation available to know how to use the library for game data, but figuring out the internal workings might prove difficult.

### SaintCoinach.Cmd

The project `SaintCoinach.Cmd` is a very basic console application that can be used to extract various assets.
The following commands are currently supported:

* `lang`: Displays or changes the language used for data files. Valid arguments are `Japanese`, `English`, `German`, `French`. If no argument is supplied the currently used language is shown.
* Raw file:
  * `raw`: Exports a file from the game assets without any conversions. The argument should be the friendly name of the file.
  * `rawexd` Export all data (default), or only specific data files, seperated by spaces. No post-processing is applied to values. 
* `image`: Exports a file from the game assets as a PNG-image. The argument should be the friendly name of the image file.
* UI Icons:
  * `ui`: Exports one or multiple UI icons as PNG-images. The argument can either be the number of a single UI icon, or the first and last number for a range of icons seperated by a space. Valid numbers are in the interval [0, 999999].
  * `uihd`: As above, but HD icons
* Game Sheets:
  * `exd`: Exports all or a specified number of game data sheets as CSV-files. Arguments can either be empty to export all files, or a list of sheet names seperated by whitespace.
  * `allexd`: As `exd`, but including all languages
  * `allrawexd`: As `exd`, all languages, No post-processing is applied to values.
  * `exdheader`: Export all data headers.                              
  * `rawexd`: Exports all or a specified number of game data sheets as CSV-files without post-processing applied. Arguments can either be empty to export all files, or a list of sheet names seperated by whitespace.
* `bgm`: Exports all sound files referenced in the BGM sheet as OGG-files.
* `maps`: Export all map images. 
* `furn_mdl_tex`: Export all MDL Furniture/Yard files.
* `sql`: Exports the EXD data as SQL schema and associated imports.

#### SaintCoinach.Cmd Setup

To run SaintCoinach.Cmd, you will need to configure where the game install can be found. Either by sending it as an argument when you launch it, e.g:

```powerscript
.\SaintCoinach.Cmd.exe "C:\Steam\steamapps\common\FINAL FANTASY XIV Online"
```

or update the application settings `SaintCoinach.Cmd.dll.config` to set the data path:

```xml
<SaintCoinach.Cmd.Properties.Settings>
  <setting name="DataPath" serializeAs="String">
    <value>C:\Steam\steamapps\common\FINAL FANTASY XIV Online</value>
  </setting>
</SaintCoinach.Cmd.Properties.Settings>
```


# Godbert

Godbert is a simple application to display game data and 3D models from **Final Fantasy XIV** using the above-mentioned library.

## Functionality 
### Fully implemented

* Display of game data/text.
* Rendering of equipment, including ability to dye (if the item supports it.)
* Rendering of monsters.
* Rendering of demi-humans (non-playable races with exchangable equipment.)

### Partially implemented

* Rendering of the game's areas. Only static and completely passive objects are displayed at the moment, anything that's more complex is not included.

### To-Do

* Add more things to show.
* Store active language in config.
