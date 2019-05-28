using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ChunkGen {

    internal class ChunkController :MonoBehaviour {

        // path of chunk data on disk (savepath)
        private string fileName;

        // reference to data of that chunk
        private ChunkData chunkData;

        public void CustomStart(int x, int y) {

            fileName = Path.Combine(Application.persistentDataPath, Constants.FilePath, x.ToString() +"_"+ y.ToString());

            //if (File.Exists(fileName)) {
                //chunkData = LoadChunk();
            //} else {
                chunkData = GenerateChunk(x, y);
                SaveChunk();
            //}
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
            int chunkSize = ChunkManager.Instance.ChunkSize;
            float frequenzy = ChunkManager.Instance.Frequenzy;
            float octaves = ChunkManager.Instance.Octaves;
            int xOffset = x * ChunkManager.Instance.ChunkSize;
            int yOffset = y * ChunkManager.Instance.ChunkSize;


            // holds the noise
            byte[] tiles = new byte[ChunkManager.Instance.ChunkSize * ChunkManager.Instance.ChunkSize];

            // generate terrainNoise
            for (int yy = 0; yy < ChunkManager.Instance.ChunkSize; yy++) {
                for (int xx = 0; xx < ChunkManager.Instance.ChunkSize; xx++) {

                    float noiseTemp = Mathf.PerlinNoise(frequenzy*(xOffset + xx) , frequenzy* (yOffset + yy));
                    //TOMScript.Instance.SetTile(gameObject, 0.3f, 05f);
                    //float noiseHum = Mathf.PerlinNoise(xOffset + xx, yOffset + yy);
                    //byte tileID = 0;
                    ChunkManager.Instance.Texture.SetPixel(xOffset + xx, yOffset + yy, new Color(noiseTemp, noiseTemp, noiseTemp, 1));
                    //tiles[xx + yy * numberOfChunks] = 
                }
            }
            ChunkManager.Instance.Texture.Apply();
            ChunkManager.Instance.UpdateTexture();


            Debug.Log("gen chunk");
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
