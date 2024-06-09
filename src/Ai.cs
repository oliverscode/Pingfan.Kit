using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Pingfan.Kit;

/// <summary>
/// openai 对接, 默认使用gpt-3.5-turbo模型, 还可以选择gpt-4-turbo
/// </summary>
public class Ai
{
    private const string ApiUrl = "https://api.openai.com/v1/chat/completions";

    private readonly string _apiKey;

    /// <summary>
    /// 预设的系统消息
    /// </summary>
    public string SystemMessage { get; set; } = "请用简单的语言回答,不要说一下客套话,仅仅回答问题就好";

    /// <summary>
    /// 大语言模型, 默认gpt3.5模型
    /// </summary>
    public string Model { get; set; } = "gpt-3.5-turbo";

    /// <summary>
    /// 最大记住你最近的多少条对话
    /// </summary>
    public int MaxChatLength { get; set; } = 10;

    // public event Action<char> OnAnswer;

    // 对话历史
    private readonly List<object> _conversationHistory = new();

    private object _sendMessage;
    private object _receiveMessage;

    public Ai(string apiKey)
    {
        _apiKey = apiKey!;
    }

    private string Send(string message)
    {
        using var client = new WebClient();
        // 设置请求头
        client.Headers.Add("Content-Type", "application/json");
        client.Headers.Add("Authorization", $"Bearer {_apiKey}");


        _sendMessage = new { role = "user", content = message };

        var messages = new List<object>
        {
            new { role = "system", content = SystemMessage }
        };

        // 如果对话历史超过 MaxChatLength * 2, 就不要前面的对话, 但是记录还是得保留
        var history = new List<object>();
        if (_conversationHistory.Count > MaxChatLength * 2)
        {
            history.AddRange(_conversationHistory.GetRange(MaxChatLength * 2 - 2, MaxChatLength * 2));
        }
        else
        {
            history.AddRange(_conversationHistory);
        }

        messages.AddRange(history);
        messages.Add(_sendMessage);
        var data = new { model = Model, messages };

        // 获取结果
        var response = client.UploadString(ApiUrl, "POST", JsonSerializer.Serialize(data));
        var json = JsonDocument.Parse(response);
        var root = json.RootElement;
        var choices = root.GetProperty("choices");
        var choice = choices[0];
        var reply = choice.GetProperty("message");
        var replyContent = reply.GetProperty("content").GetString();
        _receiveMessage = new { role = "assistant", content = replyContent };

        _conversationHistory.Add(_sendMessage);
        _conversationHistory.Add(_receiveMessage);

        return replyContent!;
    }

    /// <summary>
    /// 对话
    /// </summary>
    public string Talk(string message)
    {
        return Send(message);

        // 更新对话历史，以便下一次请求
        /*{
    "id": "chatcmpl-9TMl5mrTVll80o7uj1u8keccmfNiU",
    "object": "chat.completion",
    "created": 1716786023,
    "model": "gpt-3.5-turbo-0125",
    "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "你好！我是一个基于自然语言处理技术的助手程序，可以帮助您回答问题和提供信息。有什么可以帮助您的吗？"
      },
      "logprobs": null,
      "finish_reason": "stop"
    }
    ],
    "usage": {
    "prompt_tokens": 29,
    "completion_tokens": 51,
    "total_tokens": 80
    },
    "system_fingerprint": null
    }
    */
    }
}