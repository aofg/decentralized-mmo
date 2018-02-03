using UFTM;
using UnityEngine;

namespace UI.Tests
{
    public class BuilderViewMock : MonoBehaviour
    {
        public Tileset Atlas;

        private void Start()
        {
            var builder = GetComponent<IBuilderView>();
            builder.Setup(Atlas);
        }
    }
}