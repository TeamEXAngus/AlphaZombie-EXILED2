using Exiled.Events.EventArgs;

namespace AlphaZombie.Handlers
{
    internal class Dying
    {
        public void OnDying(DyingEventArgs ev)
        {
            //Destroys Alpha Zombie when they die
            if (Functions.IsAlphaZombie(ev.Target))
            {
                Functions.DestroyAlphaZombie(ev.Target);
                Functions.AlphaZombieDeathAnnouce(ev.HitInformation.GetDamageType(), ev.Killer, ev.Target);
            }
        }
    }
}