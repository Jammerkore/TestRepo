using System;
using System.Collections;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace MIDRetail.DataCommon
{
	public class ClientIPInjectorSink : BaseChannelObjectWithProperties, IServerChannelSink
	{
		private IServerChannelSink _nextSink;

		public ClientIPInjectorSink(IServerChannelSink nextSink)
		{
			_nextSink = nextSink;
		}

		override public System.Collections.IDictionary Properties
		{
			get
			{
				return base.Properties;
			}
		}

		public System.Runtime.Remoting.Channels.IServerChannelSink NextChannelSink
		{
			get
			{
				return _nextSink;
			}
		}

		public void AsyncProcessResponse(
			IServerResponseChannelSinkStack sinkStack,
			object state,
			IMessage msg,
			ITransportHeaders headers,
			System.IO.Stream stream)
		{
			IPAddress IPAddr;

			try
			{
				IPAddr = (IPAddress)headers[CommonTransportKeys.IPAddress];
				CallContext.SetData("ClientIP", IPAddr);
			}
			catch (Exception)
			{
			}

			sinkStack.AsyncProcessResponse(msg, headers, stream);
		}

		public System.IO.Stream GetResponseStream(System.Runtime.Remoting.Channels.IServerResponseChannelSinkStack sinkStack, object state, System.Runtime.Remoting.Messaging.IMessage msg, System.Runtime.Remoting.Channels.ITransportHeaders headers)
		{
			return null;
		}

		public System.Runtime.Remoting.Channels.ServerProcessing ProcessMessage(
			IServerChannelSinkStack sinkStack,
			IMessage requestMsg,
			ITransportHeaders requestHeaders,
			System.IO.Stream requestStream,
			out IMessage responseMsg,
			out ITransportHeaders responseHeaders,
			out System.IO.Stream responseStream)
		{

			IPAddress IPAddr;
			ServerProcessing srvProc;

			try
			{
				IPAddr = (IPAddress)requestHeaders[CommonTransportKeys.IPAddress];
				CallContext.SetData("ClientIP", IPAddr);
			}
			catch (Exception)
			{
			}

			sinkStack.Push(this, null);

			srvProc = _nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);

			if (srvProc == ServerProcessing.Complete)
			{
			}
			
			return srvProc;
		}
	}

	public class ClientIPInjectorSinkProvider : IServerChannelSinkProvider
	{
		private IServerChannelSinkProvider _nextProvider;

		public ClientIPInjectorSinkProvider()
		{
		}

		public ClientIPInjectorSinkProvider(IDictionary properties, ICollection ByValproviderdata)
		{
		}

		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			return new ClientIPInjectorSink(_nextProvider.CreateSink(channel));
		}

		public IServerChannelSinkProvider Next
		{
			get
			{
				return _nextProvider;
			}
			set
			{
				_nextProvider = value;
			}
		}

		public void GetChannelData(IChannelDataStore channelData)
		{
		}
	}
}
