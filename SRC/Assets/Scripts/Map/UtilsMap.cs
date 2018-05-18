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

		public override bool Equals(object obj)
		{
			if (!(obj is Point))
			{
				return false;
			}

			var point = (Point)obj;
			return x == point.x &&
				   y == point.y;
		}

		public override int GetHashCode()
		{
			var hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Point lhs, Point rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		public static bool operator !=(Point lhs, Point rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
	}


	public struct TypeTile : System.IEquatable<TypeTile>
	{
		public ETile Tile;
		public int SubTile;

		public bool Equals(TypeTile other)
		{
			return this.Tile == other.Tile && this.SubTile == other.SubTile;
		}
	}

	public class TileFlyweight
	{
		public TypeTile Type;
		public Sprite Text;
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