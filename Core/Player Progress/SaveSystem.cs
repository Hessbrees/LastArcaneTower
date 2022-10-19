using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SavePlayer(PlayerDataScructure player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fog";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, player);
        stream.Close();

    }

    public static PlayerDataScructure LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fog";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerDataScructure data = formatter.Deserialize(stream) as PlayerDataScructure;
            stream.Close();
            return data;
        }
        return null;
    }

}
