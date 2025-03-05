using UnityEngine;

namespace UpdateSys {
   public static class UpdateSystemExt {
      /// <summary>
      /// Enables OnSystemUpdate to be executed on IUpdatable
      /// </summary>
      public static void StartUpdate(this IUpdatable element) => UpdateSystem.StartUpdate(element);

      /// <summary>
      /// Enables OnSystemUpdate to be executed on IUpdatable
      /// </summary>
      public static void StopUpdate(this IUpdatable element) => UpdateSystem.StopUpdate(element);

      /// <summary>
      /// Enables OnSystemFixedUpdate to be executed on IUpdatable
      /// </summary>
      public static void StartFixedUpdate(this IFixedUpdatable element) => UpdateSystem.StartFixedUpdate(element);

      /// <summary>
      /// Disables OnSystemFixedUpdate
      /// </summary>
      public static void StopFixedUpdate(this IFixedUpdatable element) => UpdateSystem.StopFixedUpdate(element);

      /// <summary>
      /// Enables OnSystemLateUpdate
      /// </summary>
      public static void StartLateUpdate(this ILateUpdatable element) => UpdateSystem.StartLateUpdate(element);

      /// <summary>
      /// Disables OnSystemFixedUpdate
      /// </summary>
      public static void StopLateUpdate(this ILateUpdatable element) => UpdateSystem.StopLateUpdate(element);


      /// <summary>
      /// Attempts to start all available updates
      /// </summary>
      public static void StartAllUpdates(this IAnyUpdatable element) {
         IUpdatable updatable = element as IUpdatable;
         updatable?.StartUpdate();

         IFixedUpdatable fixedUpdatable = element as IFixedUpdatable;
         fixedUpdatable?.StartFixedUpdate();

         ILateUpdatable lateUpdatable = element as ILateUpdatable;
         lateUpdatable?.StartLateUpdate();
      }

      /// <summary>
      /// Attempts to stop all available updates
      /// </summary>
      public static void StopAllUpdates(this IAnyUpdatable element) {
         IUpdatable updatable = element as IUpdatable;
         updatable?.StopUpdate();

         IFixedUpdatable fixedUpdatable = element as IFixedUpdatable;
         fixedUpdatable?.StopFixedUpdate();

         ILateUpdatable lateUpdatable = element as ILateUpdatable;
         lateUpdatable?.StopLateUpdate();
      }

      /// <summary>
      /// Attempts to start all available updates
      /// </summary>
      public static void StartAllUpdates(this MonoBehaviour element) {
         IUpdatable updatable = element as IUpdatable;
         updatable?.StartUpdate();

         IFixedUpdatable fixedUpdatable = element as IFixedUpdatable;
         fixedUpdatable?.StartFixedUpdate();

         ILateUpdatable lateUpdatable = element as ILateUpdatable;
         lateUpdatable?.StartLateUpdate();
      }

      /// <summary>
      /// Attempts to stop all available updates
      /// </summary>
      public static void StopAllUpdates(this MonoBehaviour element) {
         IUpdatable updatable = element as IUpdatable;
         updatable?.StopUpdate();

         IFixedUpdatable fixedUpdatable = element as IFixedUpdatable;
         fixedUpdatable?.StopFixedUpdate();

         ILateUpdatable lateUpdatable = element as ILateUpdatable;
         lateUpdatable?.StopLateUpdate();
      }
   }
}