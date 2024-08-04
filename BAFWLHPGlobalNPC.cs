using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossAttackFasterWithLessHP
{
    internal class BAFWLHPGlobalNPC : GlobalNPC
    {
        //this will run after the current AI process has already ran

        public override void PostAI(NPC npc)
        {
            //check if the current npc is the EoC
            if (npc.type == NPCID.EyeofCthulhu)
            {
                //get the current hp percentage in reverse, and round it to the last two decimal places
                float HPpercentage = MathF.Round(1 - npc.GetLifePercent(), 2);

                //set the min and max attack speed values;
                double minATKspeed = 0;
                double maxATKspeed = 3;

                //convert the current HP percentage, to a value between min and max attack speed in float
                float scaledValue = (float)(minATKspeed + HPpercentage * (maxATKspeed - minATKspeed));

                //round it to the last two decimal places
                scaledValue = MathF.Round(scaledValue, 2);

                //add the scaled value to the npc timer to increase the attack speed
                npc.ai[2] += scaledValue;
            }
            base.PostAI(npc);
        }
    }
}
