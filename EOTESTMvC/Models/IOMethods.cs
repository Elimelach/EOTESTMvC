using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Models
{
    public static class IOMethods
    {
        private static string GetOrCreataRoute()
        {
            string cur = Directory.GetCurrentDirectory();
            string root = Directory.GetDirectoryRoot(cur);
            string path = @$"{root}\EO\SavedAuth\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string file = "codes.txt";
            if (!File.Exists(path+file))
            {
                File.Create(path + file);
            }
            return path + file;
        }
        public static Tokens ReadCodeFile()
        {
            FileStream stream = new FileStream(GetOrCreataRoute(), FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader streamReader = new StreamReader(stream);
            Tokens tokens = null;
            while (streamReader.Peek()!= -1)
            {
                var splited = streamReader.ReadLine().Split('|');
                tokens = new Tokens
                {
                    AuthorizationCode = splited[0],
                    RelmId = splited[1],
                    AccessToken = splited[2],
                    AccessToken_exp = DateTime.Parse(splited[3]),
                    RefreshToken = splited[4],
                    RefreshToken_exp = DateTime.Parse(splited[5])
                };
            }
            streamReader.Close();
            return tokens;
        }
        public static void WriteTokon(Tokens tokens)
        {
            FileStream stream = new FileStream(GetOrCreataRoute(), FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(tokens.AuthorizationCode + "|");
            writer.Write(tokens.RelmId + "|" );
            writer.Write(tokens.AccessToken + "|");
            writer.Write(tokens.AccessToken_exp.ToString() + "|");
            writer.Write(tokens.RefreshToken + "|");
            writer.WriteLine(tokens.RefreshToken_exp.ToString());
            writer.Close();


        }
        public static bool IsValid100Token()
        {
            Tokens tokens = ReadCodeFile();
            if (tokens == null) return false;
            if (tokens.RefreshToken_exp > DateTime.Now) return true;
            return false;
        }
        public static void DeleteAuth()
        {
            File.Delete(GetOrCreataRoute());
        }
    }
}
