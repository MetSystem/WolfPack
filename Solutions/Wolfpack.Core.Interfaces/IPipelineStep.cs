using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface IPipelineStep<T>
    {
        void PreValidate(T context);
        void Execute(T context);
        void PostValidate(T context);

        bool ShouldExecute(T context);
        StepResult<T> GenerateResult(int index);
    }
}