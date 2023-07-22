using System.Collections;

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
    }

}