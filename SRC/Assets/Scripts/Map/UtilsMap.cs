using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
	public enum ETile
	{
		RESOURCE,
		OBSTACLE,
		GROUND,
	}

	public enum EResource
	{
		GOLD,
		IRON,
		WATER,
		TREASURE,
	}

	public enum EObstacle
	{
		ROCK,
		WALL,
		FIRE,
	}

	public enum EGround
	{
		SAND,
		GRASS,
		MISC,
	}

	public struct Point
	{
		public int x, y;
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		

		public static bool operator ==(Point lhs, Point rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		public static bool operator !=(Point lhs, Point rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}

		public static Point operator +(Point lhs, Point rhs)
		{
			return new Point(lhs.x + rhs.x, lhs.y + rhs.y);
		}
		public static Point operator -(Point lhs, Point rhs)
		{
			return new Point(lhs.x - rhs.x, lhs.y - rhs.y);
		}

		public override string ToString()
		{
			return "[" + x + ":" + y + "]";
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}


	[System.Serializable]
	public struct TypeTile : System.IEquatable<TypeTile>
	{
		public ETile Tile;
		public int SubTile;

		public TypeTile(ETile tile, int subTile)
		{
			Tile = tile;
			SubTile = subTile;
		}

		public bool Equals(TypeTile other)
		{
			return this.Tile == other.Tile && this.SubTile == other.SubTile;
		}
	}

	[System.Serializable]
	public class TileFlyweight
	{
		public TypeTile Type;
		public Sprite Text;
		public Color Color;
	}

	public class TileMapData
	{
		public TileFlyweight Data;
		public int AmountResources;

		public TileMapData(TileFlyweight data, int amountResources = -1)
		{
			Data = data;
			AmountResources = amountResources;
		}
	}
}