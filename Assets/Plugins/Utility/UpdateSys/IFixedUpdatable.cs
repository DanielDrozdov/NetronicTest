namespace UpdateSys {
   public interface IFixedUpdatable : IAnyUpdatable {
      void OnSystemFixedUpdate(float deltaTime);
   }
}
