using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BufferReaderTest : MonoBehaviour
    {
        public void Awake()
        {
            MessageBuffer<BufferTester>.Subscribe(BufferDebug);
        }

        public void OnDisable()
        {
            MessageBuffer<BufferTester>.Unsubscribe(BufferDebug);
        }

        public void BufferDebug(BufferTester msg)
        {
            print("Im Working");
        }
    }
}
