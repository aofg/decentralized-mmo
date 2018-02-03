using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
namespace UFTM
{
    public class Renderer : MonoBehaviour
    {
        public const int COLS = 32;
        public const int ROWS = 32;
        public const int MAX_LAYERS = 10;
        public const int MAX_PARTICLES = ROWS * COLS * MAX_LAYERS;
        
        public Tileset Tileset;
        public Material SharedMaterial;
        private ParticleSystem ps;
        private ParticleSystemRenderer pr;

        private ParticleSystem.Particle[] particles;
        private Tile[] tiles;

        private int lastTile;
        private bool dirty;
        
#if !UFTM_WITHOUT_GC
    #if UFTM_WITH_USED
        private HashSet<int> usedTiles = new HashSet<int>();
    #else
        private Queue<int> dirtyTiles = new Queue<int>();
    #endif
#endif

        private void Awake()
        {
            tiles = new Tile[MAX_PARTICLES];
            ps = gameObject.AddComponent<ParticleSystem>();
            pr = gameObject.GetComponent<ParticleSystemRenderer>();

//            var main = ps.main;
//            main.startSize = 0.96f;
        }

        public void RemoveTiles(int x, int y)
        {
            var index = y * COLS + x;

            for (int layer = 0; layer < MAX_LAYERS; layer++)
            {
                var finalIndex = index + layer * COLS * ROWS;
#if !UFTM_WITHOUT_GC
    #if UFTM_WITH_USED
                if (usedTiles.Remove(finalIndex))
                {
                    tiles[finalIndex].Id = 0;
                    var particle = particles[finalIndex];
                    particle.position = Vector3.one * -1000;
                    particle.remainingLifetime = 1f;
                    particles[finalIndex] = particle;
                    dirty = true;
                }
    #else
                if (tiles[finalIndex].Id > 0)
                {
                    dirtyTiles.Enqueue(finalIndex);
                    tiles[finalIndex].Id = 0;
                    dirty = true;
                }
    #endif
#else
                if (tiles[finalIndex].Id > 0)
                {
                    tiles[finalIndex].Id = 0;
                    var particle = particles[finalIndex];
                    particle.position = Vector3.one * -1000;
                    particle.remainingLifetime = 1f;
                    particles[finalIndex] = particle;
                }
#endif
            }
        }

        public void ShowTile(int x, int y, int id, int layer = 0)
        {
            var index = y * COLS + x + layer * COLS * ROWS;
            
            tiles[index].X = (byte) x;
            tiles[index].Y = (byte) y;
            tiles[index].Id = (ushort)(id + 1);
            
#if !UFTM_WITHOUT_GC
    #if UFTM_WITH_USED
            usedTiles.Add(index);
    #else
            dirtyTiles.Enqueue(index);
    #endif
#endif

            dirty = true;
        }

        private void Start()
        {
            ps.Stop();

            pr.material = SharedMaterial;
            pr.material.mainTexture = Tileset.BaseTexture;
            
            var emission = ps.emission;
            emission.enabled = false;

            var shape = ps.shape;
            shape.enabled = false;
            
            var main = ps.main;
            main.maxParticles = MAX_PARTICLES;
            main.startLifetime = Tileset.Count * 2;
            main.duration = float.MaxValue;
            main.startSpeed = 0f;
            main.loop = false;
            main.gravityModifier = 0f;

            var sheet = ps.textureSheetAnimation;
            sheet.enabled = true;
            sheet.numTilesX = Tileset.Cols;
            sheet.numTilesY = Tileset.Rows;
            sheet.cycleCount = 2;
                
            
            particles = new ParticleSystem.Particle[MAX_PARTICLES];
            ps.Play();
            ps.Emit(MAX_PARTICLES);
            ps.GetParticles(particles);
            ps.Pause();

            PrepareParticles();
        }

        private void PrepareParticles()
        {
            // TODO: Emit when needed?
            for (int index = 0; index < MAX_PARTICLES; index++)
            {
                var particle = particles[index];
                particle.position = Vector3.one * -1000;
                particle.remainingLifetime = 1f;
                particles[index] = particle;
            }
        }

        private void Update()
        {
            if (dirty)
            {
                Cleanup();
            }
        }

        private void Cleanup()
        {
            Profiler.BeginSample("UFTM Renderer Cleanup");
#if !UFTM_WITHOUT_GC
    #if UFTM_WITH_USED
            foreach (var index in usedTiles)
            {
                var tile = tiles[index];
                if (tiles[index].Id > 0)
                {
                    var particle = particles[index];
                    particle.position = new Vector3(tile.X, tile.Y, 0);
                    particle.remainingLifetime = GetTimeToIndex(tile.Id);
                    particles[index] = particle;
                }
                
            }
    #else
            while (dirtyTiles.Count > 0)
            {
                var index = dirtyTiles.Dequeue();
                var tile = tiles[index];
                if (tile.Id > 0)
                {
                    var particle = particles[index];
                    particle.position = new Vector3(tile.X, tile.Y, 0);
                    particle.remainingLifetime = GetTimeToIndex(tile.Id);
                    particles[index] = particle;   
                    
                }
                else
                {
                    var particle = particles[index];
                    particle.position = Vector3.one * -1000;
                    particle.remainingLifetime = 1f;
                    particles[index] = particle;
                }
            }
    #endif
#else
            for (int layer = 0; layer < MAX_LAYERS; layer++)
            {
                for (int y = 0; y < ROWS; y++)
                {
                    for (int x = 0; x < COLS; x++)
                    {
                        var index = layer * ROWS * COLS + y * COLS + x;
                        var tile = tiles[index];
                        if (tiles[index].Id > 0)
                        {
                            var particle = particles[index];
                            particle.position = new Vector3(tile.X - 16f, tile.Y - 16f, -layer);
                            particle.remainingLifetime = GetTimeToIndex(tile.Id);
                            particles[index] = particle;                            
                        }
                    }
                }
            }
#endif
            
            ps.SetParticles(particles, MAX_PARTICLES);
            dirty = false;
            Profiler.EndSample();
        }

        private float GetTimeToIndex(int index)
        {
            return Tileset.Count - index % Tileset.Count;
        }
    }
}