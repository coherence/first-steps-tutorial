using System.Collections;
using Coherence.Toolkit;
using UnityEngine;

namespace Coherence.Samples
{
    /// <summary>
    /// This script is in charge of spawning a number of physical balls (which are network entities).
    /// Then, the box rotates randomly. Whoever owns the box entity owns all the ball instances as well.
    /// The actual physical simulation is just default Unity physics.
    /// </summary>
    public class PhysicsSimulation : MonoBehaviour
    {
        public CoherenceSync spherePrefab;
        public GameObject loadingScreen;
        public float speed = 1f;

        private int gridSize = 23;
        private float spacing = 1f;
        private int maxObjects = 300;

        private float minRotation = -20f;
        private float maxRotation = 20f;

        private float currentRotation;
        private bool invertRotation;

        private float interpTime;

        private bool startRotation;

        private void Awake()
        {
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out CoherenceBridge coherenceBridge);
            coherenceBridge.onConnected.AddListener(_ =>
            {
                loadingScreen.SetActive(true);
            });
            coherenceBridge.onLiveQuerySynced.AddListener(_ =>
            {
                loadingScreen.SetActive(false);

                if (!GetComponent<CoherenceSync>().HasStateAuthority)
                {
                    return;
                }

                SpawnSpheres();
                StartCoroutine(StartRotating());
            });
        }

        private IEnumerator StartRotating()
        {
            yield return new WaitForSeconds(1f);
            startRotation = true;
        }

        private void Update()
        {
            if (!startRotation)
            {
                return;
            }

            transform.Rotate(new Vector3(currentRotation, currentRotation, currentRotation) * Time.deltaTime);

            if (!invertRotation)
            {
                currentRotation = Mathf.Lerp(minRotation, maxRotation, interpTime);
                interpTime += speed * Time.deltaTime;

                if (interpTime >= 1f)
                {
                    interpTime = 0f;
                    invertRotation = true;
                }
            }
            else
            {
                currentRotation = Mathf.Lerp(maxRotation, minRotation, interpTime);
                interpTime += speed * Time.deltaTime;

                if (interpTime >= 1f)
                {
                    interpTime = 0f;
                    invertRotation = false;
                }
            }
        }

        private void SpawnSpheres()
        {
            int objectsSpawned = 0;
            for (int x = 0; x < gridSize && objectsSpawned < maxObjects; x++)
            {
                for (int z = 0; z < gridSize && objectsSpawned < maxObjects; z++)
                {
                    float xPos = -11f + (x * spacing);
                    float zPos = -11f + (z * spacing);
                    Vector3 spawnPosition = new Vector3(xPos, 2.5f, zPos);

                    Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
                    objectsSpawned++;
                }
            }
        }
    }
}
