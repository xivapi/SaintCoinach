using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {


    //From old client constructors were in the format Client::LayoutEngine::Layer::BgPartsLayoutInstance::`vftable'


    public enum LgbEntryType : int {
        BgParts = 1,
        //Keep this for backwards compatability
        Model = 1,
        Light = 3,
        Vfx = 4,
        PositionMarker = 5,
        Gimmick = 6,
        SharedGroup6 = 6,// secondary variable is set to 2
        Sound = 7,
        EventNpc = 8,
        BattleNpc = 9,
        Aetheryte = 12,
        EnvSpace = 13,
        Gathering = 14,
        SharedGroup15 = 15,// secondary variable is set to 13
        Treasure = 16,
        Weapon = 39,
        PopRange = 40,
        ExitRange = 41,
        MapRange = 43,
        NaviMeshRange = 44,
        EventObject = 45,
        EnvLocation = 47,
        EventRange = 49,
        QuestMarker = 51,
        CollisionBox = 57,
        DoorRange = 58,
        LineVfx = 59,
        ClientPath = 65,
        ServerPath = 66,
        GimmickRange = 67,
        TargetMarker = 68,
        ChairMarker = 69,
        ClickableRange = 70,
        PrefetchRange = 71,
        FateRange = 72,
        SphereCastRange = 75,

    }
}
