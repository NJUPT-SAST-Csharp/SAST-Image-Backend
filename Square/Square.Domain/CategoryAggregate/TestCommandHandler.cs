using Primitives.Command;

namespace Square.Domain.CategoryAggregate
{
    internal sealed class TestCommandHandler : ICommandRequestHandler<TestCommand>
    {
        public Task Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
