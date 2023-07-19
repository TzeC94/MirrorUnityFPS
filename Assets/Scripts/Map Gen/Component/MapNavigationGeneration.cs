using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Unity.AI.Navigation {

    public class MapNavigationGeneration : NavMeshSurface, IMapGen {
        public void Initialize() {

        }

        public IEnumerator Process() {

            yield return null;

            if (GameManagerBase.instance.isServer) {

                BuildNavMesh();

            }

        }

        public void Load() {
            return;
        }

        public void Save() {
            return;
        }
    }

}