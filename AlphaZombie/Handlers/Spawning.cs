using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;
using NorthwoodLib.Pools;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace AlphaZombie.Handlers
{
    internal class Spawning
    {
        public void OnSpawning(SpawningEventArgs ev)
        {
            DestroyAZIfClassChanged(ev.Player);
            TrySpawnAZIf049Spawned(ev.Player);
        }

        private void DestroyAZIfClassChanged(Player Player)
        {
            //Destroys Alpha Zombie if they change class
            if (Player.IsAlphaZombie() && Player.Role != RoleType.Scp0492)
            {
                Player.DestroyAlphaZombie();
            }
        }

        private void TrySpawnAZIf049Spawned(Player Player)
        {
            bool ShouldTrySpawnAZ = Player.Role == RoleType.Scp049 &&
                                    Round.ElapsedTime.TotalSeconds <= 5; //Five seconds chosen arbitrarily
            if (ShouldTrySpawnAZ)
            {
                SpawningAZ(out bool SpawnSuccess);
                Log.Debug($"Failed to spawn Alpha Zombie.", !SpawnSuccess && AlphaZombie.Instance.Config.DebugMessages);
            }
        }

        //Boolean return value states whether a player was spawned or not.
        private void SpawningAZ(out bool Success)
        {
            var PlayerList = ListPool<Player>.Shared.Rent(Player.List);

            bool EnoughPlayers = PlayerList.Count >= AlphaZombie.Instance.Config.MinPlayersForSpawn;
            bool PercentChance = RandomIntInRange(0, 100) <= AlphaZombie.Instance.Config.AlphaZombieSpawnChance;
            bool CanSpawn = EnoughPlayers && PercentChance;

            if (!CanSpawn)
            {
                Success = false;
                return;
            }

            Player NewAlphaZombie = TryChoosePlayer(out bool SuccessfullyChosePlayer, PlayerList);

            if (!SuccessfullyChosePlayer)
            {
                Success = false;
                return;
            }

            //Players are spawned one-by-one, so CallDelayed() prevents players from being set to Alpha Zombie then back to a normal class
            Timing.CallDelayed(1f, () => NewAlphaZombie.SpawnAlphaZombie());

            Success = true;
            return;
        }

        private Player TryChoosePlayer(out bool Success, List<Player> playerList)
        {
            var ListOfPlayersNot049 = playerList.Where(Ply => Ply.Role != RoleType.Scp049).ToList();
            var ListCount = ListOfPlayersNot049.Count;

            if (ListCount == 0)
            {
                Log.Debug("Failed to spawn Alpha Zombie because no players exist which are not SCP-049", AlphaZombie.Instance.Config.DebugMessages);

                Success = false;
                return null;
            }

            Success = true;
            return ListOfPlayersNot049[RandomIntInRange(0, ListCount)];
        }

        private int RandomIntInRange(int min, int max) => Random.Range(min, max + 1); //Unity random has an exclusive max argument
    }
}