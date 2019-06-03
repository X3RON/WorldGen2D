using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ChunkGen {

    internal class ChunkController : MonoBehaviour {

        [SerializeField] private Tile tileGras;
        [SerializeField] private Tile tileGrasLight;
        [SerializeField] private Tile tileDirt;
        [SerializeField] private Tile tileSand;
        [SerializeField] private GameObject treeBig;
        [SerializeField] private GameObject treeSmall;

        // path of chunk data on disk (savepath)
        private string fileName;

        // reference to data of that chunk
        private ChunkData chunkData;

        // reference to my tilemap
        private Tilemap terrainTilemap;

        public void CustomStart(int x, int y) {

            fileName = Path.Combine(Application.persistentDataPath, ChunkConstants.FilePath, x.ToString() + "_" + y.ToString());
            terrainTilemap = GetComponentInChildren<Tilemap>();

            if (File.Exists(fileName) && false) {
                Debug.Log("Load...");
                chunkData = LoadChunk();
            } else {
                Debug.Log("Generate...");
                chunkData = GenerateChunk(x, y);
                SaveChunk();

            }
            IDtoTerrain();
            IDtoMapObject();

        }

        #region ID to Terrain / MapObject 

        private void IDtoTerrain() {
            int i = 0;
            Vector3Int pos = Vector3Int.zero;
            Tile t;
            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {
                    pos.x = xx;
                    pos.y = yy;

                    switch (chunkData.TerrainTiles[i++]) {
                        case ChunkConstants.Terrain_GrasID: t = tileGras; break;
                        case ChunkConstants.Terrain_SandID: t = tileSand; break;
                        case ChunkConstants.Terrain_LightGrasID: t = tileGrasLight; break;
                        case ChunkConstants.Terrain_DirtID: t = tileDirt; break;
                        default: t = tileDirt; break;
                    }

                    terrainTilemap.SetTile(pos, t);
                }
            }
        }

        private void IDtoMapObject() {
            int i = 0;
            Vector3 pos = Vector3.zero;
            GameObject gO;

            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {
                    pos.x = xx + chunkData.X * ChunkManager.Instance.ChunkSize + Random.Range(-ChunkConstants.Resource_Random, ChunkConstants.Resource_Random);
                    pos.y = yy + chunkData.Y * ChunkManager.Instance.ChunkSize + Random.Range(-ChunkConstants.Resource_Random, ChunkConstants.Resource_Random);

                    switch (chunkData.MapObjects[i++]) {
                        case ChunkConstants.MapObject_TreeBig: gO = treeBig; break;
                        case ChunkConstants.MapObject_TreeSmall: gO = treeSmall; break;
                        default: gO = null; break;
                    }

                    if (gO != null) {
                        GameObject g = Instantiate(gO, pos, Quaternion.identity);
                        g.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-g.transform.position.y);
                    }
                }
            }
        }
        #endregion

        #region Chunk Generation, saving & loading 

        private void SaveChunk() {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate)) {
                binaryFormatter.Serialize(fileStream, chunkData);
            }
        }

        private ChunkData GenerateChunk(int x, int y) {

            float[] tempNoiseMap = Generate2DNoise(ChunkManager.Instance.TempPara, new Vector2(x, y));
            float[] humNoiseMap = Generate2DNoise(ChunkManager.Instance.HumPara, new Vector2(x, y));
            float[] treeNoiseMap = Generate2DNoise(ChunkManager.Instance.TreePara, new Vector2(x, y));

            return new ChunkData(x, y, NoiseToTerrainID(tempNoiseMap, humNoiseMap), NoiseToTreeID(treeNoiseMap));
        }

        private float[] Generate2DNoise(NoiseParameter para, Vector2 chunkOffset) {

            float frequenzy = para.frequenzy;

            chunkOffset.x *= ChunkManager.Instance.ChunkSize;
            chunkOffset.y *= ChunkManager.Instance.ChunkSize;

            chunkOffset.x += para.seed;
            chunkOffset.y += para.seed;

            float[] tiles = new float[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];
            int i = 0;

            // generate terrainNoise
            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {

                    float impact = 1;
                    float noise = 0;

                    frequenzy = para.frequenzy;

                    for (int oct = 0; oct < ChunkManager.Instance.Octaves; oct++) {
                        noise += impact * Mathf.PerlinNoise(frequenzy * (chunkOffset.x + xx), frequenzy * (chunkOffset.y + yy));

                        frequenzy *= para.frequenzyMultiplier;
                        impact *= para.persistent;

                    }
                    tiles[i++] = Mathf.Pow(noise, para.redistribution);
                }
            }
            return tiles;
        }

        private ChunkData LoadChunk() {

            if (File.Exists(fileName)) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream fileStream = File.Open(fileName, FileMode.Open)) {
                    return (ChunkData)binaryFormatter.Deserialize(fileStream);
                }
            }
            return null;
        }
        #endregion

        #region Convert Noise To ID
        private byte[] NoiseToTerrainID(float[] temp, float[] hum) {

            byte[] tileIDs = new byte[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];
            int i = 0;

            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {

                    if (temp[i] > 0.5f) {

                        if (hum[i] > 0.5f) {
                            tileIDs[i] = ChunkConstants.Terrain_GrasID;
                        } else {
                            tileIDs[i] = ChunkConstants.Terrain_SandID;
                        }
                    } else {

                        if (hum[i] > 0.5f) {
                            tileIDs[i] = ChunkConstants.Terrain_LightGrasID;
                        } else {
                            tileIDs[i] = ChunkConstants.Terrain_DirtID;
                        }
                    }
                    i++;
                }
            }
            return tileIDs;


        }

        private byte[] NoiseToTreeID(float[] tree) {

            byte[] treeIDs = new byte[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];
            int i = 0;
            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {
                    if (tree[i] > 0.9) {
                        treeIDs[i] = ChunkConstants.MapObject_TreeBig;
                    } else if (tree[i] > 0.8) {
                        treeIDs[i] = ChunkConstants.MapObject_TreeSmall;
                    } else {
                        treeIDs[i] = 0;
                    }
                    i++;
                }
            }
            return treeIDs;
        }

        #endregion
    }

}
