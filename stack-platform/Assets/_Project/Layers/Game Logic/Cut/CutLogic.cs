
using System;
using System.Threading.Tasks;
using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Cut
{
    public class CutLogic : MonoBehaviour
    {
        private SignalBus _signalBus;

        private PlatformTracker _platformTracker;
        private CutLogicData _cutLogicData;
        private CutterObjectConfig _cutterObjectConfig;
        private IAlignment _alignment;

        public ICutter CurrentCutter;

        private Cutter.Factory _cutterFactory;

        public float LastHullWidth;


        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker, CutLogicData cutLogicData,
            CutterObjectConfig cutterObjectConfig, IAlignment alignment, Cutter.Factory cutterFactory)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
            _cutLogicData = cutLogicData;
            _cutterObjectConfig = cutterObjectConfig;
            _alignment = alignment;
            _cutterFactory = cutterFactory;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlatformStopRequestedSignal>(Pretreatment);
            LastHullWidth = 1.5f;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlatformStopRequestedSignal>(Pretreatment);
        }

        private void Pretreatment()
        {
            if (_platformTracker.NextPlatform == null)
            {
                Debug.LogWarning("Pretreatment canceled, Next Platform is not yet tracked!");
                return;
            }

            FindCornerVertices();
            FindAngles();
            CutterNextLocation();
        }

        private void FindCornerVertices()
        {
            SRender.AnyObjectCornerVertexLocation(_platformTracker.CurrentPlatform.GetRenderer(),
                out _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                out _cutLogicData.CurrentPlatform.Location.ForwardRight, VertexLocation.LeftForward,
                VertexLocation.RightForward);

            SRender.AnyObjectCornerVertexLocation(_platformTracker.NextPlatform.GetRenderer(),
                out _cutLogicData.NextPlatform.Location.BackwardLeft,
                out _cutLogicData.NextPlatform.Location.BackwardRight, VertexLocation.LeftBackward,
                VertexLocation.RightBackward);
        }

        private void FindAngles()
        {
            _cutLogicData.CurrentPlatform.Angle.WithForwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicData.CurrentPlatform.Angle.WithForwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.CurrentPlatform.Location.ForwardRight,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);

            _cutLogicData.NextPlatform.Angle.WithBackwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.NextPlatform.Location.BackwardLeft,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicData.NextPlatform.Angle.WithBackwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.NextPlatform.Location.BackwardRight,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
        }

        private void CutterNextLocation()
        {
            if (_cutLogicData.NextPlatform.Location.BackwardRight.x <
                _cutLogicData.CurrentPlatform.Location.ForwardLeft.x
                || _cutLogicData.NextPlatform.Location.BackwardLeft.x >
                _cutLogicData.CurrentPlatform.Location.ForwardRight.x)
            {
                Debug.LogError("There is no intersection");
                _alignment.PerfectIntersectionStreak = 0;
                // ObjectPool.Enqueue(_platformRuntimeData.NextPlatform, "Platform");
                return;
            }

            if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft < _cutLogicData.CurrentPlatform.Angle.WithForwardLeft)
            {
                Debug.Log("platform on the left by player");

                // //TODO: add tolerance to perfect platform aligning
                if (_alignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardLeft,
                        _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                        _cutLogicData.AlignmentToleranceBoundLeft))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    // _platformTracker.NextPlatform.Outline.gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    //call sound
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    //call text
                    _cutLogicData.AlignmentToleranceBoundLeft = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();

                Debug.Log("NOT Perfect Alignment on left");
                SpawnCutter(_cutterFactory, _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                if (CurrentCutter == null)
                {
                    Debug.LogWarning("CutLogic: SpawnCutter hiç cutter üretmedi!");
                    return;
                }

                CurrentCutter?.ExternalCut(CurrentCutter.GetTransform(), _platformTracker.NextPlatform,
                    FellHullSide.Left,
                    _platformTracker.NextPlatform.GetRenderer().material);
            }

            else if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft <
                     Mathf.Abs(_cutLogicData.CurrentPlatform.Angle.WithForwardLeft))
            {
                Debug.Log("platform on the right by player");

                //TODO: add tolerance to perfect platform aligning
                if (_alignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardRight,
                        _cutLogicData.CurrentPlatform.Location.ForwardRight,
                        _cutLogicData.AlignmentToleranceBoundRight))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    // _platformTracker.NextPlatform.Outline.gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    // //call sound
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    //call text
                    _cutLogicData.AlignmentToleranceBoundRight = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();

                Debug.Log("NOT Perfect Alignment on right");
                if (_cutterObjectConfig == null)
                {
                    Debug.LogError("_cutterObjectConfig is null");
                    return;
                }

                if (_cutLogicData == null)
                {
                    Debug.LogError("_cutLogicData is null");
                    return;
                }

                SpawnCutter(_cutterFactory, _cutLogicData.CurrentPlatform.Location.ForwardRight,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                CurrentCutter?.ExternalCut(CurrentCutter.GetTransform(), _platformTracker.NextPlatform,
                    FellHullSide.Right,
                    _platformTracker.NextPlatform.GetRenderer().material);
            }


        }

        // private void SetInternalPositionBeforeAssign(ref Vector3 position, ref Quaternion rotation)
        // {
        //     Position = position;
        //     Rotation = rotation;
        // }
        //
        public void SpawnCutter(Cutter.Factory cutterFactory, Vector3 position, Quaternion rotation,
            out ICutter spawnedCutter)
        {
            // SetInternalPositionBeforeAssign(ref position, ref rotation);
            // spawnedCutter = Instantiate(Prefab, position, rotation).GetComponent<Cutter>();
            if (cutterFactory == null)
            {
                Debug.LogError("CutterFactory is NULL! Injection olmadı.");
                spawnedCutter = null;
                return;
            }

            spawnedCutter = cutterFactory.Create();
            spawnedCutter.GetTransform().position = position;
            spawnedCutter.GetTransform().rotation = rotation;
        }
    }
}