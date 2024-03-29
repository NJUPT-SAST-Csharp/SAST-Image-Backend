using FoxResult;
using Primitives.Entity;
using Square.Domain.DomainServices.CreateTopic;
using Square.Domain.TopicAggregate.Commands.SubscribeTopic;
using Square.Domain.TopicAggregate.Commands.UnsubscribeTopic;
using Square.Domain.TopicAggregate.Commands.UpdateTopicInfo;
using Utilities;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>, ITopic
{
    private Topic()
        : base(default) { }

    private Topic(UserId authorId)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _authorId = authorId;
    }

    internal static ITopic CreateNewTopic(CreateTopicCommand command)
    {
        Topic topic = new(command.Requester.Id);

        //TODO: Raise domain event
        return topic;
    }

    #region Fields

    private readonly UserId _authorId;

    private readonly List<TopicSubscribe> _subscribers = [];

    #endregion

    #region Methods

    public Result UpdateTopicInfo(UpdateTopicInfoCommand command)
    {
        return Result.Success;
    }

    public void Subscribe(SubscribeTopicCommand command)
    {
        if (_subscribers.Any(subscriber => subscriber.UserId == command.Requester.Id))
        {
            return;
        }

        _subscribers.Add(new(command.Requester.Id, Id));
    }

    public void Unsubscribe(UnsubscribeTopicCommand command)
    {
        _subscribers.RemoveAll(x => x.UserId == command.Requester.Id);
    }

    public bool IsManagedBy(in RequesterInfo user)
    {
        return _authorId == user.Id || user.IsAdmin;
    }

    #endregion
}
