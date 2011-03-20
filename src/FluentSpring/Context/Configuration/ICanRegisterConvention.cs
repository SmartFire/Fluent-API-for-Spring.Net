
using FluentSpring.Context.Conventions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanRegisterConvention
    {
        /// <summary>
        /// Determines which convention to apply.
        /// The <typeparamref name="X"/> will determine which implemented IConvention to apply.
        /// </summary>
        /// <typeparam name="X">This must implement IConvention interface</typeparam>
        /// <returns></returns>
        X Apply<X>() where X : IConvention, new();
    }
}
