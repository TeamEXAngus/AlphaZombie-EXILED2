using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace AlphaZombie.Commands
{
    //Command to turn the chosen player into an Alpha Zombie
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AlphaZombie : ICommand
    {
        public string Command { get; } = "SpawnAlphaZombie";

        public string[] Aliases { get; } = { "spawnalphazombie", "saz" };

        public string Description { get; } = "Turns the chosen plauer into an Alpha Zombie.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            //Permission checking
            if (!(sender as CommandSender).CheckPermission("az.spawn"))
            {
                response = "You do not have permission to use ths command! Missing permission \"az.spawn\"";
                return false;
            }

            //If no player specified
            if (arguments.Count != 1)
            {
                response = "Incorrect usage! Usage: SpawnAlphaZombie <player>";
                return false;
            }

            Player TargetPlayer = Player.Get(arguments.At(0));

            //Make sure that the chosen player exists
            if (TargetPlayer == null)
            {
                response = "You entered an invalid player!";
                return false;
            }

            TargetPlayer.SpawnAlphaZombie();

            response = $"Succesfully spawned {TargetPlayer.Nickname} as Alpha Zombie!";
            return true;
        }
    }
}