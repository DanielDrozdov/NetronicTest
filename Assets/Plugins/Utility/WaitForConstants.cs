using UnityEngine;

namespace Utility {
   public static class WaitForConstants {
      public static readonly WaitForSeconds PointOneSecond = new WaitForSeconds(0.1f);
      public static readonly WaitForSeconds QuarterOfSecond = new WaitForSeconds(0.25f);
      public static readonly WaitForSeconds HalfSecond = new WaitForSeconds(0.5f);
      public static readonly WaitForSeconds OneSecond = new WaitForSeconds(1f);
      public static readonly WaitForFixedUpdate ForFixedUpdate = new WaitForFixedUpdate();
      public static readonly WaitForEndOfFrame ForEndOfFrame = new WaitForEndOfFrame();
   }
}