
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
        
        [Inject]
        public void Construct(PlatformTracker platformTracker, CutLogicData cutLogicData)
        {
            _platformTracker = platformTracker;
            _cutLogicData = cutLogicData;
        }
        
        private void Pretreatment()
        {
            FindCornerVertices();
            FindAngles();
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

        private void FindCutterNextLocation()
        {
            if (_cutLogicData.NextPlatform.Location.BackwardRight.x < _cutLogicData.CurrentPlatform.Location.ForwardLeft.x
                || _cutLogicData.NextPlatform.Location.BackwardLeft.x > _cutLogicData.CurrentPlatform.Location.ForwardRight.x)
            {
                Debug.LogError("There is no intersection");
                // PerfectIntersectionStreak.Value = 0;
                // ObjectPool.Enqueue(_platformRuntimeData.NextPlatform, "Platform");
                // IsThereIntersection.Value = false;
                return;
            }

            if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft < _cutLogicData.CurrentPlatform.Angle.WithForwardLeft)
            {
                // Debug.Log("platform on the left by player");
                // IsThereIntersection.Value = true;
                // //TODO: add tolerance to perfect platform aligning
                // if (_perfectAlignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardLeft,
                //         _cutLogicData.CurrentPlatform.Location.ForwardLeft,
                //         _cutLogicData.AlignmentToleranceBoundLeft))
                {
                    // LastHullWidth.Value = STransform.GetAnyObjectWidth(_platformTracker.NextPlatform.gameObject);
                    // _perfectAlignment.AlignPlatform(_platformRuntimeData.NextPlatform.transform,
                    //     _platformRuntimeData.CurrentPlatform.transform);
                    // _platformRuntimeData.NextPlatform.Outline.gameObject.SetActive(true);
                    // PerfectIntersectionStreak.Value++;
                    //call sound
                    // SoundManager.PlayNoteOnStreak?.Invoke(PerfectIntersectionStreak.Value);
                    //call text
                    // CutLogicData.AlignmentToleranceBoundLeft = .25f;

                    return;
                }

                // PerfectIntersectionStreak.Value = 0;
                // SoundManager.ResetOnStreakLost?.Invoke();

                // Debug.Log("NOT Perfect Alignment on left");
                // CutterObjectData.SpawnCutter(CutLogicData.CurrentPlatform.Location.UpperLeft,
                //     Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                // if (CurrentCutter != null)
                //     CurrentCutter.ExternalCut(CurrentCutter.transform, _platformRuntimeData.NextPlatform,
                //         FellHullSide.Left,
                //         _platformRuntimeData.NextPlatformRenderer.material);
            }

            else if (_cutLogicData.NextPlatform.Angle.WithBackwardLeft <
                     Mathf.Abs(_cutLogicData.CurrentPlatform.Angle.WithForwardRight))
            {
                // Debug.Log("platform on the right by player");
                // IsThereIntersection.Value = true;
                //TODO: add tolerance to perfect platform aligning
                // if (_perfectAlignment.IsTherePerfectAlignment(_cutLogicData.NextPlatform.Location.BackwardRight,
                //         _cutLogicData.CurrentPlatform.Location.ForwardRight,
                //         _cutLogicData.AlignmentToleranceBoundRight))
                {
                    // LastHullWidth.Value = STransform.GetAnyObjectWidth(_platformRuntimeData.NextPlatform.gameObject);
                    // _perfectAlignment.AlignPlatform(_platformRuntimeData.NextPlatform.transform,
                    //     _platformRuntimeData.CurrentPlatform.transform);
                    // _platformRuntimeData.NextPlatform.Outline.gameObject.SetActive(true);
                    // PerfectIntersectionStreak.Value++;
                    // //call sound
                    // SoundManager.PlayNoteOnStreak?.Invoke(PerfectIntersectionStreak.Value);
                    //call text
                    // CutLogicData.AlignmentToleranceBoundRight = .25f;

                    return;
                }

                // PerfectIntersectionStreak.Value = 0;
                // SoundManager.ResetOnStreakLost?.Invoke();

                // Debug.Log("NOT Perfect Alignment on right");
                // CutterObjectData.SpawnCutter(_cutLogicData.CurrentPlatform.Location.ForwardRight,
                //     Quaternion.Euler(0.0f, 0.0f, 90.0f), out CurrentCutter);

                // if (CurrentCutter != null)
                //     CurrentCutter.ExternalCut(CurrentCutter.transform, _platformRuntimeData.NextPlatform,
                //         FellHullSide.Right,
                //         _platformTracker.NextPlatform.GetRenderer().material);
            }
        }
    }
}