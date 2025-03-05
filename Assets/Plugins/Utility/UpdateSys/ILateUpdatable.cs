namespace UpdateSys {
   public interface ILateUpdatable : IAnyUpdatable {
      void OnSystemLateUpdate(float deltaTime);
   }
}
