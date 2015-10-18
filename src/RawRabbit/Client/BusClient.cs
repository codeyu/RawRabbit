﻿using System;
using System.Threading.Tasks;
using RawRabbit.Common;
using RawRabbit.Common.Operations;
using RawRabbit.Core.Client;
using RawRabbit.Core.Configuration.Publish;
using RawRabbit.Core.Configuration.Respond;
using RawRabbit.Core.Configuration.Subscribe;
using RawRabbit.Core.Message;

namespace RawRabbit.Client
{
	public class BusClient : IBusClient
	{
		private readonly IConfigurationEvaluator _configEval;
		private readonly ISubscriber _subscriber;
		private readonly IPublisher _publisher;
		private readonly IResponder _responder;

		public BusClient(
			IConfigurationEvaluator configEval,
			ISubscriber subscriber,
			IPublisher publisher,
			IResponder responder)
		{
			_configEval = configEval;
			_subscriber = subscriber;
			_publisher = publisher;
			_responder = responder;
		}

		public Task SubscribeAsync<T>(Func<T, MessageInformation, Task> subscribeMethod, Action<ISubscriptionConfigurationBuilder> configuration = null) where T : MessageBase
		{
			var config = _configEval.GetConfiguration<T>(configuration);
			return _subscriber.SubscribeAsync(subscribeMethod, config);
		}

		public Task PublishAsync<T>(T message, Action<IPublishConfigurationBuilder> configuration = null) where T : MessageBase
		{
			var config = _configEval.GetConfiguration<T>(configuration);
			return _publisher.PublishAsync(message, config);
		}

		public Task RespondAsync<TRequest, TResponse>(Func<TRequest, MessageInformation, TResponse> onMessage, Action<IResponderConfigurationBuilder> configuration = null) where TRequest : MessageBase where TResponse : MessageBase
		{
			var config = _configEval.GetConfiguration<TRequest>(configuration);
			return _responder.RespondAsync(onMessage, config);
		}
	}
}
