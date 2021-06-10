using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;
using NorthwoodLib.Pools;

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

            //Spawning Alpha Zombie with SCP-049-2
            if (ev.Player.Role == RoleType.Scp049 && Round.ElapsedTime.TotalSeconds <= 5) //Five seconds chosen arbitrarily
            {
                var spawnSuccess = SpawningAZ();
                Log.Debug("Failed to spawn Alpha Zombie.", !(spawnSuccess) && AlphaZombie.Instance.Config.DebugMessages);
            }
        }

        //Boolean return value states whether a player was spawned or not.
        public bool SpawningAZ()
        {
            var playerList = ListPool<Player>.Shared.Rent(Player.List); //Definitely not ripped from Stalky106
            int playerCount = playerList.Count;

            //If enough players online && random chance based on configs
            if (playerCount >= AlphaZombie.Instance.Config.MinPlayersForSpawn && AlphaZombie.Instance.random.Next(0, 100) <= AlphaZombie.Instance.Config.AlphaZombieSpawnChance)
            {
                Player newAlphaZombie = playerList[AlphaZombie.Instance.random.Next(0, playerCount)];
                int tries = 0;

                //Over-engineered code segment prevents Alpha Zombie from replacing the SCP-049 that spawned it
                while (newAlphaZombie.Role == RoleType.Scp049)
                {
                    newAlphaZombie = playerList[AlphaZombie.Instance.random.Next(0, playerCount)];

                    //Will prevent the server from hanging in the event that only SCP-049s exist
                    if (tries++ > 5) { return false; } //Five loops is chosen aribtrarily, perhaps I was thinking about five a lot when I wrote this code
                }

                //Players are spawned one-by-one, so CallDelayed() prevents players from being set to Alpha Zombie then back to a normal class
                Timing.CallDelayed(1f, () =>
                {
                    Functions.SpawnAlphaZombie(newAlphaZombie);
                });

                return true;
            }

            return false;
        }
    }
}