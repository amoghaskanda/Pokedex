namespace Pokedex.Tests
{
    public class MockMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _responseMessage;

        public MockMessageHandler(HttpResponseMessage response)
        {
            _responseMessage = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseMessage);
        }
        public new Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken);
        }

    }
}
