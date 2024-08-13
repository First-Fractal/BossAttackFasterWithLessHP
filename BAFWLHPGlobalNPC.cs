using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossAttackFasterWithLessHP
{
    internal class BAFWLHPGlobalNPC : GlobalNPC
    {
        //list of all the boss parts to be affected by the system
        static int[] fastBossParts = { NPCID.KingSlime, NPCID.EyeofCthulhu, NPCID.BrainofCthulhu, NPCID.QueenBee, NPCID.SkeletronHead, NPCID.Deerclops, NPCID.WallofFlesh, NPCID.WallofFleshEye, NPCID.QueenSlimeBoss, NPCID.SkeletronPrime, NPCID.Spazmatism, NPCID.Retinazer, NPCID.Plantera, NPCID.Golem, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree, NPCID.HallowBoss, NPCID.DukeFishron, NPCID.CultistBoss, NPCID.MoonLordHead, NPCID.MoonLordHand, NPCID.MoonLordFreeEye };

        //this will run after the current AI process has already ran
        public override void PostAI(NPC npc)
        {
            //check if the current npc is the the list
            if (fastBossParts.Contains(npc.type))
            {
                //ffFunc.Talk("AI[0]: " + npc.ai[0].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("AI[1]: " + npc.ai[1].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("AI[2]: " + npc.ai[2].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("AI[3]: " + npc.ai[3].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[0]: " + npc.localAI[0].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[1]: " + npc.localAI[1].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[2]: " + npc.localAI[2].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[3]: " + npc.localAI[3].ToString(), Microsoft.Xna.Framework.Color.White);

                //get the current hp percentage in reverse, and round it to the last two decimal places
                float HPpercentage = MathF.Round(1 - npc.GetLifePercent(), 2);

                //set the min and max attack speed values;
                double minATKspeed = 0;
                double maxATKspeed = 2;

                //convert the current HP percentage, to a value between min and max attack speed in float
                float scaledValue = (float)(minATKspeed + HPpercentage * (maxATKspeed - minATKspeed));

                //round it to the last two decimal places
                scaledValue = MathF.Round(scaledValue, 2);

                ffFunc.Talk(HPpercentage.ToString(), Microsoft.Xna.Framework.Color.Green);
                ffFunc.Talk(scaledValue.ToString(), Microsoft.Xna.Framework.Color.Red);

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

                //for all boss parts that use localAI[1] as timers
                if (npc.type == NPCID.WallofFleshEye || npc.type == NPCID.Plantera || npc.type == NPCID.KingSlime || npc.type == NPCID.BrainofCthulhu && npc.type == NPCID.Retinazer && npc.type == NPCID.Plantera)
                {
                    npc.localAI[1] += scaledValue;
                }

                ffFunc.Talk(npc.TypeName, Microsoft.Xna.Framework.Color.Orange);
                ffFunc.Talk("----------------------------------------------------", Microsoft.Xna.Framework.Color.White);


            }
            base.PostAI(npc);
        }
    }
}
