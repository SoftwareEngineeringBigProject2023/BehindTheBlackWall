## 接口定义格式：
在导入unitypackage后, 可以看Protos文件夹下的Proto文件，下面将介绍proto文件的简单结构

#### 一个proto文件，以测试服务为例：

    syntax = "proto3";

    import "google/protobuf/empty.proto";
    option csharp_namespace = "SEServer.Statements.Domain.Shared";

    package test;

    service TestService {
        // 客户端向服务器发送请求, 服务器返回一个应答
        rpc HelloWorld (google.protobuf.Empty) returns (HelloWorldResponse);

        // 客户端向服务器发送请求, 服务器返回数个应答
	    rpc HelloWorldServerStream (google.protobuf.Empty) returns (stream HelloWorldResponse);
	    
        // 客户端向服务器发送数个请求, 服务器返回一个应答
        rpc HelloWorldClientStream (stream HelloWorldRequest) returns (google.protobuf.Empty);
	    
        // 客户端向服务器发送数个请求, 服务器返回数个应答
        rpc HelloWorldBinaryStream (stream HelloWorldRequest) returns (stream HelloWorldResponse);

        // 总而言之，要发送数个对象时，定义中就会在类型前面加一个stream
    }

    // 定义请求消息，都是基础类型，名称稍有不同，比如：int64、int32之类的
    message HelloWorldRequest
    {
	    string Message = 1;
    }

    // 定义响应消息，都是基础类型，名称稍有不同，比如：int64、int32之类的
    message HelloWorldResponse
    {
	    string Message = 1;
    }

## 使用方法示例：

#### 一个unity的测试脚本，以测试服务为例：

    using Grpc.Net.Client;
    using UnityEngine;
    using SEServer.Statements.Domain.Shared;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using Cysharp.Threading.Tasks;
    using System;

    public class TestBehaviour : MonoBehaviour
    {
        // 创建GrpcChannel, 可以重复使用
        private static readonly GrpcChannel _statementsChannel
            = GrpcChannel.ForAddress("http://localhost:5104", new GrpcChannelOptions
            {
                HttpHandler = new PrefixBridgeHandler("statements")
            });

        // 一个空白的请求, 可以重复使用
        private static readonly Empty _empty = new Empty();

        // 创建客户端对象, 可以重复使用
        private static readonly TestService.TestServiceClient _testClient = new TestService.TestServiceClient(_statementsChannel);


        private async void Start()
        {
            await TestHelloWorld();
            await TestServerStream();
            await TestClientStream();
            await TestBinaryStream();
        }
    
        // 客户端向服务器发送请求, 服务器返回一个应答
        private async UniTask TestHelloWorld()
        {
            var response = await _testClient.HelloWorldAsync(_empty);
            Debug.Log($"{nameof(TestService.TestServiceClient.HelloWorldAsync)}: {response.Message}");
        }

        // 客户端向服务器发送请求, 服务器返回数个应答
        private async UniTask TestServerStream()
        {
            var serverStreamCall = _testClient.HelloWorldServerStream(_empty);
            await foreach (var item in serverStreamCall.ResponseStream.ReadAllAsync())
                Debug.Log($"{nameof(TestService.TestServiceClient.HelloWorldServerStream)}: {item.Message}");
        }

        // 客户端向服务器发送数个请求, 服务器返回一个应答
        private async UniTask TestClientStream()
        {
            var clientStreamCall = _testClient.HelloWorldClientStream();
            for (int i = 0; i < 10; i++)
                await clientStreamCall.RequestStream.WriteAsync(new HelloWorldRequest
                {
                    Message = "HelloWorld"
                });
        }

        // 客户端向服务器发送数个请求, 服务器返回数个应答
        private async UniTask TestBinaryStream()
        {
            var binaryStream = _testClient.HelloWorldBinaryStream();
            for (int i = 0; i < 10; i++)
                await binaryStream.RequestStream.WriteAsync(new HelloWorldRequest
                {
                    Message = "HelloWorld"
                });

            await foreach (var item in binaryStream.ResponseStream.ReadAllAsync())
                Debug.Log($"{nameof(TestService.TestServiceClient.HelloWorldBinaryStream)}: {item.Message}");
        }
    }

注意尽量使用后缀带Async的异步方法以优化性能

## 接口含义说明：

主要是针对一些含义可能不够清楚的接口进行说明

### 授权流程：

由于io游戏的性质, 该授权非常简单, 就是用户名密码授权, 并取消了时限, 力求点击就玩。在调用SignInAsync或RegisterAsync后将得到JwtToken, 获取到的token需要储存起来, 之后其他服务调用则都需要授权。具体token使用例子如下：
    
    // 注册获取JwtToken
    GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5104", 
        new GrpcChannelOptions
        {
            HttpHandler = new PrefixBridgeHandler("auth")
        });

    var client = new AuthenticationService.AuthenticationServiceClient(channel);

    var response = await client.RegisterAsync(new SignInRequest
    {
        UserName = "test",
        Password = "test"
    });

    // 下面展示带token去调用方法
    var headers = new Metadata
    {
        { "Authorization", $"Bearer {response.AccessToken}" }
    };

    _ = await _testClient.HelloWorldAsync(_empty, headers);

### 排行榜搜索：

以GetRankListByScoreRequest为例, 其中SearchName为null或""时是无搜索条件的

GetRankListByKDAAsync获取击杀榜

GetRankListByScoreAsync获取分数榜

### PlayerService：

EnterGameAsync在有JwtToken时请求一次，将返回用户数据

EnterRoomAsync在玩家进入战场时请求一次，将保存用户选择的三种部件

ExitRoomAsync在玩家通过区域离开战场时请求一次，将保存用户在战场中缴获的物品、分数、人头数并返回本次的分数和人头数

DeathAsync在玩家在战场中死亡时请求一次，将清空用户本次在战场中缴获的物品、分数、人头数并返回本次的分数和人头数

CheckGlobalWeaponModuleAsync可以获取玩家已经带出去的所有武器配件

CheckRoomWeaponModuleAsync可以获取玩家在战场中缴获的武器配件

CheckGlobalRankAsync可以获取玩家已经获取的在战场外的总分数

CheckRoomRankAsync可以获取玩家已经获取的在战场内的总分数

UpdateRoomWeaponModuleAsync可以更新玩家在战场中获得的武器配件，在玩家舔包时调用，将玩家舔包后目前拥有的所有武器配件全部发送更新

UpdateRoomRank可以更新玩家在战场中分数，将玩家目前的击杀数和分数发送更新