using UnityEngine;

namespace Pooling {
   public interface IGenericPoolElement {
      int PoolRef { get; set; }
      bool IsCommissioned { get; set; }
      bool IsDestroyed { get; }
      GameObject GameObject { get; }
      Transform PoolParentTrm { get; set; }
      void Commission();
      void Decommission();
   }
}