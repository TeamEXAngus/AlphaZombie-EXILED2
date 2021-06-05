using Exiled.API.Interfaces;
using System.ComponentModel;
using System.Collections.Generic;
using Exiled.API.Enums;

namespace AlphaZombie
{
    public sealed class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled on this server.")]
        public bool IsEnabled { get; set; } = true;

        [Description("The scale that should be applied to the Alpha Zombie.")]
        public Dictionary<string, float> AlphaZombieScale { get; set; } = new Dictionary<string, float>
        {
            { "x", 1.2f },
            { "y", 1.2f },
            { "z", 1.2f }
        };

        [Description("The max health of the Alpha Zombie.")]
        public int AlphaZombieMaxHP { get; set; } = 4000;

        [Description("The minimum number of players in the server for an Alpha Zombie to spawn alongisde an SCP-049.")]
        public int MinPlayersForSpawn { get; set; } = 4;

        [Description("The percent chance that an Alpha Zombie will spawn alongside an SCP-049.")]
        public int AlphaZombieSpawnChance { get; set; } = 50;

        [Description("How long in seconds should the game wait before changing the Alpha Zombie's size and HP. Set this to a higher value if scale or HP aren't working.")]
        public float SpawnDelay { get; set; } = 3f;

        [Description("The text that should be shown at the top of the screen when a player spawns as Alpha Zombie. Set to \"none\" to disable.")]
        public string BroadcastOnSpawn { get; set; } = "You are <color=#ff0000>SCP-049-2-A</color>!\nYou are fast and <color=#00ff00>poison</color> your enemies!";

        [Description("The text that should be shown at the top of the screen when a player is hit by Alpha Zombie. Set to \"none\" to disable.")]
        public string BroadcastWhenHit { get; set; } = "You have been<color=#00ff00>poisoned</color>!\nUse <color=#ff0000>SCP-500</color> to cure yourself!";

        [Description("The amount of time that text broadcasts from this plugin should remain on screen.")]
        public ushort BroadcastDuration { get; set; } = 5;

        [Description("Whether or not CASSIE should make an announcement when Alpha Zombie dies.")]
        public bool DeathAnnounce { get; set; } = true;

        [Description("The chance that CASSIE will glitch or stutter when announcing the death of Alpha Zombie.")]
        public float DeathAnnounceGlitchChance { get; set; } = 0.2f;

        [Description("How much damage the Alpha Zombie's attack should do.")]
        public int AlphaZombieAttackDamage { get; set; } = 50;

        [Description("What status effects the Alpha Zombie gives players who it hits.")]
        public List<EffectType> AlphaZombieInflict { get; set; } = new List<EffectType> { EffectType.Poisoned };

        [Description("How much damage should the Alpha Zombie take from decontamination. It's best to set this to 10% of AlphaZombieMaxHP")]
        public int AlphaZombieDecontDamage { get; set; } = 400;

        [Description("Should debug messages be shown in the server console?")]
        public bool DebugMessages { get; set; } = true;
    }
}