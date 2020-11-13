using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGO_Card_Ranker
{
    public class YGORankDB
    {

        private SqliteConnection conn;
        public YGORankDB(string fileLoc)
        {
            conn = new SqliteConnection($"Data Source={fileLoc}");
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*) FROM sqlite_master 
                    WHERE TYPE='table' AND (NAME='rankedcards');
            ";
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var num_tables = reader.GetInt32(0);
                if (num_tables < 1)
                {
                    throw new Exception("Wrong database");
                }
            }
        }

        public uint GetNextUnRanked(uint gid=0)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT id FROM rankedcards WHERE ranking=0 AND id>{gid} LIMIT 1;
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
            return nextgid;
        }
        public uint GetPrevUnRanked(uint gid = 0)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT id FROM rankedcards WHERE ranking=0 AND id<{gid} ORDER BY id DESC LIMIT 1;
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
            return nextgid;
        }


        public void SetRankByGid(uint gid, int rank)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                INSERT OR REPLACE INTO rankedcards (id, ranking)
                    VALUES({gid}, {rank});
            ";
            command.ExecuteNonQuery();
            
        }

        public int GetRankByGid(uint gid)
        {
            var command = conn.CreateCommand();
            command.CommandText = $@"
                SELECT ranking FROM rankedcards WHERE id={gid};
            ";

            int rank = 0;
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                rank = reader.GetInt32(0);

            }
            return rank;
        }
    }
}
