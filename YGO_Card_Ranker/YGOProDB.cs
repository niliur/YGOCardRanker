using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace YGO_Card_Ranker
{
    public class YGOProDB
    {

        private SqliteConnection conn;
        public YGOProDB(string fileLoc)
        {
            conn = new SqliteConnection($"Data Source={fileLoc}");
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*) FROM sqlite_master 
                    WHERE TYPE='table' AND (NAME='datas' OR NAME='texts');
            ";
            using(var reader = command.ExecuteReader())
            {
                reader.Read();
                var num_tables = reader.GetInt32(0);
                if(num_tables < 2)
                {
                    throw new Exception("Wrong database");
                }
            }
        }

        public List<YGOCard> SearchCardByName(string name)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT texts.name, texts.desc,datas.id, datas.atk, datas.def, datas.type FROM datas INNER JOIN texts ON datas.id=texts.id WHERE texts.name LIKE '%{name}%'  LIMIT 20;
            ";
            var outputList = new List<YGOCard>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var card = new YGOCard();
                    card.CardName = reader.GetString(0);
                    card.CardText = reader.GetString(1);
                    card.CardCode = (uint)reader.GetInt32(2);
                    card.CardAttack = (uint)reader.GetInt32(3);
                    card.CardDef = (uint)reader.GetInt32(4);
                    card.CardType = (uint)reader.GetInt32(5);
                    outputList.Add(card);
                }
            }
            return outputList;
        }
        public YGOCard GetNextCard(uint gid = 0)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                 SELECT id FROM datas WHERE id>{gid} LIMIT 1;
            ";

            uint nextgid;
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    nextgid = (uint)reader.GetInt32(0);
                }
                else
                {
                    nextgid = 10000;
                }

            }
            return GetCardByGid(nextgid);
        }
        public YGOCard GetPrevCard(uint gid = 0)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT id FROM datas WHERE id<{gid} ORDER BY id DESC LIMIT 1;
            ";

            uint nextgid;
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    nextgid = (uint)reader.GetInt32(0);
                }
                else
                {
                    nextgid = 10000;
                }

            }
            return GetCardByGid(nextgid);
        }


        public YGOCard GetCardByGid(uint gid)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT texts.name, texts.desc,datas.id, datas.atk, datas.def, datas.type FROM datas INNER JOIN texts ON datas.id=texts.id WHERE datas.id={gid};
            ";

            YGOCard card = new YGOCard();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    throw new Exception($"Card with id {gid} not found");
                }
                card.CardName = reader.GetString(0);
                card.CardText = reader.GetString(1);
                card.CardCode = (uint)reader.GetInt32(2);
                card.CardAttack = (uint)reader.GetInt32(3);
                card.CardDef = (uint)reader.GetInt32(4);
                card.CardType = (uint)reader.GetInt32(5);
            }
            return card;
        }
    }
}
