using System;
using System.Runtime.CompilerServices;
using UniRx;

namespace SignalsFramework {
   /// <summary>
   /// Simplification extensions for the <see cref="Subject{T}"/>
   /// </summary>
   public static class SignalExt {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Fire (this Subject<Unit> stream) => stream.OnNext(Unit.Default);
      
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Fire<T> (this Subject<T> stream, T data) => stream.OnNext(data);
   }

   /// <summary>
   /// Typed subject that passes data T+T1 via stream same way as subject does 
   /// </summary>
   /// <remarks>
   /// This one is suboptimal.
   /// Use a custom data struct to pass data around instead of passing directly as two parameters.
   /// </remarks>
   public class Subject<T, T1> : IObservable<Data<T, T1>> {
      private readonly Subject<Data<T, T1>> _sequence = new Subject<Data<T, T1>>();

      public IDisposable Subscribe(IObserver<Data<T, T1>> observer) {
         return _sequence.Subscribe(observer.OnNext);
      }

      public void Fire(T data, T1 data2) {
         _sequence.OnNext(new Data<T, T1> {X1 = data, X2 = data2});
      }
   }
   
   public struct Data<T, T1> {
      public T X1;
      public T1 X2;
   }
}