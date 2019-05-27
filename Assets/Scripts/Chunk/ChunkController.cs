using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ChunkGen {

    internal class ChunkController :MonoBehaviour {

        // path of chunk data on disk (savepath)
        private string fileName;

        // reference to data of that chunk
        private ChunkData chunkData;

        internal ChunkController(int x, int y) {

            fileName = Path.Combine(Application.persistentDataPath, Constants.FilePath, x.ToString() +"_"+ y.ToString());

            if (File.Exists(fileName)) {
                chunkData = LoadChunk();
            } else {
                chunkData = GenerateChunk(x, y);
                SaveChunk();
            }
            Debug.Log("Created chunk: " + x.ToString() + "_" + y.ToString());
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
            int xOffset = x * ChunkManager.Instance.ChunkSize;
            int yOffset = y * ChunkManager.Instance.ChunkSize;

            // holds the noise
            byte[] terrainNoise = new byte[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];

            // generate terrainNoise
            for (int yy = 0; yy < numberOfChunks; yy++) {
                for (int xx = 0; xx < numberOfChunks; xx++) {
                    terrainNoise[xx + yy * numberOfChunks] = (byte)(Mathf.PerlinNoise(xOffset + xx, yOffset + yy) * 255);
                }
            }
            Debug.Log("gen chunk");
            return new ChunkData(x, y, terrainNoise);

        }

        private ChunkData LoadChunk() {

            if (File.Exists(fileName)) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream fileStream = File.Open(fileName, FileMode.Open)) {
                    Debug.Log("load: " + fileName);
                    return (ChunkData)binaryFormatter.Deserialize(fileStream);

                }
            }
            return null;
        }
        #endregion


    }

}
