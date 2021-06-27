using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;
using NorthwoodLib.Pools;
using UnityEngine;

namespace AlphaZombie.Handlers
{
    internal class Spawning
    {
        public void OnSpawning(SpawningEventArgs ev)
        {
            //Destroys Alpha Zombie if they change class
            if (Functions.IsAlphaZombie(ev.Player) && ev.Player.Role != RoleType.Scp0492)
            {
                Functions.DestroyAlphaZombie(ev.Player);
            }

            bool ShouldTrySpawnAZ = ev.Player.Role == RoleType.Scp049 &&
                                    Round.ElapsedTime.TotalSeconds <= 5; //Five seconds chosen arbitrarily
            if (ShouldTrySpawnAZ)
            {
                SpawningAZ(out bool SpawnSuccess);
                Log.Debug("Failed to spawn Alpha Zombie.", !(SpawnSuccess) && AlphaZombie.Instance.Config.DebugMessages);
            }
        }

        //Boolean return value states whether a player was spawned or not.
        private void SpawningAZ(out bool Success)
        {
            var playerList = ListPool<Player>.Shared.Rent(Player.List); //Definitely not ripped from Stalky106
            int playerCount = playerList.Count;

            bool enoughPlayers = playerCount >= AlphaZombie.Instance.Config.MinPlayersForSpawn;
            bool percentChance = Random.Range(0, 100 + 1) <= AlphaZombie.Instance.Config.AlphaZombieSpawnChance;
            bool canSpawn = enoughPlayers && percentChance;
            if (!canSpawn)
            {
                Success = false;
                return;
            }

            Player newAlphaZombie = playerList[RandomIntInRange(0, playerCount)];
            int tries = 0;

            //Over-engineered code segment prevents Alpha Zombie from replacing the SCP-049 that spawned it
            while (newAlphaZombie.Role == RoleType.Scp049)
            {
                newAlphaZombie = playerList[RandomIntInRange(0, playerCount)];

                //Will prevent the server from hanging in the event that only SCP-049s exist
                //Five loops is chosen aribtrarily, perhaps I was thinking about five a lot when I wrote this code
                if (tries++ > 5)
                {
                    Success = false;
                    return;
                }
            }

            //Players are spawned one-by-one, so CallDelayed() prevents players from being set to Alpha Zombie then back to a normal class
            Timing.CallDelayed(1f, () => Functions.SpawnAlphaZombie(newAlphaZombie));

            Success = true;
            return;
        }

        private int RandomIntInRange(int min, int max) => Random.Range(min, max + 1); //Unity random has an exclusive max argument
    }
}