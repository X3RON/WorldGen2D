using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoiseParameter {

    [Header("seed of Perlin noise")]
    public float seed;

    [Header("zoom into noise")]
    [Range(0.001f, 0.8f)]
    public float frequenzy;

    [Header("change of frequency per octave")]
    [Range(0.5f, 10f)]
    public float frequenzyMultiplier;

    [Header("impact of octaves")]
    [Range(0.0f, 0.9f)]
    public float persistent;

    [Header("manipulate height (small values = get a higher noise)")]
    [Range(0f, 10f)]
    public float redistribution;
}
