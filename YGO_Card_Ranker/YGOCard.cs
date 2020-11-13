using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGO_Card_Ranker
{
    public class YGOCard
    {
        private string cardName;
        private string cardText;
        private uint cardCode;
        private uint cardAttack;
        private uint cardDef;
        private uint cardType;



        public string CardName { get => cardName; set => cardName = value; }
        public string CardText { get => cardText; set => cardText = value; }
        public uint CardCode { get => cardCode; set => cardCode = value; }
        public uint CardAttack { get => cardAttack; set => cardAttack = value; }
        public uint CardDef { get => cardDef; set => cardDef = value; }
        public uint CardType { get => cardType; set => cardType = value; }
        public string CardTypeStr { get => YGOCardTypes.GetCardTypeString(CardType); }
    }
}
