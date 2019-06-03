using UnityEngine;
using UnityEngine.Tilemaps;

namespace ChunkGen {
    
    internal class Autotile : MonoBehaviour{

        [SerializeField] Tile[] gras;

        public static Tile[] GetAutotile(byte[] feld, int size) {


            char[] Tilefeld = new char[feld.Length];

            byte[,] feld2d = new byte[feld.Length, feld.Length];

            byte searchvalue;

            // 2d feld füllen
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    feld2d[x, y] = feld[x + y * size];
                }
            }


            for (int i = 0; i < feld.Length; i++) {
                Tilefeld[i] = '0';
            }



            for (int i = 0; i < size; i++) {

                for (int o = 0; o < size; o++) {
                    byte AktWert = 0;

                    if (feld2d[o, i] > 0) {

                        searchvalue = feld2d[o, i];
                        // Oben drüber; Addierter Wert: 1
                        if (i > 0) {
                            if (feld2d[o, i - 1] == searchvalue) {
                                AktWert += 1;

                            }
                        }

                        // links, addierter Wert: 2
                        if (o > 0) {
                            if (feld2d[o - 1, i] == searchvalue) {
                                AktWert += 2;
                            }
                        }

                        //rechts, Addierter Wert: 4
                        if (o < size - 1) {
                            if (feld2d[o + 1, i] == searchvalue) {
                                AktWert += 4;
                            }
                        }

                        // unten, addierter Wert: 8
                        if (i < size - 1) {
                            if (feld2d[o, i + 1] == searchvalue) {
                                AktWert += 8;
                            }
                        }

                        char lol = WertZuFeld(AktWert);

                        Tilefeld[o + i * size] = lol;

                    }
                }

            }

            return Tilefeld;

        }

        private static char WertZuFeld(byte wert) {
            if (wert == 0) {
                return 'A';
            } else if (wert == 1) {
                return 'B';
            } else if (wert == 2) {
                return 'C';
            } else if (wert == 3) {
                return 'D';
            } else if (wert == 4) {
                return 'E';
            } else if (wert == 5) {
                return 'F';
            } else if (wert == 6) {
                return 'G';
            } else if (wert == 7) {
                return 'H';
            } else if (wert == 8) {
                return 'I';
            } else if (wert == 9) {
                return 'J';
            } else if (wert == 10) {
                return 'K';
            } else if (wert == 11) {
                return 'L';
            } else if (wert == 12) {
                return 'M';
            } else if (wert == 13) {
                return 'N';
            } else if (wert == 14) {
                return 'O';
            } else if (wert == 15) {
                return 'P';
            } else {
                return 'Z';
            }
        }

    }
    
}
