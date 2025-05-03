
using System;
using System.Threading.Tasks;
using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Infrastructure.Pools;
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
        private IObjectPool _platformPool;
        public ICutter CurrentCutter;

        private Cutter.Factory _cutterFactory;

        public float LastHullWidth;


        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker, CutLogicData cutLogicData,
            CutterObjectConfig cutterObjectConfig, IAlignment alignment, IObjectPool pool, Cutter.Factory cutterFactory)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
            _cutLogicData = cutLogicData;
            _cutterObjectConfig = cutterObjectConfig;
            _alignment = alignment;
            _platformPool = pool;
            _cutterFactory = cutterFactory;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_platformTracker), _platformTracker),
                (nameof(_cutLogicData), _cutLogicData),
                (nameof(_cutterObjectConfig), _cutterObjectConfig),
                (nameof(_alignment), _alignment),
                (nameof(_platformPool), _platformPool),
                (nameof(_cutterFactory), _cutterFactory)
            );
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
                Debug.LogWarning($"[{GetType().Name}].Pretreatment canceled because NextPlatform not yet tracked! " +
                                 "It can be fixed, but it will not cause any issues as it is set to not affect the game logic.");
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
                Debug.LogWarning("There is no intersection");
                _alignment.PerfectIntersectionStreak = 0;
                _platformPool.Release(_platformTracker.NextPlatform);
                return;
            }

            if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft < _cutLogicData.CurrentPlatform.Angle.WithForwardLeft)
            {
                if (_alignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardLeft,
                        _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                        _cutLogicData.AlignmentToleranceBoundLeft))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    _platformTracker.NextPlatform.GetOutline().gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    _cutLogicData.AlignmentToleranceBoundLeft = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();
;
                SpawnCutter(_cutterFactory, _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                CurrentCutter?.ExternalCut(CurrentCutter.GetTransform(), _platformTracker.NextPlatform,
                    FellHullSide.Left,
                    _platformTracker.NextPlatform.GetRenderer().material);
            }

            else if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft <
                     Mathf.Abs(_cutLogicData.CurrentPlatform.Angle.WithForwardLeft))
            {
                if (_alignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardRight,
                        _cutLogicData.CurrentPlatform.Location.ForwardRight,
                        _cutLogicData.AlignmentToleranceBoundRight))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    _platformTracker.NextPlatform.GetOutline().gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    _cutLogicData.AlignmentToleranceBoundRight = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();

                SpawnCutter(_cutterFactory, _cutLogicData.CurrentPlatform.Location.ForwardRight,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                CurrentCutter?.ExternalCut(CurrentCutter.GetTransform(), _platformTracker.NextPlatform,
                    FellHullSide.Right,
                    _platformTracker.NextPlatform.GetRenderer().material);
            }

        }
        
        private void SpawnCutter(Cutter.Factory cutterFactory, Vector3 position, Quaternion rotation,
            out ICutter spawnedCutter)
        {
            if (cutterFactory == null)
            {
                Debug.LogError("Cutter.Factory is not injected");
                spawnedCutter = null;
                return;
            }

            spawnedCutter = cutterFactory.Create();
            spawnedCutter.GetTransform().position = position;
            spawnedCutter.GetTransform().rotation = rotation;
        }
    }
}