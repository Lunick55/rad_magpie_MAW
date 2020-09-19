using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SavePlayer(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player" + player.stats.playerNumber + ".coconut";
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}
	
	public static void SaveCoconuts(List<CoconutData> coconuts)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/coconuts.coconut");
		FileStream stream = new FileStream(path, FileMode.Create);

		CoconutSaveData data = new CoconutSaveData(coconuts);

		formatter.Serialize(stream, data);
		stream.Close();
	}	
	
	public static void SaveLevel1(Level1Manager level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/level1.level");
		FileStream stream = new FileStream(path, FileMode.Create);

		Level1Manager.Level1Data data = new Level1Manager.Level1Data(level);

		formatter.Serialize(stream, data);
		stream.Close();
	}	
	public static void SaveLevel2(Level2Manager level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/level2.level");
		FileStream stream = new FileStream(path, FileMode.Create);

		Level2Data data = new Level2Data(level);

		formatter.Serialize(stream, data);
		stream.Close();
	}	
	public static void SaveLevel3(LevelBossManager level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/level3.level");
		FileStream stream = new FileStream(path, FileMode.Create);

		LevelBossData data = new LevelBossData(level);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static PlayerData LoadPlayer(int playerIndex)
	{
		string path = Application.persistentDataPath + "/player" + playerIndex + ".coconut";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			PlayerData data = formatter.Deserialize(stream) as PlayerData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}
	public static CoconutSaveData LoadCoconuts()
	{
		string path = Application.persistentDataPath + string.Format("/coconuts.coconut");

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			CoconutSaveData data = formatter.Deserialize(stream) as CoconutSaveData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}
	public static Level1Manager.Level1Data LoadLevel1()
	{
		string path = Application.persistentDataPath + ("/level1.level"); ;

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			Level1Manager.Level1Data data = formatter.Deserialize(stream) as Level1Manager.Level1Data;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}
	public static CoconutSaveData LoadLevel2()
	{
		string path = Application.persistentDataPath + string.Format("/coconuts.coconut");

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			CoconutSaveData data = formatter.Deserialize(stream) as CoconutSaveData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}
	public static CoconutSaveData LoadBossLevel()
	{
		string path = Application.persistentDataPath + string.Format("/coconuts.coconut");

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			CoconutSaveData data = formatter.Deserialize(stream) as CoconutSaveData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}

	//just a testing thing for myself. don't even know if it'll work
	public static T LoadData<T>(string fileName) where T : Data
	{
		string path = Application.persistentDataPath + string.Format("/{0}.coconut", fileName);

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			T data = formatter.Deserialize(stream) as T;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return default(T);
		}
	}
}

public class Data
{

}