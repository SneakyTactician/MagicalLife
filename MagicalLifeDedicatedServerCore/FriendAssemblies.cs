﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MagicalLifeAPITest")]
[assembly: InternalsVisibleTo("MagicalLifeClientStandard")]
[assembly: InternalsVisibleTo("MagicalLifeAPIStandard")]
[assembly: InternalsVisibleTo("MagicalLifeGUIWindows")]
[assembly: InternalsVisibleTo("MagicalLifeModdingAPI")]
[assembly: InternalsVisibleTo("MagicalLifeServerStandard")]
[assembly: InternalsVisibleTo("MagicalLifeSettingsStandard")]

namespace MagicalLifeDedicatedServer
{
    /// <summary>
    /// Used to control who has access to the internal members of this assembly.
    /// </summary>
    public class FriendAssemblies
    {
    }
}