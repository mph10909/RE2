using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BufferTester : BaseMessage { }

    public class MessageBufferTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageBuffer<BufferTester>.Dispatch();
            }
        }
    }
}
