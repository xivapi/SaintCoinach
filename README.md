# Saint Coinach

A library for extracting game assets and reading game assets from **Final Fantasy XIV: A Realm Reborn**.

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
* Parsing of model data works for *most* models but is far from complete.

### To-do

* Ability to specify the location of the file that contains mapping and history data.
* Inclusion of data from the Libra Eorzea application.
* Support for audio formats other than OGG.


## Usage

### Set-up

**Note:** When building an application using this library make sure to include a copy of [`SaintCoinach/SaintCoinach.History.zip`](https://github.com/Rogueadyn/SaintCoinach/blob/master/SaintCoinach/SaintCoinach.History.zip]) in the application's directory.

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

`ARealmReversed.Update()` can also take one additional parameter of type `IProgress<UpdateProgress>` to which progress is reported. `UpdateReport` contains a list of changes that were detected during the update.

### Usage

Game files can be access directly through `ARealmReversed.Packs`, game data can be accessed through `ARealmReversed.GameData`.

#### Game Data (`XivCollection`)

Specific collections can be retrieved using the `GetSheet<T>()` method. **Note:** This only works for objects whose game data files have the same name as the class, this applies for most classes directly inside the `SaintCoinach.Xiv` namespace.

Special cases are exposed as properties:

* `ENpcs`: This collections contains objects of type `ENpc` that include data of both `ENpcBase` and `ENpcResident`.
* `EquipSlots`: There is no actual data for specific equipment slots in the game data, but having access to them makes things more convenient, so they're available here.
* `Items`: This collection combines both `EventItem` and `Item`.
* `Shops`: This collection contains all types of shops.

## Notes

### State of documentation

Only `SaintCoinach` contains documentation, and even that only in some places (anything directly in the `SaintCoinach` namespace as well as some things in `SaintCoinach.Xiv.*`), everything else is virtually void of documentation.

There should, however, be enough documentation available to know how to use the library for game data, but figuring out the internal workings might prove difficult.
