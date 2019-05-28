using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ChunkGen {

    internal class ChunkController : MonoBehaviour {

        // path of chunk data on disk (savepath)
        private string fileName;

        // reference to data of that chunk
        private ChunkData chunkData;

        public void CustomStart(int x, int y) {

            fileName = Path.Combine(Application.persistentDataPath, Constants.FilePath, x.ToString() + "_" + y.ToString());

            //if (File.Exists(fileName)) {
            //chunkData = LoadChunk();
            //} else {
            chunkData = GenerateChunk(x, y);
            SaveChunk();
            //}

            //tilemap.set;
        }

        #region Chunk Generation, saving & loading 

        private void SaveChunk() {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate)) {
                binaryFormatter.Serialize(fileStream, chunkData);
            }
        }

        private ChunkData GenerateChunk(int x, int y) {

            int numberOfChunks = ChunkManager.Instance.NumberOfChunks;
            int chunkSize = ChunkManager.Instance.ChunkSize;
            float frequenzy = ChunkManager.Instance.Frequenzy;

            float frequenzyMultiplier = ChunkManager.Instance.FrequenzyMultiplier;
            float persi = ChunkManager.Instance.Persistent;
            float octaves = ChunkManager.Instance.Octaves;

            float xOffset = /*ChunkManager.Instance.Seed + */(x * ChunkManager.Instance.ChunkSize);
            float yOffset = /*ChunkManager.Instance.Seed + */(y * ChunkManager.Instance.ChunkSize);


            // holds the noise
            byte[] tiles = new byte[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];

            // generate terrainNoise
            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {

                    float impact = 1;
                    float noiseTemp = 0;
                    float noiseHum = 0;
                    frequenzy = ChunkManager.Instance.Frequenzy;
                    for (int oct = 0; oct < ChunkManager.Instance.Octaves; oct++) {
                        noiseTemp += impact * Mathf.PerlinNoise(frequenzy * (xOffset + xx), frequenzy * (yOffset + yy));
                        //noiseHum += impact * Mathf.PerlinNoise(frequenzy * (xOffset + xx), frequenzy * (yOffset + yy));

                        frequenzy *= frequenzyMultiplier;
                        impact *= persi;
                        


                    }
                    noiseTemp = Mathf.Pow(noiseTemp, ChunkManager.Instance.Redistrubution);

                    //TOMScript.Instance.SetTile(gameObject, noiseTemp, noiseHum);
                    ChunkManager.Instance.Texture.SetPixel((int)xOffset + xx, (int)yOffset + yy, new Color(noiseTemp, noiseTemp, noiseTemp, 1));
                }
            }

            ChunkManager.Instance.Texture.Apply();
            ChunkManager.Instance.UpdateTexture();

            return new ChunkData(x, y, tiles);

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


    }

}
