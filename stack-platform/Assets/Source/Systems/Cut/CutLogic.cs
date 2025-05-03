using Source.Core.Utilities.External;
using Source.Data.Cut;
using Source.Data.Platform;
using Source.Gameplay.Platform.Wrappers;
using Source.Infrastructure.Pools;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Systems.Cut
{
    public class CutLogic : MonoBehaviour
    {
        private SignalBus _signalBus;

        private PlatformTracker _platformTracker;
        private CutLogicConfig _cutLogicConfig;
        private IAlignment _alignment;
        private IObjectPool _platformPool;
        public ICutter CurrentCutter;

        private Cutter.Factory _cutterFactory;

        public float LastHullWidth;
        
        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker, CutLogicConfig cutLogicConfig, IAlignment alignment, IObjectPool pool, Cutter.Factory cutterFactory)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
            _cutLogicConfig = cutLogicConfig;
            _alignment = alignment;
            _platformPool = pool;
            _cutterFactory = cutterFactory;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_platformTracker), _platformTracker),
                (nameof(_cutLogicConfig), _cutLogicConfig),
                (nameof(_alignment), _alignment),
                (nameof(_platformPool), _platformPool),
                (nameof(_cutterFactory), _cutterFactory)
            );
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<CutRequestSignal>(Pretreatment);
            LastHullWidth = 1.5f;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<CutRequestSignal>(Pretreatment);
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
                out _cutLogicConfig.CurrentPlatform.Location.ForwardLeft,
                out _cutLogicConfig.CurrentPlatform.Location.ForwardRight, VertexLocation.LeftForward,
                VertexLocation.RightForward);

            SRender.AnyObjectCornerVertexLocation(_platformTracker.NextPlatform.GetRenderer(),
                out _cutLogicConfig.NextPlatform.Location.BackwardLeft,
                out _cutLogicConfig.NextPlatform.Location.BackwardRight, VertexLocation.LeftBackward,
                VertexLocation.RightBackward);
        }

        private void FindAngles()
        {
            _cutLogicConfig.CurrentPlatform.Angle.WithForwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicConfig.CurrentPlatform.Location.ForwardLeft,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicConfig.CurrentPlatform.Angle.WithForwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicConfig.CurrentPlatform.Location.ForwardRight,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);

            _cutLogicConfig.NextPlatform.Angle.WithBackwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicConfig.NextPlatform.Location.BackwardLeft,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicConfig.NextPlatform.Angle.WithBackwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicConfig.NextPlatform.Location.BackwardRight,
                _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
        }

        private void CutterNextLocation()
        {
            if (_cutLogicConfig.NextPlatform.Location.BackwardRight.x <
                _cutLogicConfig.CurrentPlatform.Location.ForwardLeft.x
                || _cutLogicConfig.NextPlatform.Location.BackwardLeft.x >
                _cutLogicConfig.CurrentPlatform.Location.ForwardRight.x)
            {
                Debug.LogWarning("There is no intersection");
                _alignment.PerfectIntersectionStreak = 0;
                _platformPool.Release(_platformTracker.NextPlatform);
                return;
            }

            if (_cutLogicConfig.NextPlatform.Angle.WithBackwardLeft < _cutLogicConfig.CurrentPlatform.Angle.WithForwardLeft)
            {
                if (_alignment.IsTherePerfectAlignment(_cutLogicConfig.NextPlatform.Location.BackwardLeft,
                        _cutLogicConfig.CurrentPlatform.Location.ForwardLeft,
                        _cutLogicConfig.AlignmentToleranceBoundLeft))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    _platformTracker.NextPlatform.GetOutline().gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    _cutLogicConfig.AlignmentToleranceBoundLeft = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();
;
                SpawnCutter(_cutterFactory, _cutLogicConfig.CurrentPlatform.Location.ForwardLeft,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                CurrentCutter?.ExternalCut(CurrentCutter.GetTransform(), _platformTracker.NextPlatform,
                    FellHullSide.Left,
                    _platformTracker.NextPlatform.GetRenderer().material);
            }

            else if (_cutLogicConfig.NextPlatform.Angle.WithBackwardLeft <
                     Mathf.Abs(_cutLogicConfig.CurrentPlatform.Angle.WithForwardLeft))
            {
                if (_alignment.IsTherePerfectAlignment(_cutLogicConfig.NextPlatform.Location.BackwardRight,
                        _cutLogicConfig.CurrentPlatform.Location.ForwardRight,
                        _cutLogicConfig.AlignmentToleranceBoundRight))
                {
                    LastHullWidth =
                        STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.GetTransform().gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    _platformTracker.NextPlatform.GetOutline().gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    _signalBus.Fire(new StreakSignal(_alignment.PerfectIntersectionStreak));
                    _cutLogicConfig.AlignmentToleranceBoundRight = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                _signalBus.Fire<StreakLostSignal>();

                SpawnCutter(_cutterFactory, _cutLogicConfig.CurrentPlatform.Location.ForwardRight,
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