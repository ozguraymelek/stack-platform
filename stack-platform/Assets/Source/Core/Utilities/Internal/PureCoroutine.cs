using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Core.Utilities.Internal
{
    public class PureCoroutine : MonoBehaviour
    {
        public void RunPureCoroutine<T>(IEnumerator<T> coroutine)
        {
            StartCoroutine(coroutine);
        }
        
        public void RunPureCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}