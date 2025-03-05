using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;

namespace UpdateSys
{
   /// <summary>
   /// Basic update manager that handles Update / FixedUpdate scenarios
   /// </summary>
   public static class UpdateSystem
   {
      public static float TimeScale { get; set; } = 1f;

      private static readonly HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
      private static readonly HashSet<IUpdatable> _updateInsertBuffer = new HashSet<IUpdatable>();
      private static readonly HashSet<IUpdatable> _updateRemoveBuffer = new HashSet<IUpdatable>();

      private static readonly HashSet<IFixedUpdatable> _fixedUpdatables = new HashSet<IFixedUpdatable>();
      private static readonly HashSet<IFixedUpdatable> _fixedUpdateInsertBuffer = new HashSet<IFixedUpdatable>();
      private static readonly HashSet<IFixedUpdatable> _fixedUpdateRemoveBuffer = new HashSet<IFixedUpdatable>();

      private static readonly HashSet<ILateUpdatable> _lateUpdatables = new HashSet<ILateUpdatable>();
      private static readonly HashSet<ILateUpdatable> _lateUpdateInsertBuffer = new HashSet<ILateUpdatable>();
      private static readonly HashSet<ILateUpdatable> _lateUpdateRemoveBuffer = new HashSet<ILateUpdatable>();

      private const string _updateMethodName = "Update";
      private const string _lateUpdateMethodName = "LateUpdate";
      private const string _fixedUpdateMethodName = "FixedUpdate";
      private const string _updateSystemProfilerName = "UpdateSystem";

      [RuntimeInitializeOnLoadMethod]
      private static void Init()
      {
         MainThreadDispatcher.StartUpdateMicroCoroutine(DoUpdateLogic(_updatables, _updateRemoveBuffer, _updateInsertBuffer, 
            ElementUpdate, () => Time.deltaTime, _updateMethodName));
         
         MainThreadDispatcher.StartFixedUpdateMicroCoroutine(DoUpdateLogic(_fixedUpdatables, _fixedUpdateRemoveBuffer, _fixedUpdateInsertBuffer, 
            ElementFixedUpdate, () => Time.fixedDeltaTime, _fixedUpdateMethodName));
         
         MainThreadDispatcher.StartLateUpdateMicroCoroutine(DoUpdateLogic(_lateUpdatables, _lateUpdateRemoveBuffer, _lateUpdateInsertBuffer, 
            ElementLateUpdate, () => Time.deltaTime, _lateUpdateMethodName));
      }

      private static void ElementUpdate(IUpdatable element, float deltaTime)
      {
         element.OnSystemUpdate(deltaTime);
      }

      private static void ElementLateUpdate(ILateUpdatable element, float deltaTime)
      {
         element.OnSystemLateUpdate(deltaTime);
      }

      private static void ElementFixedUpdate(IFixedUpdatable element, float deltaTime)
      {
         element.OnSystemFixedUpdate(deltaTime);
      }

      private static IEnumerator DoUpdateLogic<T>(HashSet<T> updatables, HashSet<T> removeBuffer, HashSet<T> insertBuffer,
         Action<T, float> doElementUpdateLogic, Func<float> getDeltaTime, string updatableName)
      {
         while (true)
         {
            Profiler.BeginSample(_updateSystemProfilerName);

            RemoveUnsubscribedUpdatables(updatables, removeBuffer);
            InsertNewUpdatables(updatables, insertBuffer);

            float deltaTime = getDeltaTime() * TimeScale;
            UpdateElements(updatables, removeBuffer, doElementUpdateLogic, updatableName, deltaTime);

            Profiler.EndSample();
            yield return null;
         }
      }

      private static void RemoveUnsubscribedUpdatables<T>(HashSet<T> updatables, HashSet<T> removeBuffer)
      {
         foreach (T element in removeBuffer)
         {
            updatables.Remove(element);
         }

         removeBuffer.Clear();
      }

      private static void InsertNewUpdatables<T>(HashSet<T> updatables, HashSet<T> insertBuffer)
      {
         foreach (T element in insertBuffer)
         {
            updatables.Add(element);
         }

         insertBuffer.Clear();
      }

      private static void UpdateElements<T>(HashSet<T> updatables, HashSet<T> removeBuffer, Action<T, float> doElementUpdateLogic,
         string updatableName, float deltaTime)
      {
         bool ifNeedToEnableTryCatchDebugging = false;
         
#if UNITY_EDITOR || DEVELOPMENT_BUILD
         ifNeedToEnableTryCatchDebugging = true;
#endif
         
         foreach (T element in updatables)
         {
            if (ifNeedToEnableTryCatchDebugging)
            {
               try
               {
                  doElementUpdateLogic(element, deltaTime);
               }
               catch (Exception ex)
               {
                  Debug.LogError($"[{updatableName}] Unhandled ex:: {ex.Message} Stack: {ex.StackTrace}");
                  removeBuffer.Add(element);
               }
            }
            else
            {
               doElementUpdateLogic(element, deltaTime);
            }
         }
      }


      /// <summary>
      /// Enables OnSystemUpdate to be executed on IUpdatable
      /// </summary>
      public static void StartUpdate(IUpdatable updatable)
      {
         _updateInsertBuffer.Add(updatable);
         _updateRemoveBuffer.Remove(updatable);
      }

      /// <summary>
      /// Disables OnSystemUpdate on IUpdatable 
      /// </summary>
      public static void StopUpdate(IUpdatable updatable)
      {
         _updateRemoveBuffer.Add(updatable);
         _updateInsertBuffer.Remove(updatable);
      }

      /// <summary>
      /// Enables OnSystemUpdate to be executed on IFixedUpdatable
      /// </summary>
      public static void StartFixedUpdate(IFixedUpdatable updatable)
      {
         _fixedUpdateInsertBuffer.Add(updatable);
         _fixedUpdateRemoveBuffer.Remove(updatable);
      }

      /// <summary>
      /// Disables OnSystemFixedUpdate on IFixedUpdatable
      /// </summary>
      public static void StopFixedUpdate(IFixedUpdatable updatable)
      {
         _fixedUpdateRemoveBuffer.Add(updatable);
         _fixedUpdateInsertBuffer.Remove(updatable);
      }

      /// <summary>
      /// Enables OnSystemLateUpdate on ILateUpdatable
      /// </summary>
      public static void StartLateUpdate(ILateUpdatable updatable)
      {
         _lateUpdateInsertBuffer.Add(updatable);
         _lateUpdateRemoveBuffer.Remove(updatable);
      }

      /// <summary>
      /// Disables OnSystemLateUpdate on ILateUpdatable
      /// </summary>
      public static void StopLateUpdate(ILateUpdatable updatable)
      {
         _lateUpdateRemoveBuffer.Add(updatable);
         _lateUpdateInsertBuffer.Remove(updatable);
      }
   }
}