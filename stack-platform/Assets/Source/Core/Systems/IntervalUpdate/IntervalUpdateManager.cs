using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Core.Systems.IntervalUpdate
{
    public class IntervalUpdateManager : MonoBehaviour
    {
        private static IntervalUpdateManager _instance;
        public static IntervalUpdateManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                var go = new GameObject("__IntervalUpdateManager");
                _instance = go.AddComponent<IntervalUpdateManager>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        private struct Entry
        {
            public IIntervalUpdate Target;
            public int FrameCounter;
            public float TimeCounter;
        }

        private readonly List<Entry> _entries = new List<Entry>();

        private void Update()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                switch (e.Target.Mode)
                {
                    case IntervalMode.FrameBased:
                    {
                        e.FrameCounter++;
                        if (e.FrameCounter >= e.Target.FrameInterval)
                        {
                            e.Target.IntervalUpdate();
                            e.FrameCounter = 0;
                        }

                        break;
                    }
                    case IntervalMode.TimeBased:
                    {
                        e.TimeCounter += Time.deltaTime;
                        if (e.TimeCounter >= e.Target.TimeInterval)
                        {
                            e.Target.IntervalUpdate();
                            e.TimeCounter = 0f;
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
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