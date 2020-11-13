using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGO_Card_Ranker
{
    public static class YGOCardTypes
    {
        public const uint Monster = 1;
        public const uint Spell = 2;
        public const uint Trap = 4;
        public const uint Irregular = 8;
        public const uint NormalMonster = 16;
        public const uint EffectMonster = 32;
        public const uint FusionMonster = 64;
        public const uint RitualMonster = 128;
        public const uint TrapMonster = 256;
        public const uint SpiritMonster = 512;
        public const uint UnionMonster = 1024;
        public const uint Gemini = 2048;
        public const uint Tuner = 4096;
        public const uint Synchro = 8192;
        public const uint Token = 16384;
        public const uint Quickplay = 65536;
        public const uint Continuous = 131072;
        public const uint Equip = 262144;
        public const uint Field = 524288;
        public const uint Counter = 1048576;
        public const uint Flip = 2097152;
        public const uint Toon = 4194304;
        public const uint XYZ = 8388608;
        public const uint Pendulum = 16777216;
        public const uint Link = 67108864;

        private static uint BitwiseAnd(uint a, uint b)
        {
            return a & b;
        }

        public static string GetCardTypeString(uint typeCode)
        {
            List<string> adjectives = new List<string>();
            string noun = "";
            if (BitwiseAnd(typeCode, Monster) > 0)
            {
                noun = "Monster";
            }
            if (BitwiseAnd(typeCode, Spell) > 0)
            {
                noun = "Spell";
            }
            if (BitwiseAnd(typeCode, Trap) > 0)
            {
                noun = "Trap";
            }
            if (BitwiseAnd(typeCode, Irregular) > 0)
            {
                noun = "Irregular";
            }
            if (BitwiseAnd(typeCode, NormalMonster) > 0)
            {
                noun = "NormalMonster";
            }
            if (BitwiseAnd(typeCode, EffectMonster) > 0)
            {
                noun = "EffectMonster";
            }
            if (BitwiseAnd(typeCode, FusionMonster) > 0)
            {
                noun = "FusionMonster";
            }
            if (BitwiseAnd(typeCode, RitualMonster) > 0)
            {
                noun = "RitualMonster";
            }
            if (BitwiseAnd(typeCode, TrapMonster) > 0)
            {
                noun = "TrapMonster";
            }
            if (BitwiseAnd(typeCode, SpiritMonster) > 0)
            {
                noun = "SpiritMonster";
            }
            if (BitwiseAnd(typeCode, UnionMonster) > 0)
            {
                noun = "UnionMonster";
            }
            if (BitwiseAnd(typeCode, Gemini) > 0)
            {
                adjectives.Add("Gemini");
            }
            if (BitwiseAnd(typeCode, Tuner) > 0)
            {
                adjectives.Add("Tuner");
            }
            if (BitwiseAnd(typeCode, Synchro) > 0)
            {
                adjectives.Add("Synchro");
            }
            if (BitwiseAnd(typeCode, Token) > 0)
            {
                adjectives.Add("Token");
            }
            if (BitwiseAnd(typeCode, Quickplay) > 0)
            {
                adjectives.Add("Quickplay");
            }
            if (BitwiseAnd(typeCode, Continuous) > 0)
            {
                adjectives.Add("Continuous");
            }
            if (BitwiseAnd(typeCode, Equip) > 0)
            {
                adjectives.Add("Equip");
            }
            if (BitwiseAnd(typeCode, Field) > 0)
            {
                adjectives.Add("Field");
            }
            if (BitwiseAnd(typeCode, Counter) > 0)
            {
                adjectives.Add("Counter");
            }
            if (BitwiseAnd(typeCode, Flip) > 0)
            {
                adjectives.Add("Flip");
            }
            if (BitwiseAnd(typeCode, Toon) > 0)
            {
                adjectives.Add("Toon");
            }
            if (BitwiseAnd(typeCode, XYZ) > 0)
            {
                adjectives.Add("XYZ");
            }
            if (BitwiseAnd(typeCode, Pendulum) > 0)
            {
                adjectives.Add("Pendulum");
            }
            if (BitwiseAnd(typeCode, Link) > 0)
            {
                adjectives.Add("Link");
            }

            string output = "";
            if (adjectives.Count > 0)
            {
                output += String.Join(" ", adjectives);
                output += " ";
            }
            return output += noun;
            
        }
    }
}
