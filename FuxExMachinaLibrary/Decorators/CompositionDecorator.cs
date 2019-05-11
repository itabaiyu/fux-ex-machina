using System.Collections.Generic;
using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Decorators
{
    /// <summary>
    /// Class which decorates a given composition with various musical ornamentation.
    /// </summary>
    public class CompositionDecorator : ICompositionDecorator
    {
        /// <summary>
        /// The decorators to use.
        /// </summary>
        private readonly List<ICompositionDecorator> _decorators = new List<ICompositionDecorator>();

        /// <summary>
        /// CompositionDecorator constructor.
        /// </summary>
        /// <param name="passingToneDecorator">The PassingToneDecorator to use</param>
        /// <param name="mordentDecorator">The MordentDecorator to use</param>
        /// <param name="appogiaturaDecorator">The AppogiaturaDecorator to use</param>
        public CompositionDecorator(
            PassingToneDecorator passingToneDecorator,
            MordentDecorator mordentDecorator,
            AppogiaturaDecorator appogiaturaDecorator
        )
        {
            _decorators.AddRange(
                new List<ICompositionDecorator>
                {
                    passingToneDecorator,
                    mordentDecorator,
                    appogiaturaDecorator
                }
            );
        }

        /// <inheritdoc />
        /// <summary>
        /// Use each of the decorators to decorate the composition.
        /// </summary>
        /// <param name="composition">The composition to decorate</param>
        public void DecorateComposition(Composition composition) => _decorators.ForEach(
            decorator => decorator.DecorateComposition(composition)
        );
    }
}