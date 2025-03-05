using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pooling 
{
   public class GenericPool 
   {
      #region [Properties]

      public AssetReference PrefabAssetReference { get; }
      public Transform ParentTransform { get; }
      public bool HasAvailableElements => AvailableElements.Count > 0;
      public int TotalElementsCount => InUseElements.Count + AvailableElements.Count;

      #endregion

      #region [Fields]

      private readonly HashSet<IGenericPoolElement> InUseElements = new HashSet<IGenericPoolElement>();
      private readonly HashSet<IGenericPoolElement> AvailableElements = new HashSet<IGenericPoolElement>();

      #endregion
      

      public GenericPool(AssetReference prefabAssetReference, Transform poolParentTrm)
      {
         PrefabAssetReference = prefabAssetReference;
         ParentTransform = poolParentTrm;
      }
      
      public T PoolFirst<T>() where T : MonoBehaviour {
         IGenericPoolElement element = null;
         
         foreach (IGenericPoolElement el in AvailableElements) {
            element = el;
            break;
         }
         
         AvailableElements.Remove(element);
         InUseElements.Add(element);

         return element as T;
      }

      public void AddToPool(IGenericPoolElement poolElement) {
         poolElement.PoolRef = GetHashCode();
         poolElement.PoolParentTrm = ParentTransform;
         AvailableElements.Add(poolElement);
      }

      public void ReturnToPool(IGenericPoolElement poolElement) {
         Debug.Assert(!AvailableElements.Contains(poolElement),
                      "GenericPool:: This element is already in the pool. "
                      + "This may cause some bugs, consider not decommissioning twice!");
         
         AvailableElements.Add(poolElement);
         InUseElements.Remove(poolElement);
         
         poolElement.IsCommissioned = false;
      }

      public void RemoveFromPool(IGenericPoolElement element) {
         AvailableElements.Remove(element);
         InUseElements.Remove(element);
      }
      
      public void ClearDeletedObjectsReferences()
      {
         var toRemove = GetElementsToRemove();
         
         foreach (IGenericPoolElement elementToRemove in toRemove)
         {
            AvailableElements.Remove(elementToRemove);
         }
      }

      private HashSet<IGenericPoolElement> GetElementsToRemove()
      {
         HashSet<IGenericPoolElement> toRemove = new HashSet<IGenericPoolElement>();
         
         foreach (IGenericPoolElement element in InUseElements)
         {
            if (element.IsDestroyed)
            {
               toRemove.Add(element);
               continue;
            }
            
            AvailableElements.Add(element);
         }
         
         InUseElements.Clear();

         foreach (IGenericPoolElement element in AvailableElements)
         {
            if (!element.IsDestroyed) continue;
            
            toRemove.Add(element);
         }
         
         return toRemove;
      }
   }
}