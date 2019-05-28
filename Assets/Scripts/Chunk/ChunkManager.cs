using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ChunkGen {

    public class ChunkManager : MonoBehaviour {

        // Inspector Values
        [SerializeField] private int chunkSize;

        [Header("amount of chunks in X and Y direction")]
        [SerializeField] private int numberofChunks;

        [Header("zoom into noise")]
        [SerializeField] private float frequenzy;
        [Header("chnage of noise per octave")]
        [SerializeField] private float frequenzyMultiplier;
        [Header("impact of octaves")]
        [SerializeField] private float persistent;
        [Header("seed of Perlin noise")]
        [SerializeField] private float seed;
        [Header("number of octaves")]
        [SerializeField] private float octaves;

        // Singleton
        public static ChunkManager Instance { get; private set; }

        // Properties
        public int ChunkSize { get { return chunkSize; } }
        public int NumberOfChunks { get { return numberofChunks; } }
        public float Frequenzy { get { return frequenzy; } }
        public float Octaves { get { return octaves; } }
        public Texture2D Texture { get; set; }

        // Grid, which holds the references to every chunk
        private List<GameObject> chunks = new List<GameObject>();

        
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
            StartCoroutine("CreateWorld");
        }

        private void CheckIfDirectoryExists() {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, Constants.FilePath))) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, Constants.FilePath));
            }
        }
    }
}
