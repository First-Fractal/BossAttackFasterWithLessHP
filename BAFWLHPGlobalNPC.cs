using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossAttackFasterWithLessHP
{
    internal class BAFWLHPGlobalNPC : GlobalNPC
    {
        //list of all the boss parts to be affected by the system
        static int[] fastBossParts = { NPCID.KingSlime, NPCID.EyeofCthulhu, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.BrainofCthulhu, NPCID.QueenBee, NPCID.SkeletronHead, NPCID.Deerclops, NPCID.WallofFlesh, NPCID.WallofFleshEye, NPCID.QueenSlimeBoss, NPCID.TheDestroyerBody, NPCID.SkeletronPrime, NPCID.Spazmatism, NPCID.Retinazer, NPCID.Plantera, NPCID.Golem, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree, NPCID.HallowBoss, NPCID.DukeFishron, NPCID.CultistBoss, NPCID.MoonLordHead, NPCID.MoonLordHand, NPCID.MoonLordFreeEye };

        //function for scaleing a value bassed on the hp values
        public float scaleValueBassedOnHP(float hp, float maxHP)
        {
            //get the current hp percentage in reverse, and round it to the last two decimal places
            float HPpercentage = MathF.Round(1 - hp/maxHP, 2);

            //set the min and max attack speed values;
            double minATKspeed = 0;
            double maxATKspeed = 2;

            //convert the current HP percentage, to a value between min and max attack speed in float
            float scaledValue = (float)(minATKspeed + HPpercentage * (maxATKspeed - minATKspeed));

            //round it to the last two decimal places
            scaledValue = MathF.Round(scaledValue, 2);

            return scaledValue;
        }

        //public value for getting the max EoW HP
        public static int EoWMaxHP = 0;

        //function for getting the total hp of the EoW
        public int getEoWTotalHP()
        {
            //set the hp to 0
            int hp = 0;

            //go through every npc in the world
            foreach (NPC n in Main.npc)
            {
                //check if the current npc is a body piece of EoW, and is not dead
                if ((n.type == NPCID.EaterofWorldsHead || n.type == NPCID.EaterofWorldsBody || n.type == NPCID.EaterofWorldsTail) && n.active)
                {
                    //appent the npc life to the total pool hp
                    hp += n.life;
                }
            }

            //return the hp
            return hp;
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            //if the current npc that just spawn in is the tail, then that mean all of the EoW body peices are spawn in
            if (npc.type == NPCID.EaterofWorldsTail)
            {
                //set the current max hp to the total hp of the EoW
                EoWMaxHP = getEoWTotalHP();
            }
            base.OnSpawn(npc, source);
        }

        //this will run after the current AI process has already ran
        public override void PostAI(NPC npc)
        {

            //check if the current npc is the the list
            if (fastBossParts.Contains(npc.type))
            {
                //get the scaled value bassed on the hp of the boss
                float scaledValue = scaleValueBassedOnHP(npc.life, npc.lifeMax);

                //for all boss parts that use AI[1] as timers
                if (npc.type == NPCID.QueenBee || npc.type == NPCID.Deerclops || npc.type == NPCID.WallofFlesh ||
                    npc.type == NPCID.QueenSlimeBoss || npc.type == NPCID.Golem || npc.type == NPCID.GolemFistLeft ||
                    npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead || npc.type == NPCID.GolemHeadFree ||
                    npc.type == NPCID.HallowBoss || npc.type == NPCID.CultistBoss || npc.type == NPCID.MoonLordHead ||
                    npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordFreeEye)
                {
                    //dont break eol's ai when she goes into phase 2
                    if (npc.type == NPCID.HallowBoss && npc.ai[0] == 10) return;
                    //make queen bee only affect the states that used the timer
                    if (npc.type == NPCID.QueenBee && (npc.ai[0] != 1 && npc.ai[0] != 3)) return;
                    npc.ai[1] += scaledValue;
                }

                //for all boss parts that use AI[2] as timers
                if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.SkeletronHead ||
                    npc.type == NPCID.SkeletronPrime || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer ||
                    npc.type == NPCID.GolemHead || npc.type == NPCID.GolemHeadFree || npc.type == NPCID.DukeFishron ||
                    npc.type == NPCID.MoonLordHead)
                {
                    npc.ai[2] += scaledValue;
                }

                //increase the firerate of fireball and lasers for the twins
                if (npc.ai[1] == 0 && (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer))
                {
                    npc.ai[3] += scaledValue;
                }

                //for the destroyer body that has a timer
                if (npc.type == NPCID.TheDestroyerBody) npc.localAI[0] += scaledValue;

                //for the EoW, just speed him up
                if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                {
                    //get the current hp of the EoW
                    int hp = getEoWTotalHP();
                    
                    //scale the value based on the current hp left
                    scaledValue = scaleValueBassedOnHP(hp, EoWMaxHP);

                    //speed up the EoW based on the hp % and current velocity
                    npc.position += (npc.velocity * scaledValue);
                }
                //for all boss parts that use localAI[1] as timers
                if (npc.type == NPCID.WallofFleshEye || npc.type == NPCID.Plantera || npc.type == NPCID.KingSlime || npc.type == NPCID.BrainofCthulhu && npc.type == NPCID.Retinazer && npc.type == NPCID.Plantera)
                {
                    npc.localAI[1] += scaledValue;
                }

                ffFunc.Talk(NPCID.Search.GetName(npc.type), Microsoft.Xna.Framework.Color.Orange);
                ffFunc.Talk("----------------------------------------------------", Microsoft.Xna.Framework.Color.White);
            }
            base.PostAI(npc);
        }
    }
}
