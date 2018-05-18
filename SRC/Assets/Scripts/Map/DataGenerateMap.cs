using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Map
{
	[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData")]
	public class DataGenerateMap : ScriptableObject
	{
		public struct DataSpawnTile
		{
			public TypeTile Type;
			public float ScaleNoise;
			public float SpawnLuck;
			public float SpawnValue; // Avec le noisie
		}
		public int SizeMapX = 20;
		public int SizeMapY = 20;
		public TypeTile Ground;
		public TypeTile Border;
		public DataSpawnTile[] SpawnTile;
	}
}