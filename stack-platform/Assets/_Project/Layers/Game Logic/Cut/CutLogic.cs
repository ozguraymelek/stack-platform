
using System.Threading.Tasks;
using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Cut
{
    public class CutLogic : MonoBehaviour
    {
        private PlatformTracker _platformTracker;
        private CutLogicData _cutLogicData;
        private CutterObjectConfig _cutterObjectConfig;
        private IAlignment _alignment;
        
        
        public ICutter CurrentCutter;
        
        [Inject]
        public void Construct(PlatformTracker platformTracker, CutLogicData cutLogicData, CutterObjectConfig cutterObjectConfig, IAlignment alignment)
        {
            _platformTracker = platformTracker;
            _cutLogicData = cutLogicData;
            _cutterObjectConfig = cutterObjectConfig;
            _alignment = alignment;
        }
        
        private void Pretreatment()
        {
            FindCornerVertices();
            FindAngles();
            CutterNextLocation();
        }

        private void FindCornerVertices()
        {
            SRender.AnyObjectCornerVertexLocation(_platformTracker.CurrentPlatform.GetRenderer(), out _cutLogicData.CurrentPlatform.Location.ForwardLeft, 
                out _cutLogicData.CurrentPlatform.Location.ForwardRight, VertexLocation.LeftForward, VertexLocation.RightForward);
            
            SRender.AnyObjectCornerVertexLocation(_platformTracker.NextPlatform.GetRenderer(), out _cutLogicData.NextPlatform.Location.BackwardLeft, 
                out _cutLogicData.NextPlatform.Location.BackwardRight, VertexLocation.LeftBackward, VertexLocation.RightBackward);
        }
        
        private void FindAngles()
        {
            _cutLogicData.CurrentPlatform.Angle.WithForwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.CurrentPlatform.Location.ForwardLeft, _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicData.CurrentPlatform.Angle.WithForwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.CurrentPlatform.Location.ForwardRight, _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            
            _cutLogicData.NextPlatform.Angle.WithBackwardLeft = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.NextPlatform.Location.BackwardLeft, _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
            _cutLogicData.NextPlatform.Angle.WithBackwardRight = SMath.AngleBetweenSpecificLocations(
                _cutLogicData.NextPlatform.Location.BackwardRight, _platformTracker.CurrentPlatform.GetTransform().position, wantConjugateAngle: false);
        }

        private void CutterNextLocation()
        {
            if (_cutLogicData.NextPlatform.Location.BackwardRight.x < _cutLogicData.CurrentPlatform.Location.ForwardLeft.x
                || _cutLogicData.NextPlatform.Location.BackwardLeft.x > _cutLogicData.CurrentPlatform.Location.ForwardRight.x)
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
                    // LastHullWidth.Value = STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    // _platformTracker.NextPlatform.Outline.gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    //call sound
                    // SoundManager.PlayNoteOnStreak?.Invoke(PerfectIntersectionStreak.Value);
                    //call text
                    _cutLogicData.AlignmentToleranceBoundLeft = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                // SoundManager.ResetOnStreakLost?.Invoke();

                Debug.Log("NOT Perfect Alignment on left");
                _cutterObjectConfig.SpawnCutter(_cutLogicData.CurrentPlatform.Location.ForwardLeft,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                // if (CurrentCutter != null)
                //     CurrentCutter.ExternalCut(CurrentCutter.transform, _platformRuntimeData.NextPlatform,
                //         FellHullSide.Left,
                //         _platformRuntimeData.NextPlatformRenderer.material);
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
                    // LastHullWidth.Value = STransform.GetAnyObjectWidth(_platformRuntimeData.NextPlatform.gameObject);
                    _alignment.AlignPlatform(_platformTracker.NextPlatform.GetTransform(),
                        _platformTracker.CurrentPlatform.GetTransform());
                    // _platformTracker.NextPlatform.Outline.gameObject.SetActive(true);
                    _alignment.PerfectIntersectionStreak++;
                    // //call sound
                    // SoundManager.PlayNoteOnStreak?.Invoke(PerfectIntersectionStreak.Value);
                    //call text
                    _cutLogicData.AlignmentToleranceBoundRight = .25f;

                    return;
                }

                _alignment.PerfectIntersectionStreak = 0;
                // SoundManager.ResetOnStreakLost?.Invoke();

                Debug.Log("NOT Perfect Alignment on right");
                _cutterObjectConfig.SpawnCutter(_cutLogicData.CurrentPlatform.Location.ForwardRight,
                    Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                // if (CurrentCutter != null)
                    // CurrentCutter.ExternalCut(CurrentCutter.transform, _platformRuntimeData.NextPlatform,
                    //     FellHullSide.Right,
                    //     _platformTracker.NextPlatform.GetRenderer().material);
            }
        }
    }
}