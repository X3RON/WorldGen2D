using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;

namespace ChunkGen {

    public class ChunkManager : MonoBehaviour {

        #region Inspector Values
        [Header("number of tiles per chunk")]
        [Range(8, 64)]
        [SerializeField] private int chunkSize;

        [Header("number of chunks in X and Y direction")]
        [SerializeField] private int numberofChunks;

        [Header("zoom into noise")]
        [Range(0.001f, 0.01f)]
        [SerializeField] private float frequenzy;

        [Header("change of frequency per octave")]
        [Range(0.5f, 10f)]
        [SerializeField] private float frequenzyMultiplier;

        [Header("impact of octaves")]
        [Range(0.0f, 0.9f)]
        [SerializeField] private float persistent;

        [Header("manipulate height (small values = get a higher noise)")]
        [Range(0f, 10f)]
        [SerializeField] private float redistribution;

        [Header("seed of Perlin noise")]
        [SerializeField] private int seed;

        [Header("number of octaves/iterations")]
        [Range(2, 6)]
        [SerializeField] private int octaves;

        #endregion
        // Singleton

        #region Properties

        public static ChunkManager Instance { get; private set; }

        public int ChunkSize { get { return chunkSize; } }
        public int NumberOfChunks { get { return numberofChunks; } }
        public float Frequenzy { get { return frequenzy; } }
        public float FrequenzyMultiplier { get { return frequenzyMultiplier; } }
        public float Persistent { get { return persistent; } }
        public float Seed { get { return seed; } }
        public float Octaves { get { return octaves; } }
        public float Redistrubution { get { return redistribution; } }
        public Texture2D Texture { get; set; }
        #endregion

        // Grid, which holds the references to every chunk
        private List<GameObject> chunks = new List<GameObject>();

        Stopwatch sw = new Stopwatch();

        public void UpdateTexture() {
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(ChunkManager.Instance.Texture, new Rect(0, 0, numberofChunks * chunkSize, numberofChunks * chunkSize), new Vector2(0.5f, 0.5f));
        }

        private void CreateWorld() {

            for (int y = 0; y < numberofChunks; y++) {
                for (int x = 0; x < numberofChunks; x++) {
                    GameObject gO = Instantiate(Resources.Load("Chunk"), Vector3.zero, Quaternion.identity) as GameObject;
                    gO.GetComponent<ChunkController>().CustomStart(x, y);
                    chunks.Add(gO);
                }
            }
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds*0.001f);
        }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }
        }

        private void Start() {
            Texture = new Texture2D(numberofChunks * chunkSize, numberofChunks * chunkSize);
            CheckIfDirectoryExists();
            sw.Start();
            StartCoroutine("CreateWorld");
        }

        private void CheckIfDirectoryExists() {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, Constants.FilePath))) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, Constants.FilePath));
            }
        }
    }
}
