﻿using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace AlphaZombie
{
    public class AlphaZombie : Plugin<Config>
    {
        private static AlphaZombie singleton = new AlphaZombie();
        public static AlphaZombie Instance => singleton;
        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);
        public override Version Version { get; } = new Version(1, 0, 1);

        private Handlers.Dying dying;
        private Handlers.Hurting hurting;
        private Handlers.Spawning spawning;

        public Random random = new Random();

        private AlphaZombie()
        {
        }

        //Run startup code when plugin is enabled
        public override void OnEnabled()
        {
            RegisterEvents();
        }

        //Run shutdown code when plugin is disabled
        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        //Plugin startup code
        public void RegisterEvents()
        {
            dying = new Handlers.Dying();
            hurting = new Handlers.Hurting();
            spawning = new Handlers.Spawning();

            PlayerHandler.Hurting += hurting.OnHurting;
            PlayerHandler.Dying += dying.OnDying;
            PlayerHandler.Spawning += spawning.OnSpawning;
        }

        //Plugin shutdown code
        public void UnregisterEvents()
        {
            PlayerHandler.Hurting -= hurting.OnHurting;
            PlayerHandler.Dying -= dying.OnDying;
            PlayerHandler.Spawning -= spawning.OnSpawning;

            dying = null;
            hurting = null;
            spawning = null;
        }
    }
}