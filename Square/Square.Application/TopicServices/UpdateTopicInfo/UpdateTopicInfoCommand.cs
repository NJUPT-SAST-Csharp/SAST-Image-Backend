﻿using System.Security.Claims;
using Primitives.Command;
using Square.Domain;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.UpdateTopicInfo
{
    public sealed class UpdateTopicInfoCommand(
        long topicId,
        string title,
        string description,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public TopicId TopicId { get; } = new(topicId);

        public TopicTitle Title { get; } = new(title);

        public TopicDescription Description { get; } = new(description);

        public RequesterInfo Requester { get; } = new(user);
    }
}
