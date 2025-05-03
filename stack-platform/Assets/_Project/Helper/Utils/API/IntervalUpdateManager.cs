using System.Collections.Generic;
using UnityEngine;

namespace _Project.Helper.Utils.API
{
    public class IntervalUpdateManager : MonoBehaviour
    {
        static IntervalUpdateManager _instance;
        public static IntervalUpdateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("__IntervalUpdateManager");
                    _instance = go.AddComponent<IntervalUpdateManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        struct Entry
        {
            public IIntervalUpdate Target;
            public int FrameCounter;
            public float TimeCounter;
        }

        List<Entry> _entries = new List<Entry>();

        void Update()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                if (e.Target.Mode == IntervalMode.FrameBased)
                {
                    e.FrameCounter++;
                    if (e.FrameCounter >= e.Target.FrameInterval)
                    {
                        e.Target.IntervalUpdate();
                        e.FrameCounter = 0;
                    }
                }
                else // SecondBased
                {
                    e.TimeCounter += Time.deltaTime;
                    if (e.TimeCounter >= e.Target.TimeInterval)
                    {
                        e.Target.IntervalUpdate();
                        e.TimeCounter = 0f;
                    }
                }
                _entries[i] = e;
            }
        }

        public void Register(IIntervalUpdate update)
        {
            _entries.Add(new Entry { Target = update, FrameCounter = 0, TimeCounter = 0f });
        }

        public void Unregister(IIntervalUpdate update)
        {
            _entries.RemoveAll(e => e.Target == update);
        }
    }
}