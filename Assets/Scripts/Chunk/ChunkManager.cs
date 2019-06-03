using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ChunkGen {

    public class ChunkManager : MonoBehaviour {

        #region Inspector Values

        [Header("number of tiles per chunk")]
        [Range(8, 64)]
        [SerializeField] private int chunkSize;

        [Header("number of chunks in X and Y direction")]
        [SerializeField] private int numberOfChunks;

        [Header("number of octaves/iterations")]
        [Range(1, 6)]
        [SerializeField] private int octaves;

        [Header("Temperature Noise Parameters")]
        [SerializeField] private NoiseParameter tempPara;

        [Header("Humidity Noise Parameters")]
        [SerializeField] private NoiseParameter humPara;

        [Header("Humidity Noise Parameters")]
        [SerializeField] private NoiseParameter treePara;

        #endregion

        #region Properties

        public static ChunkManager Instance { get; private set; }

        public int ChunkSize { get { return chunkSize; } }
        public int NumberOfChunks { get { return numberOfChunks; } }
        public float Octaves { get { return octaves; } }

        public NoiseParameter TempPara { get { return tempPara; } }
        public NoiseParameter HumPara { get { return humPara; } }
        public NoiseParameter TreePara { get { return treePara; } }

        #endregion

        // Grid, which holds the references to every chunk
        private List<GameObject> chunks = new List<GameObject>();
        private Stopwatch sw = new Stopwatch();

        private void CreateWorld() {

            for (int y = 0; y < numberOfChunks; y++) {
                for (int x = 0; x < numberOfChunks; x++) {
                    GameObject gO = Instantiate(Resources.Load("Chunk"), new Vector3(x * chunkSize, y * chunkSize, 0), Quaternion.identity) as GameObject;
                    gO.GetComponent<ChunkController>().CustomStart(x, y);
                    chunks.Add(gO);
                }
            }
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds * 0.001f);
        }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }
        }

        private void Start() {
            CheckIfDirectoryExists();
            sw.Start();
            StartCoroutine("CreateWorld");
        }

        private void CheckIfDirectoryExists() {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, ChunkConstants.FilePath))) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, ChunkConstants.FilePath));
            }
        }
    }
}
