using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ModuleDistributor.Grpc;
using SEServer.Statements.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.GrpcServices
{
    [GrpcService]
    internal class TestGrpcService : TestService.TestServiceBase
    {
        private readonly ILogger<TestGrpcService> _logger;

        public TestGrpcService(ILogger<TestGrpcService> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public override Task<HelloWorldResponse> HelloWorld(Empty request, ServerCallContext context)
        {

            return Task.FromResult(new HelloWorldResponse
            {
                Message = "HelloWorld"
            });
        }

        [AllowAnonymous]
        public override async Task HelloWorldServerStream(Empty request, IServerStreamWriter<HelloWorldResponse> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
                await responseStream.WriteAsync(new HelloWorldResponse
                {
                    Message = "HelloWorld"
                });
        }

        [AllowAnonymous]
        public override async Task<Empty> HelloWorldClientStream(IAsyncStreamReader<HelloWorldRequest> requestStream, ServerCallContext context)
        {
            await foreach (var item in requestStream.ReadAllAsync())
                _logger.LogInformation(item.Message);

            return new Empty();
        }

        [AllowAnonymous]
        public override async Task HelloWorldBinaryStream(IAsyncStreamReader<HelloWorldRequest> requestStream, IServerStreamWriter<HelloWorldResponse> responseStream, ServerCallContext context)
        {
            await foreach (var item in requestStream.ReadAllAsync())    
                await responseStream.WriteAsync(new HelloWorldResponse { Message = item.Message });
        }
    }
}
