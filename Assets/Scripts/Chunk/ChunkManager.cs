using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ChunkGen {

    public class ChunkManager : MonoBehaviour {

        // Inspector Values
        [SerializeField] private int chunkSize;

        [Header("amount of chunks in X and Y direction")]
        [SerializeField] private int numberofChunks;

        // Singleton
        public static ChunkManager Instance { get; private set; }

        // Properties
        public int ChunkSize { get { return chunkSize; } }
        public int NumberOfChunks { get { return numberofChunks; } }

        // Grid, which holds the references to every chunk
        private List<GameObject> chunks = new List<GameObject>();

        private void CreateWorld() {

            for (int y = 0; y < numberofChunks; y++) {
                for (int x = 0; x < numberofChunks; x++) {
                    chunks.Add(Instantiate(Resources.Load("Chunk"), Vector3.zero, Quaternion.identity) as GameObject);
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
