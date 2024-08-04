using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossAttackFasterWithLessHP
{
    internal class BAFWLHPGlobalNPC : GlobalNPC
    {
        //public static int[] bossParts = {NPCID.Spazmatism, NPCID.Retinazer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree, NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordFreeEye };
        public override void AI(NPC npc)
        {
            //if (ffVar.Bosses.Boss.Contains(npc.type))
            //if (npc.type == bossParts[10])
            if (npc.type == NPCID.Plantera)
            {
                ffFunc.Talk("AI[0]: " + npc.ai[0].ToString(), Microsoft.Xna.Framework.Color.White);
                ffFunc.Talk("AI[1]: " + npc.ai[1].ToString(), Microsoft.Xna.Framework.Color.White);
                ffFunc.Talk("AI[2]: " + npc.ai[2].ToString(), Microsoft.Xna.Framework.Color.White);
                ffFunc.Talk("AI[3]: " + npc.ai[3].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[0]: " + npc.localAI[0].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[1]: " + npc.localAI[1].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[2]: " + npc.localAI[2].ToString(), Microsoft.Xna.Framework.Color.White);
                //ffFunc.Talk("localAI[3]: " + npc.localAI[3].ToString(), Microsoft.Xna.Framework.Color.White);
                ffFunc.Talk(npc.FullName, Microsoft.Xna.Framework.Color.Orange);
                ffFunc.Talk("----------------------------------------------------", Microsoft.Xna.Framework.Color.White);
            }
            base.AI(npc);
        }
    }
}
