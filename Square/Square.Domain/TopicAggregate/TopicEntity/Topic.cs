using FoxResult;
using Primitives.Entity;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Domain.TopicAggregate.Commands.CreateTopic;
using Square.Domain.TopicAggregate.Commands.DeleteTopic;
using Square.Domain.TopicAggregate.Commands.SubscribeTopic;
using Square.Domain.TopicAggregate.Commands.UnsubscribeTopic;
using Square.Domain.TopicAggregate.Commands.UpdateTopicInfo;
using Square.Domain.TopicAggregate.Events;
using Utilities;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>
{
    private Topic()
        : base(default) { }

    private Topic(UserId authorId, TopicTitle title, CategoryId categoryId)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _categoryId = categoryId;
        _authorId = authorId;
        _title = title;
    }

    #region Fields

    private readonly CategoryId _categoryId;

    private readonly UserId _authorId;

    private readonly TopicTitle _title;

    private readonly List<TopicSubscribe> _subscribers = [];

    #endregion

    #region Methods

    internal static async Task<Result<Topic>> CreateNewTopicAsync(
        CreateTopicCommand command,
        ITopicUniquenessChecker checker,
        ITopicRepository repository
    )
    {
        if (await checker.IsConflictAsync(command.Title))
        {
            return Result.Fail(Error.Conflict<Topic>());
        }

        Topic topic = new(command.Requester.Id, command.Title, command.CategoryId);

        topic.AddDomainEvent(new TopicCreatedEvent(command, topic.Id));

        repository.AddTopic(topic);

        return Result.Return(topic);
    }

    internal async Task<Result> UpdateTopicInfoAsync(
        UpdateTopicInfoCommand command,
        ITopicUniquenessChecker checker
    )
    {
        if (await checker.IsConflictAsync(command.Title))
        {
            return Result.Fail(Error.Conflict<Topic>());
        }

        if (command.Requester.Id == _authorId || command.Requester.IsAdmin)
        {
            AddDomainEvent(new TopicInfoUpdatedEvent(Id, command.Title, command.Description));
            return Result.Success;
        }

        return Result.Fail(Error.Forbidden);
    }

    internal void Subscribe(SubscribeTopicCommand command)
    {
        if (_subscribers.Any(subscriber => subscriber.UserId == command.Requester.Id))
        {
            return;
        }

        _subscribers.Add(new(command.Requester.Id, Id));

        AddDomainEvent(new TopicSubscribedEvent(Id, command.Requester.Id));
    }

    internal void Unsubscribe(UnsubscribeTopicCommand command)
    {
        int number = _subscribers.RemoveAll(x => x.UserId == command.Requester.Id);
        if (number > 0)
        {
            AddDomainEvent(new TopicUnsubscribedEvent(Id, command.Requester.Id));
        }
    }

    internal Result DeleteTopic(DeleteTopicCommand command, ITopicRepository repository)
    {
        if (command.Requester.Id == _authorId || command.Requester.IsAdmin)
        {
            repository.DeleteTopic(this);
            AddDomainEvent(new TopicDeletedEvent(Id));
            return Result.Success;
        }

        return Result.Fail(Error.Forbidden);
    }

    #endregion
}
