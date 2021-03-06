using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

namespace AlphaZombie
{
    internal static class Functions
    {
        //Turns a player into an Alpha Zombie
        public static void SpawnAlphaZombie(this Player player)
        {
            player.Position = Exiled.API.Extensions.Role.GetRandomSpawnPoint(RoleType.Scp049);
            player.SetRole(RoleType.Scp0492, true);
            player.Items.Clear(); //Must clear items because in this case, SetRole doesn't remove them
            player.SessionVariables.Add("IsAlphaZombie", true);

            if (AlphaZombie.Instance.Config.BroadcastOnSpawn != "none")
                player.Broadcast(AlphaZombie.Instance.Config.BroadcastDuration, AlphaZombie.Instance.Config.BroadcastOnSpawn);

            Timing.CallDelayed(AlphaZombie.Instance.Config.SpawnDelay, () =>
            {
                var scale = AlphaZombie.Instance.Config.AlphaZombieScale;
                player.Scale = new UnityEngine.Vector3(scale["x"], scale["y"], scale["z"]);

                player.EnableEffect(EffectType.Scp207);
                player.MaxHealth = AlphaZombie.Instance.Config.AlphaZombieMaxHP;
                player.Health = player.MaxHealth;
            });
        }

        //Stops a player from being an Alpha Zombie
        public static void DestroyAlphaZombie(this Player player)
        {
            player.SessionVariables.Remove("IsAlphaZombie");
            player.Scale = UnityEngine.Vector3.one;
            player.DisableEffect(EffectType.Scp207);
        }

        //Announces the death of an Alpha Zombie through CASSIE
        public static void AlphaZombieDeathAnnouce(DamageTypes.DamageType damageType, Player killer, Player target)
        {
            if (!AlphaZombie.Instance.Config.DeathAnnounce)
                return;

            const string Name = "SCP 0 4 9 2 nato_a";

            if (killer != target)
            {
                if (killer.Team == Team.MTF)
                {
                    AnnounceUsingCassie($"{Name} succesfully terminated . Termination unit {UnitNameToCassieWords(killer.UnitName)}");
                    return;
                }

                AnnounceUsingCassie($"{Name} succesfully terminated by {killer.Role}");
                return;
            }

            //Can't use switch statement; 'DamageTypes' are not constant values
            if (damageType == DamageTypes.Decont)
            {
                AnnounceUsingCassie($"{Name} lost in decontamination sequence");
                return;
            }

            if (damageType == DamageTypes.Tesla)
            {
                AnnounceUsingCassie($"{Name} succesfully terminated by automatic security system");
                return;
            }

            if (damageType == DamageTypes.Nuke)
            {
                AnnounceUsingCassie($"{Name} terminated by alpha warhead");
                return;
            }

            AnnounceUsingCassie($"{Name} terminated . termination cause unspecified");
        }

        //Sends a CASSIE announcement with the configured glitch chance
        public static void AnnounceUsingCassie(string message)
        {
            Cassie.GlitchyMessage(message, AlphaZombie.Instance.Config.DeathAnnounceGlitchChance, AlphaZombie.Instance.Config.DeathAnnounceGlitchChance);
        }

        //Turns Player.UnitName into a CASSIE-readable string
        public static string UnitNameToCassieWords(this string unit) => $"nato_{unit[0]} {unit.Substring(unit.Length - 2)}";

        //Self explanatory, for code readability
        public static bool IsAlphaZombie(this Player player) => player.SessionVariables.ContainsKey("IsAlphaZombie");
    }
}