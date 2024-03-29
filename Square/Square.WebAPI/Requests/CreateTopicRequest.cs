﻿namespace Square.WebAPI.Requests
{
    public sealed class CreateTopicRequest
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string MainColumnText { get; init; }
        public IFormFileCollection Images { get; init; }
    }
}
