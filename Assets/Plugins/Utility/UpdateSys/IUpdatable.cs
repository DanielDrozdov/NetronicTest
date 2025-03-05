namespace UpdateSys {
   public interface IUpdatable : IAnyUpdatable {
      void OnSystemUpdate(float deltaTime);
   }
}
