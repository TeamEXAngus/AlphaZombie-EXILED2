using Exiled.API.Enums;
using Exiled.Events.EventArgs;

namespace AlphaZombie.Handlers
{
    internal class Hurting
    {
        public void OnHurting(HurtingEventArgs ev)
        {
            //Inflicts status effects and damage, and sends broadcast, when player is hit by Alpha Zombie
            if (ev.Target != ev.Attacker && Functions.IsAlphaZombie(ev.Attacker))
            {
                HitByAlphaZombie(ev);
            }

            //Alpha Zombie Damage Handlers
            if (Functions.IsAlphaZombie(ev.Target))
            {
                AlphaZombieDamageHandlers(ev);
            }
        }

        //Called when a player is hit by Alpha Zombie, sends broadcast and inflicts status effects
        public void HitByAlphaZombie(HurtingEventArgs ev)
        {
            if (AlphaZombie.Instance.Config.BroadcastWhenHit != "none")
            {
                ev.Target.Broadcast(AlphaZombie.Instance.Config.BroadcastDuration, AlphaZombie.Instance.Config.BroadcastWhenHit);
            }

            ev.Amount = AlphaZombie.Instance.Config.AlphaZombieAttackDamage;
            foreach (EffectType effect in AlphaZombie.Instance.Config.AlphaZombieInflict)
            {
                ev.Target.EnableEffect(effect);
            }
        }

        //Called when Alpha Zombie takes damage, prevents coke damage and modifies decont damage
        public void AlphaZombieDamageHandlers(HurtingEventArgs ev)
        {
            //This would look better with a switch statement: Too bad
            if (ev.DamageType == DamageTypes.Scp207)
            {
                ev.IsAllowed = false;
            }
            else if (ev.DamageType == DamageTypes.Decont)
            {
                ev.Amount = AlphaZombie.Instance.Config.AlphaZombieDecontDamage;
            }
        }
    }
}