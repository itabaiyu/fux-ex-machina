using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Decorators
{
    public interface ICompositionDecorator
    {
        /// <summary>
        /// Decorates a given composition with musical ornamentation.
        /// </summary>
        /// <param name="composition">The composition to decorate</param>
        void DecorateComposition(Composition composition);
    }
}