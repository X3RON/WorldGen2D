using System;

namespace ChunkGen {

    [Serializable]
    public class ChunkData {

        // position of chunk X
        public int X { get; private set; }

        // position of chunk Y
        public int Y { get; private set; }

        // holds the tile ids
        public byte[] TerrainTiles { get; private set; }

        public byte[] MapObjects { get; set; }

        public ChunkData(int x, int y, byte[] terrainTiles, byte[] mapObjects) {
            X = x;
            Y = y;
            TerrainTiles = terrainTiles;
            MapObjects = mapObjects;
        }

    }
}
