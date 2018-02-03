using Rentitas;

namespace Revalue
{
    public interface IValueComponent<T> : IComponent
    {
        T Value { get; set; }
    }
    
    public abstract class RentitasValue<T> : IValueComponent<T>
    {
        public virtual T Value { get; set; }
    }
}