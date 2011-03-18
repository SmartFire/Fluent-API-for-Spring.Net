using FluentSpring.Context.Conventions;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ApplyConventionBinder : ICanRegisterConvention
    {
        private readonly ICanConfigureConvention _conventionParser;

        public ApplyConventionBinder(ICanConfigureConvention conventionParser)
        {
            _conventionParser = conventionParser;
        }

        public X Apply<X>() where X : IConvention, new()
        {
            X convention = new X();
            _conventionParser.AddConventionApplicant(convention);
            return convention;
        }
    }
}
