using Errant.src.Graphics;
using Errant.src.Loaders;
using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Errant.src.World {

    public class PersistentTile {

		private static Dictionary<BIOME, ushort> biomeGroundIdMap = new Dictionary<BIOME, ushort>() {
			{ BIOME.OCEAN, GroundIds.Water },
			{ BIOME.PLAINS, GroundIds.Grass },
			{ BIOME.FOREST, GroundIds.LushGrass },
			{ BIOME.SAVANNA, GroundIds.DryGrass },
			{ BIOME.JUNGLE, GroundIds.ThickGrass },
			{ BIOME.SWAMP, GroundIds.MossyGrass },
			{ BIOME.ICE_PLAINS, GroundIds.Snow },
			{ BIOME.ALPS, GroundIds.Stone },
			{ BIOME.TAIGA, GroundIds.SnowyGrass },
			{ BIOME.DESERT, GroundIds.Sand },
			{ BIOME.MESA, GroundIds.Conglomerate },
			{ BIOME.VOLCANIC, GroundIds.Basalt },
		};

        static Random rng = new Random();

        public ushort GroundTileId { get; set; }
        public ushort ObjectTileId { get; set; }

        public PersistentTile() {
            GroundTileId = 0;
            ObjectTileId = 0;
        }

        public PersistentTile(PointData pointData) {

            GroundTileId = biomeGroundIdMap[pointData.biome];

			if (!pointData.land) {
                GroundTileId = GroundIds.Water;
			}

            if (rng.NextDouble() > 0.975f) {
                ObjectTileId = 1;
            } 
            else if (rng.NextDouble() > 0.95f) {
                ObjectTileId = 2;
            }
            else {
                ObjectTileId = 0;
            }
        }

        public void ClearObject() {
            ObjectTileId = 0;
        }
	}

    public class ActiveTile {

        private PersistentTile persistentTile;

        private Dictionary<ushort, byte> edgeTransitionData;
        private Dictionary<ushort, byte> cornerTransitionData;

        private List<KeyValuePair<ushort, byte>> transitionData;

        public ActiveTile(PersistentTile _persistentTile) {
            persistentTile = _persistentTile;
            edgeTransitionData = new Dictionary<ushort, byte>();
            cornerTransitionData = new Dictionary<ushort, byte>();
            transitionData = new List<KeyValuePair<ushort, byte>>();
        }

        public void DrawGround(SpriteBatch spriteBatch, TextureAtlas tileAtlas, int xPos, int yPos) {

            // Draw base Ground Texture
            int textureRegionIndex = (persistentTile.GroundTileId * tileAtlas.Width);
            TextureRegion2D textureRegion = tileAtlas.GetTextureRegion(textureRegionIndex);
            spriteBatch.Draw(textureRegion.Texture, new Vector2(xPos, yPos), textureRegion.SourceRectangle, Color.White);

            // Draw transition textures
            foreach (KeyValuePair<ushort, byte> kvp in transitionData) {
                textureRegionIndex = (kvp.Key * tileAtlas.Width) + kvp.Value;
                textureRegion = tileAtlas.GetTextureRegion(textureRegionIndex);
                spriteBatch.Draw(textureRegion.Texture, new Vector2(xPos, yPos), textureRegion.SourceRectangle, Color.White);
            }
        }

        public void DrawObject(SpriteBatch spriteBatch, int xPos, int yPos) {

            if (persistentTile.ObjectTileId == 0) {
                return;
            }

            TextureRegion2D region = ObjectManager.GetObjectTextureRegionById(persistentTile.ObjectTileId);
            spriteBatch.Draw(region.Texture, new Vector2(xPos, yPos), region.SourceRectangle, Color.White);
        }

        public void IncrementEdgeTransition(ushort tileType, byte direction) {
            if (!edgeTransitionData.ContainsKey(tileType)) {
                edgeTransitionData.Add(tileType, 0);
            }
            edgeTransitionData[tileType] += direction;
        }

        public void IncrementCornerTransition(ushort tileType, byte direction) {
            if (!cornerTransitionData.ContainsKey(tileType)) {
                cornerTransitionData.Add(tileType, 0);
            }
            cornerTransitionData[tileType] += direction;
        }

        public void PrepTransitionData() {
            // Transition data for edges and corners needs to be combined and ordered base on ground id priority. 

            List<ushort> keys = new List<ushort>(cornerTransitionData.Keys);
            foreach (ushort key in keys) {
                cornerTransitionData[key] += 16;
            }

            transitionData.AddRange(edgeTransitionData.ToList());
            transitionData.AddRange(cornerTransitionData.ToList());
            transitionData.Sort(
                delegate (KeyValuePair<ushort, byte> pair1, KeyValuePair<ushort, byte> pair2) {
                    var a = Array.IndexOf(GroundIds.priorities, pair1.Key);
                    var b = Array.IndexOf(GroundIds.priorities, pair2.Key);
                    if (a == b) {
                        return pair1.Value >= 16 ? -1 : 1;
                    }
                    return a.CompareTo(b);
                }
            );
        }

        public void ClearObject() {
            persistentTile.ClearObject();
        }
        
        public void PlaceObject(ushort objectId) {
            persistentTile.ObjectTileId = objectId;
        }

        public void PrintDebugInfo() {
            System.Diagnostics.Debug.WriteLine("===================================");
            System.Diagnostics.Debug.WriteLine("Ground Id: " + persistentTile.GroundTileId);
            System.Diagnostics.Debug.WriteLine("-----");
            System.Diagnostics.Debug.WriteLine("EdgeTransitions: ");
            foreach (KeyValuePair<ushort, byte> kvp in edgeTransitionData) {
                System.Diagnostics.Debug.WriteLine(string.Format("Id = {0}, Count = {1}", kvp.Key, kvp.Value));
            }
            System.Diagnostics.Debug.WriteLine("-----");
            System.Diagnostics.Debug.WriteLine("-----");
            System.Diagnostics.Debug.WriteLine("CornerTransitions: ");
            foreach (KeyValuePair<ushort, byte> kvp in cornerTransitionData) {
                System.Diagnostics.Debug.WriteLine(string.Format("Id = {0}, Count = {1}", kvp.Key, kvp.Value));
            }
            System.Diagnostics.Debug.WriteLine("-----");

            System.Diagnostics.Debug.WriteLine("===================================");
        }
    }
}
